using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// The window used to display tutorials and their content
    /// </summary>
#pragma warning disable 0618 //suppress obsolete warning for us, keep them active for users
    public sealed class TutorialWindow : EditorWindowProxy
#pragma warning restore 0618
    {
        static readonly Vector2 k_MinWindowSize = new Vector2(400, 600f);
        static readonly Vector2 k_MaxWindowSize = new Vector2(600, 1200f);

        /// <summary>
        /// Should we show the Close Tutorials info dialog for the user for the current project.
        /// By default the dialog is shown once per project and disabled after that.
        /// </summary>
        /// <remarks>
        /// You want to set this typically to false when running unit tests.
        /// </remarks>
        [Obsolete("Has no effect in v3, and will be removed in v4.")] //moved to TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog //todo: remove in v4
        public static ProjectSetting<bool> ShowTutorialsClosedDialog = new ProjectSetting<bool>("IET.ShowTutorialsClosedDialog", "Show info dialog when the window is closed", true);

        /// <summary>
        /// Are we currently (during this frame) transitioning from one tutorial to another?
        /// </summary>
        /// <remarks>
        /// This transition typically happens when using a Switch Tutorial button on a tutorial page.
        /// </remarks>
        public bool IsTransitioningBetweenTutorials => Model.Tutorial.IsTransitioningBetweenTutorials;

        /// <summary>
        /// The currently active tutorial, if any.
        /// </summary>
        public Tutorial CurrentTutorial => Model.Tutorial.CurrentTutorial;

        /// <summary>
        /// The category of which tutorial are being displayed. Null if the project and its packages contains no categories, meaning all tutorials are being displayed.
        /// </summary>
        public TutorialContainer CurrentCategory => Model.TableOfContent.CurrentCategory;

        /// <summary>
        /// Active container of which tutorials we are viewing.
        /// </summary>
        [Obsolete("Will be removed in v4. Use CurrentCategory instead")] //todo: remove in v4
        public TutorialContainer ActiveContainer
        {
            get { return Model.TableOfContent.CurrentCategory; }
            set
            {
                Broadcast(new CategoryClickedEvent(value));
            }
        }

        /// <summary>
        /// Sets the containers as available for selection as tutorial projects.
        /// </summary>
        /// <param name="containers">Container selection.</param>
        /// <remarks>
        /// ActiveContainer must be set to null in order to view the selection.
        /// </remarks>
        [Obsolete("Will be removed in v4. Categories loading/unloading is now managed automatically by the TutorialWindow")] //todo: remove in v4
        public void SetContainers(IEnumerable<TutorialContainer> containers) { }

        /// <summary>
        /// Clears any containers this window might be showing as available tutorial projects.
        /// </summary>
        /// <remarks>
        /// If we were viewing the container selection, the window is cleared.
        /// </remarks>
        [Obsolete("Will be removed in v4.  Categories loading/unloading is now managed automatically by the TutorialWindow")] //todo: remove in v4
        public void ClearContainers() { }

        /// <summary>
        /// Are we currently loading a window layout.
        /// </summary>
        /// <remarks>
        /// A window layout load typically happens when the project is started for the first time
        /// and the project's startup settings specify a window layout for the project, or when entering
        /// or exiting a tutorial with a window layout specified.
        /// </remarks>
        internal static bool s_IsLoadingLayout = false;

        /// <summary>
        /// Should the UI be rebuilt even if the layout has been reloaded?
        /// Usually needed when opening a closed tutorial window from the menu item.
        /// </summary>
        internal static bool s_RebuildFrontendEvenIfIsLoadingLayout = false;

        internal EventManager EventManager = new EventManager();
        /// <summary>
        /// The active instance of this window
        /// </summary>
        public static TutorialWindow Instance { get; set; }
        static TutorialWindow FindInstance() => Resources.FindObjectsOfTypeAll<TutorialWindow>().FirstOrDefault();
        /// <summary>
        /// Returns true or false depending if localization is still initializing or not
        /// </summary>
        internal bool IsWaitingForLocalizationToBeReady { get; private set; } = true;
        /// <summary>
        /// True if the basic frontend data of the Window is set, meaning that specific Views can be loaded
        /// </summary>
        internal bool FrontendIsReadyToBeInitialized { get; private set; }

        [SerializeField]
        internal string CurrentView
        {
            get => m_Model.CurrentView;
            private set
            {
                m_Model.PreviousView = CurrentView;
                m_Model.CurrentView = value;
            }
        }

        internal TutorialFrameworkModel Model => m_Model;

        TutorialFrameworkController m_Controller;
        TutorialFrameworkModel m_Model;
        HashSet<IModel> m_Models;
        HashSet<Controller> m_Controllers;
        TableOfContentModel m_TableOfContentModel;
        TutorialModel m_TutorialModel;

        VisualElement m_Root;
        HashSet<View> m_Views;
        internal TableOfContentView TableOfContentView { get; private set; }
        internal TutorialView TutorialView { get; private set; }

        StyleSheet m_LastCommonStyleSheet; // Dark/Light theme

        /// <summary>
        /// Holds all the Frontend setup methods of the available tabs
        /// </summary>
        Dictionary<string, Action> m_ViewFrontendSetupMethods;

        /// <summary>
        /// Shows Tutorials window using the currently specified behaviour.
        /// </summary>
        /// <remarks>
        /// Different behaviors:
        /// 1. If a single root tutorial container (TutorialContainer.ParentContainer is null) that has Project Layout specified exists,
        ///    the window is loaded and shown using the specified project window layout (old behaviour).
        ///    If the project layout does not contain Tutorials window, the window is shown an as a free-floating window.
        /// 2. If no root tutorial containers exist, or a root container's Project Layout is not specified, the window is shown
        ///     by anchoring and docking it next to the Inspector (new behaviour). If the Inspector is not available,
        ///     the window is shown an as a free-floating window.
        /// 3. If there is more than one root tutorial container with different Project Layout setting in the project,
        ///    one asset is chosen randomly to specify the behavior.
        /// 4. If Tutorials window is already created, it is simply brought to the foreground and focused.
        /// </remarks>
        /// <returns>The the created, or already existing, window instance.</returns>
        public static TutorialWindow ShowWindow()
        {
            return ShowWindow(true);
        }

        /// <summary>
        /// Main logic for ShowWindow()
        /// </summary>>
        /// <param name="shouldRefreshLayout">Whether or not we should reset the layout to the basic tutorial layout.
        /// Should be false when loading a tutorial step and true when first initializing the tutorial window.</param>
        /// <returns>The the created, or already existing, window instance.</returns>
        public static TutorialWindow ShowWindow(bool shouldRefreshLayout)
        {
            var rootCategories = TutorialEditorUtils.FindAssets<TutorialContainer>()
                                                    .Where(category => category.ParentContainer is null);
            var defaultCategory = rootCategories.FirstOrDefault();
            var projectLayout = defaultCategory?.ProjectLayout;
            if (rootCategories.Any(category => category.ProjectLayout != projectLayout))
            {
                Debug.LogWarningFormat(
                    "There is more than one TutorialContainers asset with different Project Layout setting in the project. " +
                    "Using asset at path {0} for the window behavior settings.",
                    AssetDatabase.GetAssetPath(defaultCategory)
                );
            }

            TutorialWindow window = null;
            if (!rootCategories.Any() || defaultCategory.ProjectLayout == null)
            {
                window = GetOrCreateWindowNextToInspector();
            }
            else if (defaultCategory.ProjectLayout != null)
            {
                window = GetOrCreateWindowAndLoadLayout(defaultCategory, shouldRefreshLayout);
            }

            return window;
        }

        /// <summary>
        /// Creates the window if it does not exist, anchoring it as a tab next to the first found Inspector.
        /// If the window exists already, it's simply brought to the foreground and focused without any other actions.
        /// If any Inspector is not visible currently, Tutorials window is will be shown as a free-floating window.
        /// </summary>
        /// <remarks>
        /// This is the new and preferred way to show the Tutorials window.
        /// </remarks>
        /// <returns></returns>
        internal static TutorialWindow GetOrCreateWindowNextToInspector()
        {
            var inspectorWindow = Resources.FindObjectsOfTypeAll<EditorWindow>()
                                           .FirstOrDefault(w => w.GetType().Name == "InspectorWindow");

            Type windowToAnchorTo = inspectorWindow != null ? inspectorWindow.GetType() : null;
            Instance = GetOrCreateWindow(windowToAnchorTo); // create & anchor or simply focus
            // If Inspector not visible/opened, Tutorials window will be created as a free-floating window
            if (inspectorWindow)
            {
                inspectorWindow.DockWindow(Instance, EditorWindowUtils.DockPosition.Right);
            }
            return Instance;
        }

        /// <summary>
        /// Creates the window if it does not exist, and positions it using a window layout
        /// specified either by the TutorialContainer.ProjectLayout or Tutorial Framework's default layout.
        /// If the window exists already, it's simply brought to the foreground and focused without any other actions.
        /// If the project layout does not contain Tutorials window, it will be shown as a free-floating window.
        /// </summary>
        /// <remarks>
        /// This is the old way to show the Tutorials window and should be preferred only in situations where
        /// a special window layout is preferred when starting a tutorial project for the first time.
        /// </remarks>
        /// <param name="container">The container used for the project layout setting.</param>
        /// <returns></returns>
        internal static TutorialWindow GetOrCreateWindowAndLoadLayout(TutorialContainer container, bool shouldRefreshLayout)
        {
            if (Instance)
            {
                return GetOrCreateWindowNextToInspector();
            }

            if (container != null && shouldRefreshLayout)
            {
                container.LoadTutorialProjectLayout();
            }

            // If project layout did not contain tutorial window, it will be created as a free-floating window
            Instance = EditorWindowUtils.FindOpenInstance<TutorialWindow>();
            if (Instance == null)
            {
                GetOrCreateWindowNextToInspector(); // create
            }
            return Instance;
        }

        /// <summary>
        /// Creates a window and positions it as a tab of another window, if wanted.
        /// If the window exists already, it's brought to the foreground and focused.
        /// </summary>
        /// <param name="windowToAnchorTo"></param>
        /// <returns></returns>
        internal static TutorialWindow GetOrCreateWindow(Type windowToAnchorTo = null)
        {
            Instance = GetWindow<TutorialWindow>(Localization.Tr(LocalizationKeys.k_WindowTitle), windowToAnchorTo);
            Instance.minSize = k_MinWindowSize; // NOTE minSize has no effect on docked windows on 2021.2 and newer
            Instance.maxSize = k_MaxWindowSize;
            return Instance;
        }

        void OnEnable()
        {
            FrontendIsReadyToBeInitialized = false;
            IsWaitingForLocalizationToBeReady = true;
            SetupBackend();
            SetupFrontend();
            IsWaitingForLocalizationToBeReady = false;
        }

        void OnDisable()
        {
            if (TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog
            && !m_Model.Tutorial.IsLoadingLayout
            && !m_Model.Tutorial.PlayModeChanging)
            {
                // Delay call prevents us getting the dialog upon assembly reload.
                EditorApplication.delayCall += delegate
                {
                    TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog.SetValue(false);

                    string m_PromptOk = Localization.Tr(LocalizationKeys.k_WindowClosedDialogButtonOk);
                    string m_TabClosedDialogTitle = Localization.Tr(LocalizationKeys.k_WindowClosedDialogTitle);
                    string m_MenuPathGuide = Localization.Tr(MenuItems.Menu) + " > " + Localization.Tr(MenuItems.ShowTutorials);
                    string m_TabClosedDialogText = string.Format(Localization.Tr(LocalizationKeys.k_WindowClosedDialogMessage), m_MenuPathGuide);

                    EditorUtility.DisplayDialog(m_TabClosedDialogTitle, m_TabClosedDialogText, m_PromptOk);
                };
            }
            TeardownBackend();
        }

        void SetupBackend()
        {
            if (!Instance)
            {
                Instance = FindInstance();
            }

            TableOfContentView = new TableOfContentView();
            TutorialView = new TutorialView();
            RegisterView(TableOfContentView, SetupTableOfContentView);
            RegisterView(TutorialView, SetupTutorialView);

            m_Model = new TutorialFrameworkModel();
            m_TableOfContentModel = new TableOfContentModel();
            m_TutorialModel = new TutorialModel();
            m_Models = new HashSet<IModel> { m_Model, m_TableOfContentModel, m_TutorialModel };

            LoadModelsState();

            m_Controller = new TutorialFrameworkController(m_Model);
            var tableOfContentController = new TableOfContentController();
            var tutorialController = new TutorialController();
            m_Controllers = new HashSet<Controller> { m_Controller, tableOfContentController, tutorialController };

            SubscribeEvents();
            m_Model.IsOpen = true;
        }

        internal void RegisterView(View view, Action frontendSetupMethod)
        {
            if (m_Views == null)
            {
                m_Views = new HashSet<View>();
            }
            if (m_ViewFrontendSetupMethods == null)
            {
                m_ViewFrontendSetupMethods = new Dictionary<string, Action>();
            }
            m_Views.Add(view);
            m_ViewFrontendSetupMethods.Add(view.Name, frontendSetupMethod);
        }

        internal void UnregisterView(View view)
        {
            if (m_Views == null)
            {
                return;
            }
            m_Views.Remove(view);
            m_ViewFrontendSetupMethods.Remove(view.Name);
        }


        void SetupFrontend()
        {
            m_Root = rootVisualElement;

            titleContent.text = Localization.Tr(LocalizationKeys.k_WindowTitle);
            minSize = k_MinWindowSize;
            maxSize = k_MaxWindowSize;
            FrontendIsReadyToBeInitialized = true;
            RebuildFrontend();
        }

        void RebuildFrontend()
        {
            if (s_IsLoadingLayout && !s_RebuildFrontendEvenIfIsLoadingLayout)
            {
                if (!s_RebuildFrontendEvenIfIsLoadingLayout)
                {
                    return;
                }
                s_RebuildFrontendEvenIfIsLoadingLayout = false;
            }

            if (string.IsNullOrEmpty(CurrentView))
            {
                LoadView(TableOfContentView.k_Name);
            }
            else
            {
                LoadView(CurrentView);
            }
        }

        internal void LoadView(string viewName)
        {
            if (!CanSwitchToView(viewName)) { return; }

            CurrentView = viewName;
            m_Root.Clear();

            if (TutorialModel.s_AuthoringModeEnabled)
            {
                m_Root.Add(new IMGUIContainer(DrawAuthoringToolbar));
            }

            VisualTreeAsset windowContent = UIElementsUtils.LoadUXML(viewName);
            windowContent.CloneTree(m_Root);

            //preserve the base style, remove all styles defined in UXML and apply new skin
            for (int i = m_Root.styleSheets.count - 1; i > 0; i--)
            {
                m_Root.styleSheets.Remove(m_Root.styleSheets[i]);
            }

            UIElementsUtils.LoadCommonStyleSheet(m_Root);
            UpdateWindowSkin();

            m_ViewFrontendSetupMethods[viewName].Invoke();
        }

        void UpdateWindowSkin()
        {
            UIElementsUtils.RemoveStyleSheet(m_LastCommonStyleSheet, m_Root);
            UIElementsUtils.LoadSkinStyleSheet(out m_LastCommonStyleSheet, m_Root);
        }

        void SubscribeEvents()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        void UnsubscribeEvents()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
            if (m_Views != null)
            {
                foreach (var view in m_Views)
                {
                    view.UnubscribeEvents();
                }
            }
        }

        void OnDestroy() //aka: "When the user closes the window"
        {
            if (!m_Model.IsOpen)
            {
                if (!s_IsLoadingLayout)
                {
                    CurrentView = string.Empty; //we ensure that something is loaded when the window is reopened
                }
                SaveModelsState();
            }
            Instance = null;
        }

        void TeardownBackend()
        {
            m_Model.IsOpen = false;
            if (m_Models != null)
            {
                foreach (var model in m_Models)
                {
                    model.OnStop();
                }
            }

            if (!m_Model.DomainReloadOccured) //if it occurred we don't want to save the models as their "correct"state is already being managed in the Assembly reload-related callbacks
            {
                SaveModelsState();
            }

            if (m_Controllers != null)
            {
                foreach (var controller in m_Controllers)
                {
                    controller.RemoveListeners();
                }
            }
            UnsubscribeEvents();
        }

        /// <summary>
        /// Restore window state after assembly reload.
        /// </summary>
        void LoadModelsState()
        {
            foreach (var model in m_Models)
            {
                model.RestoreState(WindowCache.Instance);
            }

            foreach (var model in m_Models)
            {
                model.OnStart();
            }
        }

        /// <summary>
        /// Save state before assembly reload.
        /// </summary>
        internal void SaveModelsState()
        {
            foreach (var model in m_Models)
            {
                model.SaveState(WindowCache.Instance);
            }
            WindowCache.Instance.Serialize();
        }

        internal void OnBeforeAssemblyReload()
        {
            m_Model.DomainReloadOccured = true;
            SaveModelsState();
        }

        internal void OnAfterAssemblyReload()
        {
            LoadModelsState();
            if (Model.DomainReloadOccured)
            {
                Broadcast(new DomainReloadOccurredEvent());
            }
        }

        bool CanSwitchToView(string viewName)
        {
            if (m_Model.DomainReloadOccured
            && viewName != TutorialView.Name) //TutorialView triggers domain reload frequently and its initialization flow is strictly managed by the controller, so we don't want to accidentally re-reload it
            {
                m_Model.DomainReloadOccured = false;
                return true;
            }
            return (string.IsNullOrEmpty(CurrentView)
                && !string.IsNullOrEmpty(viewName))
                || viewName != CurrentView;
        }

        void SetupTableOfContentView() { TableOfContentView.Initialize(m_Root); }
        void SetupTutorialView() { TutorialView.Initialize(m_Root); }

        /// <summary>
        /// Notifies an event to the component's of the app
        /// </summary>
        /// <param name="evt"></param>
        internal void Broadcast(AppEvent evt)
        {
            EventManager.Broadcast(evt);
        }

        /// <summary>same as <see cref="Broadcast"/>, but static</summary>
        internal static void BroadcastEvent(AppEvent evt)
        {
            Instance?.Broadcast(evt);
        }

        internal IModel GetModel(Type modelType)
        {
            return m_Models.Where(m => m.GetType() == modelType).FirstOrDefault();
        }

        void DrawAuthoringToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));

            using (new EditorGUI.DisabledScope(Model.TableOfContent.CurrentCategory == null))
            {
                if (Button("VerticalLayoutGroup Icon", Localization.Tr(LocalizationKeys.k_AuthoringButtonSelectCategory)))
                {
                    Selection.activeObject = Model.TableOfContent.CurrentCategory;
                }
            }

            using (new EditorGUI.DisabledScope(Model.Tutorial.CurrentTutorial == null))
            {
                if (Button("HorizontalLayoutGroup Icon", Localization.Tr(LocalizationKeys.k_AuthoringButtonSelectTutorial)))
                {
                    Selection.activeObject = Model.Tutorial.CurrentTutorial;
                }

                using (new EditorGUI.DisabledScope(Model.Tutorial.CurrentTutorial?.CurrentPage == null))
                {
                    if (Button("UnityEditor.ConsoleWindow", Localization.Tr(LocalizationKeys.k_AuthoringButtonSelectPage)))
                    {
                        Selection.activeObject = Model.Tutorial.CurrentTutorial.CurrentPage;
                    }
                }

                if (Button("endButton", Localization.Tr(LocalizationKeys.k_AuthoringButtonSkipToEnd)))
                {
                    Model.Tutorial.CurrentTutorial.SkipToLastPage();
                    Model.Tutorial.CurrentTutorial.TryGoToNextPage(); // needed to trigger completion event
                }

                if (Button("Animation.NextKey", Localization.Tr(LocalizationKeys.k_AuthoringButtonAutocompletePage)))
                {
                    var paragraphsToComplete = Model.Tutorial.CurrentTutorial.CurrentPage.Paragraphs.Where(p => !p.Completed);
                    foreach (var instructiveParagraph in paragraphsToComplete)
                    {
                        foreach (var criterion in instructiveParagraph.Criteria)
                        {
                            criterion.Criterion.AutoComplete();
                            criterion.Criterion.UpdateCompletion();
                        }
                    }

                    if (!Model.Tutorial.CurrentTutorial.CurrentPage.AutoAdvanceOnComplete)
                    {
                        Model.Tutorial.CurrentTutorial.TryGoToNextPage();
                    }
                }
            }

            GUILayout.FlexibleSpace();

            using (new EditorGUI.DisabledScope(Model.Tutorial.CurrentTutorial == null))
            {
                EditorGUI.BeginChangeCheck();
                Model.Tutorial.MaskingEnabled = GUILayout.Toggle(
                    Model.Tutorial.MaskingEnabled, IconContent("Mask Icon", Localization.Tr(LocalizationKeys.k_IconPreviewMaskingTooltip)),
                    EditorStyles.toolbarButton, GUILayout.Width(k_AuthoringButtonWidth)
                );
                if (EditorGUI.EndChangeCheck())
                {
                    TutorialView.ApplyMaskingSettings(true);
                    GUIUtility.ExitGUI();
                    return;
                }
            }

            if (Button("Refresh", Localization.Tr(LocalizationKeys.k_ButtonRunStartupCode)))
            {
                UserStartupCode.RunStartupCode(TutorialProjectSettings.Instance);
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Stops and restars an editor coroutine
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="method"></param>
        internal void RestartEditorCoroutine(ref EditorCoroutine routine, IEnumerator method)
        {
            StopAndNullifyEditorCoroutine(ref routine);
            routine = EditorCoroutineUtility.StartCoroutine(method, this);
        }

        internal void StopAndNullifyEditorCoroutine(ref EditorCoroutine routine)
        {
            if (routine != null)
            {
                EditorCoroutineUtility.StopCoroutine(routine);
                routine = null;
            }
        }

        internal const float k_AuthoringButtonWidth = 30f;
        internal static bool Button(string iconName, string tooltip) =>
            GUILayout.Button(IconContent(iconName, tooltip), EditorStyles.toolbarButton, GUILayout.Width(k_AuthoringButtonWidth));

        internal static GUIContent IconContent(string iconName, string tooltip)
        {
            //you can find more suitable icons name in: https://github.com/halak/unity-editor-icons
            return EditorGUIUtility.IconContent(iconName, "|" + tooltip); // "|" needed for text to appear as tooltip
        }

        /// <summary>
        /// Marks all known tutorials as uncompleted
        /// </summary>
        internal void MarkAllTutorialsUncompleted()
        {
            var allTutorials = TutorialEditorUtils.FindAssets<Tutorial>()
                                                  .Where(t => t.ProgressTrackingEnabled);

            foreach (var tutorial in allTutorials)
            {
                tutorial.CompletedByUser = false;
            }

            Broadcast(new TutorialsCompletionStatusUpdatedEvent());
        }

        /// <summary>
        /// Starts a tutorial as if it was clicked in the Table of content.
        /// </summary>
        /// <param name="tutorial">The tutorial to start</param>
        public static void StartTutorial(Tutorial tutorial)
        {
            if (!Instance)
            {
                GetOrCreateWindowNextToInspector();
                EditorCoroutineUtility.StartCoroutineOwnerless(StartTutorialWhenFrontendIsReady(tutorial));
                return;
            }
            Instance.Broadcast(new TutorialStartRequestedEvent(tutorial, null));
        }

        static IEnumerator StartTutorialWhenFrontendIsReady(Tutorial tutorial)
        {
            while (!Instance.Model.IsOpen || !Instance.FrontendIsReadyToBeInitialized)
            {
                yield return null;
            }
            Instance.Broadcast(new TutorialStartRequestedEvent(tutorial, null));
        }

        /// <summary>
        /// Exits the current tutorial
        /// </summary>
        public static void ExitTutorial()
        {
            if (!Instance)
            {
                return;
            }
            Instance.Broadcast(new TutorialQuitEvent());
        }

        /// <summary>
        /// Clear localization table cache
        /// </summary>
        public static void ClearLocalizationCache()
        {
            LocalizationDatabaseProxy.ClearLocalizationCache();
        }

        /// <summary>
        /// Re-find our instance if we've lost it
        /// This occurs when a window is maximized
        /// </summary>
        protected override void OnResized_Internal()
        {
            if (!Instance)
            {
                Instance = FindInstance();
            }
        }
    }
}
