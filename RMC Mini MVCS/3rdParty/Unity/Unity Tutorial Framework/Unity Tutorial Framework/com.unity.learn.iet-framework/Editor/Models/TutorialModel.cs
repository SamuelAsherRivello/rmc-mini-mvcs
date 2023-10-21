using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Tutorials.Core.Editor
{
    [Serializable]
    struct SceneViewState
    {
        public bool In2DMode;
        public bool Orthographic;
        public float Size;
        public Vector3 Point;
        public Quaternion Direction;
    }

    [Serializable]
    struct SceneInfo
    {
        public bool Active;
        public string AssetPath;
        public bool WasLoaded;

        public SceneInfo(bool active, string assetPath, bool wasLoaded)
        {
            Active = active;
            AssetPath = assetPath;
            WasLoaded = wasLoaded;
        }
    }

    [Serializable]
    internal class TutorialModel : IModel
    {
        /// <summary>
        /// The original layout files are copied into this folder for modifications. 
        /// </summary>
        const string k_UserLayoutDirectory = "Temp";
        /// <summary>
        /// The original/previous layout is stored into this when loading new layouts.
        /// </summary>
        internal static readonly string k_LayoutBeforeTutorialStartedPath = $"{k_UserLayoutDirectory}/LayoutBeforeTutorialStarted.dwlt";
        internal static readonly EditorWaitForSeconds s_AutoAdvanceDelay = new EditorWaitForSeconds(0.5f);
        internal static readonly bool s_AuthoringModeEnabled = ProjectMode.IsAuthoringMode();
        internal Tutorial CurrentTutorial;

        internal bool CanMoveToNextPage => CurrentTutorial != null
                                        && CurrentTutorial.CanMoveToNextPage;

        /// <summary>
        /// Are we currently (during this frame) transitioning from one tutorial to another?
        /// </summary>
        /// <remarks>
        /// This transition typically happens when using a Switch Tutorial button on a tutorial page.
        /// </remarks>
        public bool IsTransitioningBetweenTutorials { get; internal set; }

        /// <summary>
        /// Are we currently loading a window layout.
        /// </summary>
        /// <remarks>
        /// A window layout load typically happens when the project is started for the first time
        /// and the project's startup settings specify a window layout for the project, or when entering
        /// or exiting a tutorial with a window layout specified.
        /// </remarks>
        public bool IsLoadingLayout => TutorialWindow.s_IsLoadingLayout;

        internal static event Action OnBeforeLayoutLoaded;
        internal static event Action<bool> OnLayoutLoaded; // bool == successful

        internal TutorialStyles Styles => TutorialProjectSettings.Instance.TutorialStyle;
        internal bool MaskingEnabled
        {
            get => MaskingManager.MaskingEnabled && (m_MaskingEnabled || !s_AuthoringModeEnabled);
            set { m_MaskingEnabled = value; }
        }

        internal bool PlayModeChanging => m_PlayModeChanging;

        internal bool SkipNextAutoAdvancing = false;

        [SerializeField]
        bool m_MaskingEnabled = true;

        [SerializeField]
        bool m_PlayModeChanging;

        [SerializeField]
        List<SceneInfo> m_ActiveScenesBeforeTutorialStarted = new List<SceneInfo>();

        [SerializeField]
        SceneViewState m_SceneViewStateBeforeTutorialStarted;

        /// <inheritdoc />
        public event Action StateChanged;

        /// <inheritdoc />
        public void OnStart()
        {
            EditorApplication.playModeStateChanged -= TrackPlayModeChanging;
            EditorApplication.playModeStateChanged += TrackPlayModeChanging;
        }

        /// <inheritdoc />
        public void OnStop()
        {
            if (!m_PlayModeChanging
            && CurrentTutorial != null
            && !TutorialWindow.Instance.Model.DomainReloadOccured)
            {
                AnalyticsHelper.TutorialEnded(TutorialConclusion.Quit);
            }
        }

        /// <inheritdoc />
        public void RestoreState([NotNull] IWindowCache cache)
        {
            //Debug.Log($"Restoring current tutorial: {CurrentTutorial?.GetInstanceID()}");
            m_PlayModeChanging = cache.PlayModeChanging;
            CurrentTutorial = cache.CurrentTutorial;
            m_ActiveScenesBeforeTutorialStarted = cache.ActiveScenesBeforeTutorialStarted;
            m_SceneViewStateBeforeTutorialStarted = cache.SceneViewStateBeforeTutorialStarted;
            StateChanged?.Invoke();
        }

        /// <inheritdoc />
        public void SaveState([NotNull] IWindowCache cache)
        {
            //Debug.Log($"Saving current tutorial: {CurrentTutorial?.GetInstanceID()}");
            cache.PlayModeChanging = m_PlayModeChanging;
            cache.CurrentTutorial = CurrentTutorial;
            cache.ActiveScenesBeforeTutorialStarted = m_ActiveScenesBeforeTutorialStarted;
            cache.SceneViewStateBeforeTutorialStarted = m_SceneViewStateBeforeTutorialStarted;
        }

        void TrackPlayModeChanging(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.ExitingPlayMode:
                    m_PlayModeChanging = true;
                    break;
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    m_PlayModeChanging = false;
                    break;
            }
        }

        /// <summary>
        /// Saves current state of open/loaded scenes so we can restore later
        /// </summary>
        internal void SaveOriginalScenes()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            m_ActiveScenesBeforeTutorialStarted = GetCurrentScenes().Select(s => new SceneInfo(s == activeScene, s.path, s.isLoaded)).ToList();
        }

        internal void SaveOriginalWindowLayout()
        {
            TutorialModalWindow.Hide();
            WindowLayoutProxy.SaveWindowLayout(k_LayoutBeforeTutorialStartedPath);
        }

        internal void SaveSceneViewState()
        {
            var sceneView = EditorWindow.GetWindow<SceneView>();
            m_SceneViewStateBeforeTutorialStarted.In2DMode = sceneView.in2DMode;
            m_SceneViewStateBeforeTutorialStarted.Point = sceneView.pivot;
            m_SceneViewStateBeforeTutorialStarted.Direction = sceneView.rotation;
            m_SceneViewStateBeforeTutorialStarted.Size = sceneView.size;
            m_SceneViewStateBeforeTutorialStarted.Orthographic = sceneView.orthographic;
        }

        internal static bool LoadWindowLayout(string path)
        {
            TutorialWindow.s_IsLoadingLayout = true;
            OnBeforeLayoutLoaded?.Invoke();
            /* the following instruction will cause a layout reload, which will cause the tutorial window to be closed and reopened.
             During the process, there will be a moment where two different instances of the Tutorial Window will be operating at the same time.
            This makes the Cache state unreliable during this method, hence the static flag for checking if the layout is reloading */
            bool successful = EditorUtility.LoadWindowLayout(path); // will log an error if fails
            TutorialWindow.s_IsLoadingLayout = false;
            OnLayoutLoaded?.Invoke(successful);
            return successful;
        }

        static List<Scene> GetCurrentScenes()
        {
            var scenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                scenes.Add(SceneManager.GetSceneAt(i));
            }
            return scenes;
        }

        internal IEnumerator ReopenActiveScenesAsBeforeTutorialStarted()
        {
            if (!m_ActiveScenesBeforeTutorialStarted.Any())
            {
                yield break;
            }

            if (EditorApplication.isPlaying)
            {
                // Exit play mode so we can open scenes (without necessarily loading them)
                EditorApplication.isPlaying = false;

                int currentFrameCount = Time.frameCount;
                while (currentFrameCount == Time.frameCount)
                {
                    yield return null; //going out of play mode requires a frame
                }
            }
            else
            {
                yield return null;
            }

            /* Close all existing scenes
            Closing all scenes allows us to retain the original order of scenes in the original scenes,
            would they contain same scenes as the tutorial. As we cannot remove all scenes, and must have
            at least one scene open at all times, create a dummy scene for the time being. */
            NewSceneMode dummySceneMode = SceneManager.GetActiveScene().path.IsNullOrEmpty() ? NewSceneMode.Single
                                                                                             : NewSceneMode.Additive; // prevents potential "Cannot create a new scene additively with an untitled scene unsaved" error
            Scene dummyScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, dummySceneMode);

            GetCurrentScenes().ForEach((scene) =>
            {
                if (scene != dummyScene)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
            });

            // Load original scenes
            foreach (var sceneInfo in m_ActiveScenesBeforeTutorialStarted)
            {
                if (sceneInfo.AssetPath.IsNullOrEmpty()) { continue; } // Skip new unsaved scenes

                OpenSceneMode openSceneMode = sceneInfo.WasLoaded ? OpenSceneMode.Additive : OpenSceneMode.AdditiveWithoutLoading;
                EditorSceneManager.OpenScene(sceneInfo.AssetPath, openSceneMode);
            }

            // Set original active scene
            var originalActiveScenePath = m_ActiveScenesBeforeTutorialStarted.Where(sceneInfo => sceneInfo.Active)
                                                          .Select(sceneInfo => sceneInfo.AssetPath)
                                                          .FirstOrDefault();

            foreach (var scene in GetCurrentScenes())
            {
                if (scene.path == originalActiveScenePath)
                {
                    SceneManager.SetActiveScene(scene);
                    break;
                }
            }

            // Clean up the dummy scene if we have real scenes.
            if (SceneManager.sceneCount > 1)
            {
                EditorSceneManager.CloseScene(dummyScene, true);
            }
            m_ActiveScenesBeforeTutorialStarted.Clear();
        }


        internal void RestoreSceneViewStateAsBeforeTutorialStarted()
        {
            var sceneView = EditorWindow.GetWindow<SceneView>();
            sceneView.in2DMode = m_SceneViewStateBeforeTutorialStarted.In2DMode;
            sceneView.LookAt
            (
                m_SceneViewStateBeforeTutorialStarted.Point,
                m_SceneViewStateBeforeTutorialStarted.Direction,
                m_SceneViewStateBeforeTutorialStarted.Size,
                m_SceneViewStateBeforeTutorialStarted.Orthographic,
                instant: true
            );
        }

        internal static void ReopenWindowLayoutAsBeforeTutorialStarted(Tutorial tutorial)
        {
            if ((tutorial && !tutorial.WindowLayout) // Restore layout only if the tutorial used window layout, meaning, the new auto-docking mechanism was not used.
            || !File.Exists(k_LayoutBeforeTutorialStartedPath))
            {
                return;
            }
            LoadWindowLayout(k_LayoutBeforeTutorialStartedPath);
            File.Delete(k_LayoutBeforeTutorialStartedPath);
        }

        internal static bool LoadWindowLayoutWorkingCopy(string path)
        {
            TutorialWindow.s_RebuildFrontendEvenIfIsLoadingLayout = true;
            return LoadWindowLayout(GetWorkingCopyWindowLayoutPath(path));
        }

        internal static string GetWorkingCopyWindowLayoutPath(string layoutPath) => $"{k_UserLayoutDirectory}/{new FileInfo(layoutPath).Name}";


        /// <summary>
        /// Makes a copy of the window layout file and replaces LastProjectPaths in the window layout
        /// so that pre-saved Project window states work correctly. Also resets TutorialWindow's readme in the layout.
        /// </summary>
        /// <param name="layoutPath"></param>
        /// <returns>Path to the new layout file. </returns>
        internal static string PrepareWindowLayout(string layoutPath)
        {
            try
            {
                if (!Directory.Exists(k_UserLayoutDirectory))
                {
                    Directory.CreateDirectory(k_UserLayoutDirectory);
                }

                var destinationPath = GetWorkingCopyWindowLayoutPath(layoutPath);
                File.Copy(layoutPath, destinationPath, overwrite: true);

                const string lastProjectPathProp = "m_LastProjectPath: ";
                const string readmeProp = "m_Readme: ";
                const string nullObject = "{fileID: 0}";
                string userProjectPath = Directory.GetCurrentDirectory();

                var fileContents = new List<string>();
                using (var reader = new StreamReader(destinationPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = ReplaceAfter(lastProjectPathProp, userProjectPath, line);
                        line = ReplaceAfter(readmeProp, nullObject, line);
                        fileContents.Add(line);
                    }
                }

                using (var writer = new StreamWriter(destinationPath, append: false))
                {
                    fileContents.ForEach(writer.WriteLine);
                }
                return destinationPath;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return string.Empty;
            }
        }



        static string ReplaceAfter(string before, string replaceWithThis, string lineToRead)
        {
            int index = lineToRead.IndexOf(before, StringComparison.Ordinal);
            if (index > -1)
            {
                lineToRead = lineToRead.Substring(0, index + before.Length) + replaceWithThis;
            }
            return lineToRead;
        }
    }
}
