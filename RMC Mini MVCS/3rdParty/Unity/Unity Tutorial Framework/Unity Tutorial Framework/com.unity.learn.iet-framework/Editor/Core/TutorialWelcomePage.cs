using System;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// A generic event for signaling changes in a tutorial welcome page.
    /// Parameters: sender.
    /// </summary>
    [Serializable]
    public class TutorialWelcomePageEvent : UnityEvent<TutorialWelcomePage>
    {
    }

    /// <summary>
    /// Welcome page/dialog for a project shown using TutorialModalWindow.
    /// </summary>
    /// <remarks>
    /// In addition of window title, header image, title, and description,
    /// a welcome page/dialog contains a fully customizable button row.
    /// </remarks>
    public class TutorialWelcomePage : ScriptableObject
    {
        /// <summary>
        /// Data for a customizable button.
        /// </summary>
        [Serializable]
        public class ButtonData
        {
            /// <summary>
            /// Text of the button.
            /// </summary>
            public LocalizableString Text = new LocalizableString();
            /// <summary>
            /// Tooltip of the button.
            /// </summary>
            public LocalizableString Tooltip = new LocalizableString();
            /// <summary>
            /// Callback for the button click.
            /// </summary>
            public UnityEvent OnClick = new UnityEvent();
        }

        /// <summary>
        /// Raised when any welcome page is modified.
        /// </summary>
        /// <remarks>
        /// Raised before Modified event.
        /// </remarks>
        public static TutorialWelcomePageEvent TutorialWelcomePageModified = new TutorialWelcomePageEvent();

        /// <summary>
        /// Raised when any field of the welcome page is modified.
        /// </summary>
        public TutorialWelcomePageEvent Modified = new TutorialWelcomePageEvent();

        /// <summary>
        /// Header image of the welcome dialog.
        /// </summary>
        public Texture2D Image { get => m_Image; set => m_Image = value; }
        [SerializeField]
        Texture2D m_Image;

        /// <summary>
        /// Window title of the welcome dialog.
        /// </summary>
        public LocalizableString WindowTitle { get => m_WindowTitle; set => m_WindowTitle = value; }
        [SerializeField]
        internal LocalizableString m_WindowTitle;

        /// <summary>
        /// Title of the welcome dialog.
        /// </summary>
        public LocalizableString Title { get => m_Title; set => m_Title = value; }
        [SerializeField]
        internal LocalizableString m_Title;

        /// <summary>
        /// Description of the welcome dialog.
        /// </summary>
        public LocalizableString Description { get => m_Description; set => m_Description = value; }
        [SerializeField, LocalizableTextArea(1, 10)]
        internal LocalizableString m_Description;

        /// <summary>
        /// Buttons specified for the welcome page.
        /// </summary>
        public ButtonData[] Buttons { get => m_Buttons; set => m_Buttons = value; }
        [SerializeField]
        internal ButtonData[] m_Buttons;

        /// <summary>
        /// Raises the Modified events for this asset.
        /// </summary>
        public void RaiseModified()
        {
            TutorialWelcomePageModified?.Invoke(this);
            Modified?.Invoke(this);
        }

        void OnValidate()
        {
            Title = POFileUtils.SanitizeString(Title);
            WindowTitle = POFileUtils.SanitizeString(WindowTitle);
            Description = POFileUtils.SanitizeString(Description);
        }

        /// <summary>
        /// Creates a default Close button.
        /// </summary>
        /// <param name="page">Page for which the buttons is created.</param>
        /// <returns>Data structure for the button.</returns>
        public static ButtonData CreateCloseButton(TutorialWelcomePage page)
        {
            var data = new ButtonData { Text = "Close", OnClick = new UnityEvent() };
            UnityEventTools.AddVoidPersistentListener(data.OnClick, page.CloseCurrentModalDialog);
            data.OnClick.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
            return data;
        }

        // Providing functionality for some default behaviours of the welcome dialog.

        /// <summary>
        /// Closes the an open instance of TutorialModalWindow.
        /// </summary>
        public void CloseCurrentModalDialog()
        {
            var window = EditorWindowUtils.FindOpenInstance<TutorialModalWindow>();
            if (window)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Exits the Editor.
        /// </summary>
        public void ExitEditor()
        {
            EditorApplication.Exit(0);
        }
    }
}
