using System.Linq;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    using static Localization;

    static class FrameworkSettings
    {
        internal const string k_PackageName = "com.unity.learn.iet-framework";
        static readonly float k_OriginalLabelWidth = EditorGUIUtility.labelWidth;
        static readonly string k_Category = Tr(LocalizationKeys.k_SettingsCategory);

        static Settings s_Instance;
        internal static Settings Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new Settings(k_PackageName);
                }
                return s_Instance;
            }
        }

        [SettingsProviderGroup]
        static SettingsProvider[] CreateSettingsProviders()
        {
            /* We need to add the name of the each setting on our own as keywords as we don't use the default
            UserSettingsProvider because it doesn't support localization. Add also "iet" shortcut, allowing "iet some setting" searches. */
            var keywords = new[]
            {
                "iet",
                MaskingManager.MaskingEnabled.Name,
                SerializedTypeDrawer.ShowSimplifiedTypeNames.Name,
                SerializedTypeDrawer.UseDefaultEditors.Name,
                TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog.Name,
            };
            var userSettings = new SettingsProvider("Preferences/" + k_Category, SettingsScope.User, keywords)
            {
                guiHandler = (searchContext) => DrawSettings(searchContext, DrawUserSettings)
            };

            var projectSettingsKeywords = new string[]
            {
                TutorialFrameworkModel.s_DisplayWelcomeDialogOnStartup.Name
            };

            var projectSettings = new SettingsProvider("Project/" + k_Category, SettingsScope.Project, projectSettingsKeywords)
            {
                guiHandler = (searchContext) => DrawSettings(searchContext, DrawProjectSettings)
            };
            return new[] { userSettings, projectSettings };
        }

        static void SetLabelWidth(float width) { EditorGUIUtility.labelWidth = width; }
        static void RestoreOriginalLabelWidth() { EditorGUIUtility.labelWidth = k_OriginalLabelWidth; }

        static bool DrawToggle(BaseSetting<bool> value, string searchContext)
        {
            return SettingsGUILayout.SettingsToggle(value.GetGuiContent(), value, searchContext);
        }

        static void DrawUserSettings(string searchContext)
        {
            MaskingManager.MaskingEnabled.value = DrawToggle(MaskingManager.MaskingEnabled, searchContext);
            SerializedTypeDrawer.ShowSimplifiedTypeNames.value = DrawToggle(SerializedTypeDrawer.ShowSimplifiedTypeNames, searchContext);
            SerializedTypeDrawer.UseDefaultEditors.value = DrawToggle(SerializedTypeDrawer.UseDefaultEditors, searchContext);
            TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog.value = DrawToggle(TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog, searchContext);
        }

        static void DrawProjectSettings(string searchContext)
        {
            TutorialFrameworkModel.s_DisplayWelcomeDialogOnStartup.value = DrawToggle(TutorialFrameworkModel.s_DisplayWelcomeDialogOnStartup, searchContext);
        }

        static void DrawSettings(string searchContext, System.Action<string> drawIndentGroupContent)
        {
            SetLabelWidth(300);
            // Space and indentation to mimic the default settings GUI layout as closely as possible.
            EditorGUILayout.Space();

            using (new SettingsGUILayout.IndentedGroup())
            {
                drawIndentGroupContent.Invoke(searchContext);
            }
            RestoreOriginalLabelWidth();
        }
    }
}
