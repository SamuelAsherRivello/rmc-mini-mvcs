using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Wrapper for defining IET Preferences (User Settings) conveniently.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserSetting<T> : BaseSetting<T>
    {
        /// <summary>
        /// Constructs the setting.
        /// </summary>
        /// <param name="name">Use Tr() in order to have localization support.</param>
        /// <param name="key">Key for the JSON file.</param>
        /// <param name="value"></param>
        /// <param name="tooltip">Use Tr() in order to have localization support.</param>
        public UserSetting(string key, string name, T value, string tooltip = null)
            : base(key, name, value, SettingsScope.User, tooltip)
        {
        }
    }

    /// <summary>
    /// Wrapper for defining IET Project Settings conveniently.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// If you wish to commit the changes to JSON immediately, use SetValue(value, saveProjectSettingsImmediately:true)
    /// or FrameworkSettings.Instance.Save(), otherwise changes are committed only on application quit or assembly reload.
    /// </remarks>
    public class ProjectSetting<T> : BaseSetting<T>
    {
        /// <summary>
        /// Constructs the setting.
        /// </summary>
        /// <param name="key">Key for EditorPrefs.</param>
        /// <param name="name">Use Tr() in order to have localization support.</param>
        /// <param name="value"></param>
        /// <param name="tooltip">Use Tr() in order to have localization support.</param>
        public ProjectSetting(string key, string name, T value, string tooltip = null)
            : base(key, name, value, SettingsScope.Project, tooltip)
        {
        }
    }

    /// <summary>
    /// Base class for implementing Tutorial Framework settings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseSetting<T> : UnityEditor.SettingsManagement.UserSetting<T>
    {
        /// <summary>
        /// Constructs the setting.
        /// </summary>
        /// <param name="key">Key for EditorPrefs (User) or JSON file (Project).</param>
        /// <param name="name">Use Tr() in order to have localization support.</param>
        /// <param name="value"></param>
        /// <param name="scope"></param>
        /// <param name="tooltip">Use Tr() in order to have localization support.</param>
        public BaseSetting(string key, string name, T value, SettingsScope scope, string tooltip = null)
            : base(FrameworkSettings.Instance, key, value, scope)
        {
            Name = name;
            Tooltip = tooltip;
        }

        /// <summary>
        /// Name of the setting.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tooltip for the setting.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Returns Name and Tooltip as GUIContent.
        /// </summary>
        /// <returns></returns>
        public GUIContent GetGuiContent() => new GUIContent(Name, Tooltip);
    }
}
