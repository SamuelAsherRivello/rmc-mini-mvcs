using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that a specific scene is added to the build.
    /// </summary>
    public class SceneAddedToBuildCriterion : Criterion
    {
        /// <summary>
        /// The scene that needs to be added to the build.
        /// </summary>
        public SceneAsset Scene { get => m_Scene; set => m_Scene = value; }
        [SerializeField]
        SceneAsset m_Scene;

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public override void StartTesting()
        {
            base.StartTesting();
            UpdateCompletion();

            EditorBuildSettings.sceneListChanged += UpdateCompletion;
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
            EditorBuildSettings.sceneListChanged -= UpdateCompletion;
        }

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            if (m_Scene == null)
            {
                return false;
            }
            if (m_Scene)
            {
                var scenePath = AssetDatabase.GetAssetPath(m_Scene);
                foreach (var scene in EditorBuildSettings.scenes)
                {
                    if (scene.enabled)
                    {
                        if (scene.path == scenePath)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            if (m_Scene == null)
            {
                return false;
            }

            // Look if scene is already added in the build settings
            bool addedScene = SceneIsAddedToBuildSettings(m_Scene, true);
            if (addedScene)
            {
                return true;
            }

            // If the scene does not exist, we add it
            AddSceneToBuildSettings(m_Scene);

            return true;
        }

        /// <summary>
        /// Is a specific Scene added to Build Settings.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="forceEnable"></param>
        /// <returns></returns>
        public static bool SceneIsAddedToBuildSettings(SceneAsset asset, bool forceEnable = false)
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                return false;
            }
            var scenePath = AssetDatabase.GetAssetPath(asset);
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.path == scenePath)
                {
                    if (forceEnable)
                    {
                        scene.enabled = true;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a Scene to Build Settings.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="enabled"></param>
        public static void AddSceneToBuildSettings(SceneAsset scene, bool enabled = true)
        {
            var scenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length + 1];
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
                scenes[i] = EditorBuildSettings.scenes[i];
            }

            scenes[scenes.Length - 1] = new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(scene), enabled);

            EditorBuildSettings.scenes = scenes;
        }
    }
}
