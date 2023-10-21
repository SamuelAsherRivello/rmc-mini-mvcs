using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Runs IET project initialization logic.
    /// </summary>
    [InitializeOnLoad]
    public static class UserStartupCode
    {
        const string k_DefaultsFolder = "Tutorial Defaults";
        const string k_EditorLanguageInitializedState = "EditorLanguageInitialized";
        const string k_EditorLanguagePreference = "EditorLanguage";

        static bool DisplayWelcomeDialogOnStartup
        {
            get => TutorialFrameworkModel.s_DisplayWelcomeDialogOnStartup;
            set => TutorialFrameworkModel.s_DisplayWelcomeDialogOnStartup.SetValue(value, true);
        }

        static bool IsLanguageInitialized() => SessionState.GetBool(k_EditorLanguageInitializedState, false);
        static void SetLanguageInitialized() => SessionState.SetBool(k_EditorLanguageInitializedState, true);
        static SystemLanguage LoadPreviousEditorLanguage() => (SystemLanguage)EditorPrefs.GetInt(k_EditorLanguagePreference, (int)SystemLanguage.English);
        static void SaveCurrentEditorLanguage() => EditorPrefs.SetInt(k_EditorLanguagePreference, (int)LocalizationDatabaseProxy.currentEditorLanguage);

        static UserStartupCode()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode
            || TutorialWindow.s_IsLoadingLayout)
            {
                return;
            }

            // Language change triggers an assembly reload.
            if (LoadPreviousEditorLanguage() != LocalizationDatabaseProxy.currentEditorLanguage)
            {
                SaveCurrentEditorLanguage();
                // There are several smaller and bigger localization issues with if we don't restart
                // the Editor so let's query the user to do so.
                string title = Localization.Tr(LocalizationKeys.k_TOCLabelTitle);
                string message = Localization.Tr(LocalizationKeys.k_LanguageDialogMessage);
                string ok = Localization.Tr(LocalizationKeys.k_LanguageDialogButtonOk);
                string cancel = Localization.Tr(LocalizationKeys.k_LanguageDialogButtonCancel);
                if (EditorUtility.DisplayDialog(title, message, ok, cancel))
                {
                    RestartEditor();
                }
            }

            EditorApplication.update += InitRunStartupCode;
        }

        internal static void RunStartupCode(TutorialProjectSettings projectSettings)
        {
            if (projectSettings.InitialScene != null)
            {
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(projectSettings.InitialScene));
            }

            BackupProjectAssets();

            // Ensure Editor is in predictable state
            EditorPrefs.SetString("ComponentSearchString", string.Empty);
            Tools.current = Tool.Move;

            if (TutorialEditorUtils.FindAssets<TutorialContainer>().Any())
            {
                var existingWindow = EditorWindowUtils.FindOpenInstance<TutorialWindow>();
                if (existingWindow)
                {
                    existingWindow.Close();
                }
                TutorialWindow.ShowWindow();
            }

            // NOTE camera settings can be applied successfully only after potential layout changes
            if (projectSettings.InitialCameraSettings != null && projectSettings.InitialCameraSettings.Enabled)
            {
                projectSettings.InitialCameraSettings.Apply();
            }

            if (projectSettings.WelcomePage)
            {
                TutorialModalWindow.Show(projectSettings.WelcomePage);
            }
        }

        static void InitRunStartupCode()
        {
            if (LocalizationDatabaseProxy.enableEditorLocalization && !IsLanguageInitialized())
            {
                /* Need to Request a script reload in order overcome Editor Localization issues
                with static initialization when opening the project for the first time. */
                SetLanguageInitialized();
                EditorUtility.RequestScriptReload();
                return;
            }

            /* Prepare the layout always. For example, the user might have moved the project around,
            so we need to ensure the file paths in the layouts are correct. */
            TutorialController.PrepareWindowLayouts();
            EditorApplication.update -= InitRunStartupCode;

            if (!DisplayWelcomeDialogOnStartup)
            {
                return;
            }

            DisplayWelcomeDialogOnStartup = false;
            RunStartupCode(TutorialProjectSettings.Instance);
        }

        /// <summary>
        /// Restart the Editor.
        /// </summary>
        internal static void RestartEditor()
        {
            // In older versions, calling EditorApplication.OpenProject() while having unsaved modifications
            // can cause us to get stuck in a dialog loop. This seems to be fixed in 2020.1 (and newer?).
            // As a workaround, ask for saving before starting to restart the Editor for real. However,
            // we get the dialog twice and it can cause issues if user chooses first "Don't save" and then tries
            // to "Cancel" in the second dialog.
#if !UNITY_2020_1_OR_NEWER
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
#endif
            {
                EditorApplication.OpenProject(".");
            }
        }

        internal static void BackupProjectAssets()
        {
            if (!TutorialProjectSettings.Instance.RestoreAssetsBackupOnTutorialReload)
            {
                return;
            }

            if (EditorApplication.isPlaying)
            {
                Debug.LogError("Defaults cannot be written during play mode");
                return;
            }

            string defaultsPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, k_DefaultsFolder);
            DirectoryInfo defaultsDirectory = new DirectoryInfo(defaultsPath);
            if (defaultsDirectory.Exists)
            {
                foreach (var file in defaultsDirectory.GetFiles())
                {
                    file.Delete();
                }
                foreach (var directory in defaultsDirectory.GetDirectories())
                {
                    directory.Delete(true);
                }
            }
            DirectoryCopy(Application.dataPath, defaultsPath);
        }

        internal static void DirectoryCopy(string sourceDirectory, string destinationDirectory, HashSet<string> dirtyMetaFiles = default)
        {
            var sourceDir = new DirectoryInfo(sourceDirectory);
            if (!sourceDir.Exists)
            {
                return;
            }

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            foreach (var file in sourceDir.GetFiles())
            {
                string tempPath = Path.Combine(destinationDirectory, file.Name);
                if (dirtyMetaFiles != null && string.Equals(Path.GetExtension(tempPath), ".meta", StringComparison.OrdinalIgnoreCase))
                {
                    if (!File.Exists(tempPath)
                    || !File.ReadAllBytes(tempPath).SequenceEqual(File.ReadAllBytes(file.FullName)))
                    {
                        dirtyMetaFiles.Add(tempPath);
                    }
                }
                file.CopyTo(tempPath, true);
            }

            foreach (var subdir in sourceDir.GetDirectories())
            {
                string tempPath = Path.Combine(destinationDirectory, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, dirtyMetaFiles);
            } 
        }

        /// <summary>
        /// Shows Tutorials window using the currently specified behaviour.
        /// </summary>
        /// <returns>The displayed TutorialWindow</returns>
        [Obsolete("Will be removed in v4. Use TutorialWindow.ShowWindow() instead")] //todo: remove in v4
        public static TutorialWindow ShowTutorialWindow()
        {
            return TutorialWindow.ShowWindow();
        }
    }
}
