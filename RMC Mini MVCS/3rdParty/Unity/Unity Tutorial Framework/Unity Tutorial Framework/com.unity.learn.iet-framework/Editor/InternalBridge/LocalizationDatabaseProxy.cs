using UnityEngine;
using UnityEditor;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Proxy class for accessing UnityEditor.LocalizationDatabase.
    /// </summary>
    internal class LocalizationDatabaseProxy
    {
        /// <summary>
        /// Is Editor Localization enabled.
        /// </summary>
        public static bool enableEditorLocalization
        {
            get => LocalizationDatabase.enableEditorLocalization;
            set => LocalizationDatabase.enableEditorLocalization = value;
        }

        /// <summary>
        /// Returns the current Editor language.
        /// </summary>
        public static SystemLanguage currentEditorLanguage => LocalizationDatabase.currentEditorLanguage;

        /// <summary>
        /// Clears the localization table cache
        /// </summary>
        public static void ClearLocalizationCache()
        {
            UnityEditor.L10n.ClearCache();
        }
    }
}
