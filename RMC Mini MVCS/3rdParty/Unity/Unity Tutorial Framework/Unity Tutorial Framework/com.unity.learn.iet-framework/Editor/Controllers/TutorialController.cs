using System.Collections;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Unity.Tutorials.Core.Editor
{
    internal class TutorialController : Controller
    {
        TutorialModel m_Model => Application?.Model.Tutorial;
        TutorialView m_View => Application?.TutorialView;
        Tutorial CurrentTutorial => m_Model?.CurrentTutorial;
        EditorCoroutine m_AutoAdvanceRoutine;
        EditorCoroutine m_OnEditorUpdateRoutine;

        internal TutorialController()
        {
            AddListener<TutorialStartRequestedEvent>(OnTutorialStartRequested);
            AddListener<TutorialQuitEvent>(OnTutorialQuitRequested);
            AddListener<TutorialNavigationEvent>(OnTutorialNavigationRequested);
            AddListener<DomainReloadOccurredEvent>(OnDomainReloadOccurred);
        }

        internal override void RemoveListeners()
        {
            RemoveListener<TutorialStartRequestedEvent>(OnTutorialStartRequested);
            RemoveListener<TutorialQuitEvent>(OnTutorialQuitRequested);
            RemoveListener<TutorialNavigationEvent>(OnTutorialNavigationRequested);
            RemoveListener<DomainReloadOccurredEvent>(OnDomainReloadOccurred);
            if (CurrentTutorial)
            {
                ClearTutorialListeners(CurrentTutorial);
            }
        }

        void OnDomainReloadOccurred(DomainReloadOccurredEvent evt)
        {
            if (!CurrentTutorial) { return; }
            EditorCoroutineUtility.StartCoroutine(WaitUntilViewCanBeInitialized(), Application);
        }

        IEnumerator WaitUntilViewCanBeInitialized()
        {
            while (!Application.FrontendIsReadyToBeInitialized)
            {
                yield return null;
            }
            ResumeTutorial();
        }

        bool UserWantsToSaveOrDiscardChangesOfUnsavedScene()
        {
            return EditorApplication.isPlaying //scenes can't be saved in play mode, so we don't really have an option in that case
                || EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        void OnTutorialStartRequested(TutorialStartRequestedEvent evt)
        {
            if (CurrentTutorial == evt.Tutorial) { return; }

            /* Exit early-out if user decides to cancel. Otherwise the user can get reset to the
             main tutorial selection screen in cases where the user was about to switch to
             another tutorial while finishing up another (typical use case would be having a
            "start next tutorial" button at the last page of a tutorial). */

            if (!UserWantsToSaveOrDiscardChangesOfUnsavedScene())
            {
                return;
            }

            m_Model.IsTransitioningBetweenTutorials = evt.PreviousTutorial;

            if (CurrentTutorial)
            {
                if (m_Model.IsTransitioningBetweenTutorials)
                {
                    OnTutorialCompleted(CurrentTutorial);
                }
                DeInitializeTutorial(CurrentTutorial);
            }

            if (!m_Model.IsTransitioningBetweenTutorials)
            {
                m_Model.SaveOriginalScenes();
                m_Model.SaveOriginalWindowLayout();
                m_Model.SaveSceneViewState();
            }

            m_Model.CurrentTutorial = evt.Tutorial;

            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                EditorApplication.playModeStateChanged += StartTutorialWhenEnteringEditMode;
            }
            else
            {
                StartTutorial();
            }
        }

        void OnTutorialQuitRequested(TutorialQuitEvent evt)
        {
            if (CurrentTutorial == null
            || !UserWantsToSaveOrDiscardChangesOfUnsavedScene())
            {
                return;
            }

            if (EditorApplication.isPlaying)
            {
                EditorCoroutineUtility.StartCoroutine(ExitTutorialAndPlayMode(), Application);
                return;
            }
            ExitTutorial();

        }

        IEnumerator ExitTutorialAndPlayMode()
        {
            EditorApplication.isPlaying = false;
            while (EditorApplication.isPlaying)
            {
                yield return null;
            }
            ExitTutorial();
        }

        void ExitTutorial()
        {
            m_Model.IsTransitioningBetweenTutorials = false;
            CurrentTutorial.RaiseQuit();
        }

        void OnTutorialNavigationRequested(TutorialNavigationEvent evt)
        {
            if (evt.MoveToNextPage)
            {
                LoadNextTutorialPage();
                return;
            }
            LoadPreviousTutorialPage();
        }

        void StartTutorialWhenEnteringEditMode(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
            {
                EditorApplication.playModeStateChanged -= StartTutorialWhenEnteringEditMode;
                StartTutorial();
            }
        }

        void StartTutorial()
        {
            m_Model.SkipNextAutoAdvancing = false;
            if (m_Model.CurrentTutorial.WindowLayout)
            {
                ClearTutorialListeners(CurrentTutorial); //note: we clean them up before reloading the layout, to be sure that there are no shadowing problems between windows instances
                PrepareWindowLayouts();
                TutorialModel.OnLayoutLoaded -= OnTutorialLayoutLoaded;
                TutorialModel.OnLayoutLoaded += OnTutorialLayoutLoaded;
                TutorialModel.LoadWindowLayout(m_Model.CurrentTutorial.WindowLayoutPath);
            }
            else
            {
                AddTutorialListeners(CurrentTutorial);
                CurrentTutorial.ResetProgress();
                CurrentTutorial.Initiate();
            }
        }

        void OnTutorialLayoutLoaded(bool obj)
        {
            TutorialModel.OnLayoutLoaded -= OnTutorialLayoutLoaded;
            var tutorialWindow = TutorialWindow.ShowWindow(false);
            AddTutorialListeners(CurrentTutorial);
            CurrentTutorial.ResetProgress();
            CurrentTutorial.Initiate();
        }

        void ResumeTutorial()
        {
            AddTutorialListeners(CurrentTutorial);
            CurrentTutorial.RaisePageInitiated(CurrentTutorial.CurrentPage, CurrentTutorial.CurrentPageIndex);
        }

        void AddTutorialListeners(Tutorial tutorial)
        {
            ClearTutorialListeners(tutorial);

            tutorial.Initiated.AddListener(OnTutorialInitiated);
            tutorial.Completed.AddListener(OnTutorialCompleted);
            tutorial.Quit.AddListener(OnTutorialQuit);
            tutorial.PageInitiated.AddListener(OnPageInitiated);
            tutorial.Modified.AddListener(OnCurrentTutorialModified);
        }

        void ClearTutorialListeners(Tutorial tutorial)
        {
            tutorial.Initiated.RemoveAllListeners();
            tutorial.Completed.RemoveAllListeners();
            tutorial.Quit.RemoveAllListeners();
            tutorial.PageInitiated.RemoveAllListeners();
            tutorial.Modified.RemoveAllListeners();

            foreach (var page in tutorial.PagesCollection)
            {
                page.MaskingSettingsChanged.RemoveAllListeners();
                page.NonMaskingSettingsChanged.RemoveAllListeners();
            }
        }

        void OnTutorialInitiated(Tutorial tutorial)
        {
            AnalyticsHelper.TutorialStarted(tutorial);
            if (tutorial.ProgressTrackingEnabled)
            {
                GenesisHelper.LogTutorialStarted(tutorial.LessonId);
            }
        }

        void OnTutorialCompleted(Tutorial tutorial)
        {
            /* After the tutorial is completed once, there's no longer need to report its possible repeated completions,
            for example going back and forth between the second-to-last and last page. */
            tutorial.Completed.RemoveListener(OnTutorialCompleted);

            AnalyticsHelper.TutorialEnded(TutorialConclusion.Completed);
            if (tutorial.ProgressTrackingEnabled)
            {
                GenesisHelper.LogTutorialEnded(tutorial.LessonId);
                tutorial.CompletedByUser = true;
            }
        }

        void OnTutorialQuit(Tutorial tutorial)
        {
            AnalyticsHelper.TutorialEnded(TutorialConclusion.Quit);
            DeInitializeTutorial(CurrentTutorial);
            m_View.UnubscribeEvents();

            if (tutorial.WindowLayout)
            {
                TutorialModel.OnLayoutLoaded -= OnPreviousLayoutLoaded;
                TutorialModel.OnLayoutLoaded += OnPreviousLayoutLoaded;
            }
            else
            {
                Application.LoadView(Application.TableOfContentView.Name);
            }

            if (!m_Model.IsTransitioningBetweenTutorials)
            {
                EditorCoroutineUtility.StartCoroutine(m_Model.ReopenActiveScenesAsBeforeTutorialStarted(), Application);
                TutorialModel.ReopenWindowLayoutAsBeforeTutorialStarted(tutorial);
                m_Model.RestoreSceneViewStateAsBeforeTutorialStarted();

                if (tutorial.ShowCompletionDialogOnQuit)
                {
                    tutorial.ShowCompletionDialogOnQuit = false;
                    if (tutorial.CompletionDialog)
                    {
                        TutorialModalWindow.Show(tutorial.CompletionDialog);
                    }
                }
            }
        }

        void OnPreviousLayoutLoaded(bool obj)
        {
            TutorialModel.OnLayoutLoaded -= OnPreviousLayoutLoaded;
            Application.LoadView(Application.TableOfContentView.Name);
        }

        void DeInitializeTutorial(Tutorial tutorial)
        {
            EditorApplication.update -= OnEditorUpdate;
            if (CurrentTutorial)
            {
                CurrentTutorial.CurrentPage.MaskingSettingsChanged.RemoveListener(OnTutorialPageMaskingSettingsChanged);
                CurrentTutorial.CurrentPage.NonMaskingSettingsChanged.RemoveListener(OnTutorialPageNonMaskingSettingsChanged);
            }

            m_View.ApplyMaskingSettings(false);
            tutorial.StopTutorial();
            ClearTutorialListeners(tutorial);
            m_Model.CurrentTutorial = null;
        }

        /// <summary>
        /// Shows page's content after the page is initiated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="page"></param>
        /// <param name="index"></param>
        void OnPageInitiated(Tutorial sender, TutorialPage page, int index)
        {
            page.RaiseShowing();
            AnalyticsHelper.PageShown(page, index);
            if (Application.CurrentView == m_View.Name)
            {
                m_View.Refresh();
            }
            else
            {
                Application.LoadView(m_View.Name);
            }
            page.RaiseShown();
            page.SetupCompletionCriteria
            (
                (Criterion c, TutorialParagraph p) => OnCriterionCompleted(c, p, page),
                (Criterion c, TutorialParagraph p) => OnCriterionInvalidated(c, p, page)
            );

            page.MaskingSettingsChanged.AddListener(OnTutorialPageMaskingSettingsChanged);
            page.NonMaskingSettingsChanged.AddListener(OnTutorialPageNonMaskingSettingsChanged);

            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
        }

        void OnEditorUpdate()
        {
            if (CurrentTutorial)
            {
                CurrentTutorial.CurrentPage.RaiseStaying(); //todo: maybe it'd be good to add an option to throttle this, as performing checks every frame could be heavy
                MaskingManager.OnEditorUpdate();
            }
        }

        void OnCriterionCompleted(Criterion sender, TutorialParagraph paragraph, TutorialPage page)
        {
            m_View.UpdateInstructionBoxForParagraph(paragraph);
            m_View.UpdateStateOfFooterButtons();
            m_View.ApplyMaskingSettings(true);
            if (!page.AreAllCriteriaSatisfied)
            {
                return;
            }
            page.PlayCompletionSound();
            if (page.AutoAdvanceOnComplete)
            {
                if (!m_Model.SkipNextAutoAdvancing)
                {
                    Application.RestartEditorCoroutine(ref m_AutoAdvanceRoutine, LoadNextTutorialPageAfterDelay());
                }
            }
        }

        void OnCriterionInvalidated(Criterion sender, TutorialParagraph paragraph, TutorialPage page)
        {
            m_View.UpdateInstructionBoxForParagraph(paragraph);
            m_View.UpdateStateOfFooterButtons();
            m_View.ApplyMaskingSettings(true);
            if (!page.AutoAdvanceOnComplete)
            {
                return;
            }

            Application.StopAndNullifyEditorCoroutine(ref m_AutoAdvanceRoutine);
        }

        IEnumerator LoadNextTutorialPageAfterDelay()
        {
            yield return TutorialModel.s_AutoAdvanceDelay;
            LoadNextTutorialPage();
        }

        void LoadNextTutorialPage()
        {
            if (CurrentTutorial == null) { return; }
            var currentPage = CurrentTutorial.CurrentPage;
            currentPage.MaskingSettingsChanged.RemoveListener(OnTutorialPageMaskingSettingsChanged);
            currentPage.NonMaskingSettingsChanged.RemoveListener(OnTutorialPageNonMaskingSettingsChanged);
            m_Model.SkipNextAutoAdvancing = false;
            if (!CurrentTutorial.TryGoToNextPage())
            {
                if (CurrentTutorial.CurrentPageIsLast())
                {
                    CurrentTutorial.ShowCompletionDialogOnQuit = true;
                    Application.Broadcast(new TutorialQuitEvent());
                }
            }
        }

        void LoadPreviousTutorialPage()
        {
            if (CurrentTutorial == null) { return; }

            var currentPage = CurrentTutorial.CurrentPage;
            currentPage.MaskingSettingsChanged.RemoveListener(OnTutorialPageMaskingSettingsChanged);
            currentPage.NonMaskingSettingsChanged.RemoveListener(OnTutorialPageNonMaskingSettingsChanged);

            if (CurrentTutorial.CurrentPageIndex == 0)
            {
                Application.Broadcast(new TutorialQuitEvent());
            }
            else
            {
                m_Model.SkipNextAutoAdvancing = true;
                CurrentTutorial.GoToPreviousPage();
            }
        }

        void OnCurrentTutorialModified(Tutorial sender)
        {
            m_View.Refresh();
        }

        void OnTutorialPageMaskingSettingsChanged(TutorialPage sender)
        {
            m_View.RefreshMasking();
        }

        void OnTutorialPageNonMaskingSettingsChanged(TutorialPage sender)
        {
            m_View.Refresh();
        }

        /// <summary>
        /// Replaces LastProjectPaths in window layouts used in tutorials so that e.g.
        /// pre-saved Project window states work correctly.
        /// </summary>
        internal static void PrepareWindowLayouts()
        {
            var layoutPathsOfCategories = AssetDatabase.FindAssets($"t:{typeof(TutorialContainer).FullName}")
                                                       .Select(guid => AssetDatabase.LoadAssetAtPath<TutorialContainer>(AssetDatabase.GUIDToAssetPath(guid)).ProjectLayoutPath);

            var layoutPathsOfTutorials = AssetDatabase.FindAssets($"t:{typeof(Tutorial).FullName}")
                                                      .Select(guid => AssetDatabase.LoadAssetAtPath<Tutorial>(AssetDatabase.GUIDToAssetPath(guid)).WindowLayoutPath);

            layoutPathsOfCategories.Concat(layoutPathsOfTutorials)
                                   .Where(StringExt.IsNotNullOrEmpty)
                                   .Distinct()
                                   .ToList()
                                   .ForEach(layoutPath => TutorialModel.PrepareWindowLayout(layoutPath));
        }
    }
}
