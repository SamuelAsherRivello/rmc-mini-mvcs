using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_2020_1_OR_NEWER
[assembly: UnityEditor.Localization]
#elif UNITY_2019_3_OR_NEWER
[assembly: UnityEditor.Localization.Editor.Localization]
#endif

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// A helper class for Localization.
    /// </summary>
    static class Localization 
    {
        const string k_EnableEditorLocalizationPreference = "Editor.kEnableEditorLocalization";
        const string k_EditorLocalePreference = "Editor.kEditorLocale";
        internal const string k_EnableEditorLocalizationProperty = "enableEditorLocalization";
        internal const string k_CurrentEditorLanguageProperty = "currentEditorLanguage";
        internal const string k_GetDefaultEditorLanguageMethod = "GetDefaultEditorLanguage";

        [InitializeOnLoadMethod]
        static void ForceEnableLocalization()
        {
            if (EditorPrefs.GetBool(k_EnableEditorLocalizationPreference)) { return; }

            var localizationDatabase = Type.GetType("UnityEditor.LocalizationDatabase, UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            var enableEditorLocalization = localizationDatabase.GetProperty(k_EnableEditorLocalizationProperty);

            enableEditorLocalization.SetValue(null, true);
            EditorPrefs.SetBool(k_EnableEditorLocalizationPreference, true);

            string currentLanguage = EditorPrefs.GetString(k_EditorLocalePreference);
            if (!string.IsNullOrEmpty(currentLanguage)) { return; }

            var currentEditorLanguage = localizationDatabase.GetProperty(k_CurrentEditorLanguageProperty);
            SystemLanguage defaultEditorLanguage = (SystemLanguage)localizationDatabase.GetMethod(k_GetDefaultEditorLanguageMethod, BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
            currentEditorLanguage.SetValue(null, defaultEditorLanguage);

            EditorPrefs.SetString(k_EditorLocalePreference, currentEditorLanguage.GetValue(null).ToString());
        } 

        /// <summary>
        /// Routes the call to the correct, or none, Tr() implementation depending on the used Unity version.
        /// See https://docs.unity3d.com/ScriptReference/Localization.Editor.Localization.Tr.html.
        /// </summary>
        /// <param name="stringID"></param>
        /// <returns></returns>
        internal static string Tr(string stringID)
        {
            if (string.IsNullOrEmpty(stringID))
            {
                return string.Empty;
            }

#if UNITY_2020_1_OR_NEWER
            return UnityEditor.L10n.Tr(stringID);
#elif UNITY_2019_3_OR_NEWER
            return UnityEditor.Localization.Editor.Localization.Tr(stringID);
#else
            return stringID;
#endif
        }
    }
}
