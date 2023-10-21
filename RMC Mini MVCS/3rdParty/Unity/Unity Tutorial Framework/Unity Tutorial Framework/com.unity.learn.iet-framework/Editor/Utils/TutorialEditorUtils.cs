using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Contains different utilities used in Tutorials custom editors
    /// </summary>
    public static class TutorialEditorUtils
    {
        /// <summary>
        /// Same as ProjectWindowUtil.GetActiveFolderPath() but works also in 1-panel view.
        /// </summary>
        /// <returns>Returns path with forward slashes</returns>
        public static string GetActiveFolderPath()
        {
            return Selection.assetGUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path =>
                {
                    if (Directory.Exists(path))
                        return path;
                    if (File.Exists(path))
                        return Path.GetDirectoryName(path);
                    return path;
                })
                .FirstOrDefault()
                .AsNullIfEmpty()
                ?? "Assets";
        }

        /// <summary>
        /// Find assets of type T in the project.
        /// </summary>
        /// <typeparam name="T">Type of assets to look for.</typeparam>
        /// <returns>Assets of type T found in the project.</returns>
        public static IEnumerable<T> FindAssets<T>() where T : UnityEngine.Object =>
            AssetDatabase.FindAssets($"t:{typeof(T).FullName}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>);

        /// <summary>
        /// Checks if a UnityEvent property is not in a specific execution state
        /// </summary>
        /// <param name="eventProperty">A property representing the UnityEvent (or derived class)</param>
        /// <param name="state"></param>
        /// <returns>True if the event is in the expected state</returns>
        internal static bool EventIsNotInState(SerializedProperty eventProperty, UnityEngine.Events.UnityEventCallState state)
        {
            SerializedProperty persistentCalls = eventProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");
            for (int i = 0; i < persistentCalls.arraySize; i++)
            {
                if (persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_CallState").intValue == (int)state)
                    continue;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Renders a warning box about the state of the event
        /// </summary>
        internal static void RenderEventStateWarning()
        {
            EditorGUILayout.HelpBox(Localization.Tr(LocalizationKeys.k_TutorialPageLabelEventStateWarning), MessageType.Warning);
        }

        /// <summary>
        /// Opens an Url in the browser.
        /// Links to Unity's websites will open only if the user is logged in.
        /// </summary>
        /// <param name="url"></param>
        public static void OpenUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            string urlWithoutHttpPrefix = RemoveHttpProtocolPrefix(url);
            if (IsUnityUrlRequiringAuthentication(urlWithoutHttpPrefix) && UnityConnectSession.loggedIn)
            {
                //unity websites can only be opened if they have the https prefix, we need to ensure that it exists otherwise URLs like "unity.com" won't be opened at all
                UnityConnectSession.OpenAuthorizedURLInWebBrowser(EnsureProtocolPrefixIsPresent(url, "https"));
                return;
            }

            /* Important: as its documentation says, this API is extremely powerful and could be exploited by malicius users trying to run protocols different than HTTP.
             * Moreover, it ignores urls that dont' have a 3rd domain level. In order to address both issues, we need to ensure URLs have the https prefix
             * otherwise URLs like "google.com" won't be opened at all */
            Application.OpenURL(EnsureProtocolPrefixIsPresent(url, "http"));
        }


        internal static string EnsureProtocolPrefixIsPresent(string url, string protocol)
        {
            if (url.StartsWith(protocol, System.StringComparison.OrdinalIgnoreCase))
            {
                return url;
            }
            return $"{protocol}://{url}";
        }

        /// <summary>
        /// Removes "http://" and "https://" prefixes from an url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static string RemoveHttpProtocolPrefix(string url)
        {
            if (url.StartsWith("http", System.StringComparison.OrdinalIgnoreCase))
            {
                return url.Split(new string[] { "//" }, System.StringSplitOptions.None)[1];
            }
            return url;
        }

        /// <summary>
        /// Is this a Unity URL that requires Authentication?
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static bool IsUnityUrlRequiringAuthentication(string url)
        {
            // TODO Genesis will provide an API where we can keep a list of Unity URLs that we want to support.
            url = RemoveHttpProtocolPrefix(url);
            var splitUrl = url.Split('/')[0].ToLower();

            return (url.StartsWith("unity.", System.StringComparison.OrdinalIgnoreCase) || splitUrl.Contains(".unity."))
                   && !splitUrl.Contains("assetstore");
        }
    }
}
