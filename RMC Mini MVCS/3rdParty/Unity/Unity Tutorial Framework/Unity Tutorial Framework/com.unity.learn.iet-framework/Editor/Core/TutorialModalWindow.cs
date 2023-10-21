using System;
using System.Collections;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Tutorials.Core.Editor.Localization;
using static Unity.Tutorials.Core.Editor.RichTextParser;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// A modal/utility window that can display TutorialWelcomePage as its content.
    /// Optionally utilizes masking for modality.
    /// </summary>
    public class TutorialModalWindow : EditorWindow
    {
        static readonly Vector2 k_windowSize = new Vector2(700, 500);

        /// <summary>
        /// The current instance of this window
        /// </summary>
        public static TutorialModalWindow Instance { get; set; }
        static TutorialModalWindow FindInstance() => Resources.FindObjectsOfTypeAll<TutorialModalWindow>().FirstOrDefault();
        internal TutorialStyles Styles => TutorialProjectSettings.Instance.TutorialStyle;

        /// <summary>
        /// Does the window utilize masking for modality effect.
        /// </summary>
        /// <remarks>
        /// Remember to set prior to calling TryToShow().
        /// </remarks>
        public static bool MaskingEnabled { get; set; } = false;

        /// <summary>
        /// Is the window currently visible.
        /// </summary>
        [Obsolete("Will be removed in v4. Check the status of 'Instance' instead")] //todo: remove in v4
        public static bool Visible => Instance != null;

        /// <summary>
        /// In order to set the welcome page, use the Show() function instead.
        /// </summary>
        public TutorialWelcomePage WelcomePage
        {
            get => m_WelcomePage;
            private set
            {
                if (m_WelcomePage)
                {
                    m_WelcomePage.Modified.RemoveListener(OnWelcomePageModified);
                }

                m_WelcomePage = value;

                if (m_WelcomePage)
                {
                    m_WelcomePage.Modified.AddListener(OnWelcomePageModified);
                }
            }
        }
        [SerializeField]
        TutorialWelcomePage m_WelcomePage;

        Action m_OnClose;
        VisualElement m_Root;
        StyleSheet m_LastCommonStyleSheet; // Dark/Light theme

        void OnEnable()
        {
            SetupBackend();
            SetupFrontend();
        }

        void OnDisable()
        {
            TeardownBackend();
        }

        void OnDestroy() //aka: "When the user closes the window"
        {
            m_OnClose?.Invoke();
            Unmask();
            Instance = null;
        }

        void SetupBackend()
        {
            if (!Instance)
            {
                Instance = FindInstance();
            }
            SubscribeEvents();
        }

        void SetupFrontend()
        {
            m_Root = rootVisualElement;
            minSize = k_windowSize;
            maxSize = k_windowSize;
            RebuildFrontend();
        }

        void RebuildFrontend()
        {
            if (TutorialWindow.s_IsLoadingLayout) { return; }
            LoadUIStructure();
        }

        internal void LoadUIStructure()
        {
            m_Root.Clear();

            if (TutorialModel.s_AuthoringModeEnabled)
            {
                rootVisualElement.Add(new IMGUIContainer(OnGuiToolbar));
            }

            VisualTreeAsset windowContent = UIElementsUtils.LoadUXML("WelcomeDialog");
            windowContent.CloneTree(m_Root);

            //preserve the base style, remove all styles defined in UXML and apply new skin
            for (int i = m_Root.styleSheets.count - 1; i > 0; i--)
            {
                m_Root.styleSheets.Remove(m_Root.styleSheets[i]);
            }

            UIElementsUtils.LoadCommonStyleSheet(m_Root);
            UpdateWindowSkin();

            EditorCoroutineUtility.StartCoroutine(LoadContent(), this);
        }


        IEnumerator LoadContent()
        {
            while (m_WelcomePage == null)
            {
                yield return null;
            }
            UpdateContent();
            if (MaskingEnabled)
            {
                Mask();
            }
        }

        void UpdateWindowSkin()
        {
            UIElementsUtils.RemoveStyleSheet(m_LastCommonStyleSheet, m_Root);
            UIElementsUtils.LoadSkinStyleSheet(out m_LastCommonStyleSheet, m_Root);
        }

        void SubscribeEvents()
        {
            if (m_WelcomePage)
            {
                m_WelcomePage.Modified.AddListener(OnWelcomePageModified);
            }
        }

        void UnsubscribeEvents()
        {
            if (m_WelcomePage)
            {
                m_WelcomePage.Modified.RemoveListener(OnWelcomePageModified);
            }
        }

        void TeardownBackend()
        {
            UnsubscribeEvents();
        }

        /// <summary>
        /// Shows the window using the provided content.
        /// </summary>
        /// <remarks>
        /// Shown as a utility window, https://docs.unity3d.com/ScriptReference/EditorWindow.ShowUtility.html
        /// </remarks>
        /// <param name="welcomePage">Content to be shown.</param>
        /// <param name="onClose">Optional callback to be called when the window is closed.</param>
        public static void Show(TutorialWelcomePage welcomePage, Action onClose = null)
        {
            Hide();
            var window = CreateInstance<TutorialModalWindow>();
            window.titleContent = new GUIContent(welcomePage.WindowTitle);
            window.minSize = k_windowSize;
            window.maxSize = k_windowSize;
            window.m_OnClose = onClose;
            window.WelcomePage = welcomePage;

            window.ShowUtility();
            EditorWindowUtils.CenterOnMainWindow(window); // NOTE: positioning must be done after Show() in order to work.
        }

        /// <summary>
        /// Closes the window if it's open
        /// </summary>
        public static void Hide()
        {
            Instance?.Close();
        }

        void UpdateContent()
        {
            if (!WelcomePage)
            {
                Debug.LogError("null WelcomePage.");
                return;
            }

            Instance.titleContent = new GUIContent(WelcomePage.WindowTitle);

            VisualElement header = m_Root.Q("HeaderMedia");
            header.style.backgroundImage = Background.FromTexture2D(WelcomePage.Image);

            UIElementsUtils.ShowOrHide("HeaderContainer", m_Root, WelcomePage.Image != null);
            UIElementsUtils.SetupLabel("Heading", WelcomePage.Title, m_Root, false);

            RichTextToVisualElements(WelcomePage.Description, m_Root.Q("Description"));
            AddDynamicButtonsToContent();
        }

        void AddDynamicButtonsToContent()
        {
            VisualElement buttonContainer = m_Root.Q("ButtonContainer");
            buttonContainer.Clear();

            foreach (var buttonData in WelcomePage.Buttons.Where(buttonData => buttonData.Text.Value.IsNotNullOrEmpty()))
            {
                var button = new Button(() => buttonData.OnClick?.Invoke())
                {
                    text = buttonData.Text,
                    tooltip = buttonData.Tooltip
                };
                buttonContainer.Add(button);
            } 
        }

        void OnGuiToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));

            GUILayout.FlexibleSpace();

            EditorGUI.BeginChangeCheck();
            MaskingEnabled = GUILayout.Toggle
            (
                MaskingEnabled, TutorialWindow.IconContent("Mask Icon", Tr(LocalizationKeys.k_IconPreviewMaskingTooltip)),
                EditorStyles.toolbarButton, GUILayout.Width(TutorialWindow.k_AuthoringButtonWidth)
            );
            if (EditorGUI.EndChangeCheck())
            {
                if (MaskingEnabled)
                {
                    Mask();
                }
                else
                {
                    Unmask();
                }
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndHorizontal();
        }

        void Mask()
        {
            var unmaskedViews = new UnmaskedView.MaskData();
            unmaskedViews.AddParentFullyUnmasked(this);
            var highlightedViews = new UnmaskedView.MaskData();

            MaskingManager.Mask
            (
                unmaskedViews,
                Styles.MaskingColor,
                highlightedViews,
                Styles.HighlightColor,
                Styles.BlockedInteractionColor,
                Styles.HighlightThickness
            );

            MaskingEnabled = true;
        }

        void Unmask()
        {
            MaskingManager.Unmask();
            MaskingEnabled = false;
        }

        void OnWelcomePageModified(TutorialWelcomePage sender)
        {
            UpdateContent();
        }
    }
}
