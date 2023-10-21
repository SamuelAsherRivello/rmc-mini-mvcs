using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Contains the names for the menu items used by the In-Editor Tutorials packages.
    /// </summary>
    public static class MenuItems
    {
        /// <summary>
        /// Name of the main menu used by the package.
        /// </summary>
        public const string Menu = "Tutorials";
        /// <summary>
        /// Path for menu. Append menu item names to this string.
        /// </summary>
        public const string MenuPath = Menu + "/";
        /// <summary>
        /// The default menu item for showing the tutorials in the project.
        /// </summary>
        public const string ShowTutorials = "Show Tutorials";
        /// <summary>
        /// Menu path for the authoring submenu.
        /// </summary>
        public const string AuthoringMenuPath = Menu + "/Authoring/";

        [MenuItem(MenuPath + "Welcome Dialog")]
        static void OpenWelcomeDialog()
        {
            var welcomePage = TutorialProjectSettings.Instance.WelcomePage;
            if (welcomePage != null)
                TutorialModalWindow.Show(welcomePage);
            else
                Debug.LogError("No TutorialProjectSettings.WelcomePage set.");
        }

        [MenuItem(MenuPath + ShowTutorials)]
        static void OpenTutorialWindow()
        {
            TutorialWindow.ShowWindow();
        }
    }
}
