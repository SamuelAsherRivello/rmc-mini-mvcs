using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace Unity.Tutorials.Core.Editor
{
    internal class TutorialView : View
    {
        const string k_NextButtonBorderElementName = "NextButtonBase";
        internal const string k_Name = "Tutorial";
        internal override string Name => k_Name;
        TutorialModel Model => Application?.Model?.Tutorial;
        TutorialPage CurrentPage => Model?.CurrentTutorial?.CurrentPage;
        string PageTitle => CurrentPage.Title;

        VisualElement m_Root;
        VisualElement tutorialPageContainer;
        VisualElement tutorialParagraphsContainer;
        VisualElement footer;
        ScrollView tutorialScrollview;
        Button btnNext;
        EditorCoroutine m_NextButtonBlinkRoutine;

        Dictionary<ParagraphType, VisualTreeAsset> paragraphsRepresentationsPrefabs = new Dictionary<ParagraphType, VisualTreeAsset>();
        Dictionary<TutorialParagraph, VisualElement> instructionParagraphs = new Dictionary<TutorialParagraph, VisualElement>();

        VideoPlaybackManager VideoPlaybackManager { get; } = new VideoPlaybackManager();
        public TutorialView() : base() { }

        public override void SubscribeEvents()
        {
            base.SubscribeEvents();
            GUIViewProxy.PositionChanged += OnGUIViewPositionChanged;
            EditorApplication.projectChanged += RefreshMasking;
            EditorApplication.hierarchyChanged += RefreshMaskingOnHierarchyChange;
        }

        public override void UnubscribeEvents()
        {
            base.UnubscribeEvents();
            GUIViewProxy.PositionChanged -= OnGUIViewPositionChanged;
            EditorApplication.projectChanged -= RefreshMasking;
            EditorApplication.hierarchyChanged -= RefreshMaskingOnHierarchyChange;

            Application.StopAndNullifyEditorCoroutine(ref m_NextButtonBlinkRoutine);
            VideoPlaybackManager.OnDisable();
            ApplyMaskingSettings(false);
        }

        internal void Initialize(VisualElement root)
        {
            m_Root = root;

            tutorialPageContainer = m_Root.Q("TutorialPageContainer");
            footer = m_Root.Q("TutorialActions");
            tutorialScrollview = (ScrollView)tutorialPageContainer.Q("TutorialContainer").ElementAt(0);
            tutorialParagraphsContainer = tutorialScrollview.Q("unity-content-container");

            if (paragraphsRepresentationsPrefabs.Count == 0)
            {
                foreach (ParagraphType paragraphType in Enum.GetValues(typeof(ParagraphType)))
                {
                    paragraphsRepresentationsPrefabs.Add(paragraphType, UIElementsUtils.LoadUXML($"Paragraphs/{paragraphType}"));
                }
            }

            VideoPlaybackManager.OnEnable();
            VideoPlaybackManager.ClearCache();

            Refresh();
            SubscribeEvents();
        }

        private void RefreshMaskingOnHierarchyChange()
        {
            if (!CurrentPage.ShouldRefreshMaskingOnHierarchyChange) { return; }
            RefreshMasking();
        }

        internal void RefreshMasking()
        {
            if (!Application.Model.IsOpen) { return; }
            if (Model.CurrentTutorial == null) { return; }
            MaskingManager.Unmask();
            ApplyMaskingSettings(true);
            QueueMaskUpdate();
        }

        void OnGUIViewPositionChanged(UnityEngine.Object sender)
        {
            if (Model.CurrentTutorial == null
            || Model.IsLoadingLayout
            || sender.GetType() == GUIViewProxy.TooltipViewType)
            {
                return;
            }
            ApplyMaskingSettings(true);
        }

        internal void Refresh()
        {
            UIElementsUtils.SetupLabel("lblTutorialName", Model.CurrentTutorial.TutorialTitle, m_Root, false);
            UIElementsUtils.SetupLabel("lblStepCount", $"{Model.CurrentTutorial.CurrentPageIndex + 1} / {Model.CurrentTutorial.PagesCollection.Count}", m_Root, false);

            m_Root.Q("btnQuit").RegisterCallback<MouseUpEvent>(_ => ExitTutorial());

            UIElementsUtils.SetupButton("btnPrevious", OnPreviousButtonClicked, true, footer, LocalizationKeys.k_TutorialButtonPrevious, localize: true);

            string nextButtonText = Model.CurrentTutorial.CurrentPageIndex + 1 == Model.CurrentTutorial.PagesCollection.Count ? CurrentPage.DoneButton
                                                                                                                      : CurrentPage.NextButton;
            btnNext = UIElementsUtils.SetupButton("btnNext", OnNextButtonClicked, Model.CanMoveToNextPage, footer, nextButtonText, localize: true);

            ShowCurrentTutorialContent();
            ApplyMaskingSettings(true);
        }

        void QueueMaskUpdate()
        {
            EditorApplication.update -= ApplyQueuedMask;
            EditorApplication.update += ApplyQueuedMask;
        }

        void ApplyQueuedMask()
        {
            if (!Application || Application.IsParentNull())
            {
                return;
            }

            EditorApplication.update -= ApplyQueuedMask;
            ApplyMaskingSettings(true);
        }

        internal void ApplyMaskingSettings(bool applyMask)
        {
            if (!applyMask || !Model.MaskingEnabled || Model.IsLoadingLayout || !Model.CurrentTutorial)
            {
                MaskingManager.Unmask();
                return;
            }

            MaskingSettings maskingSettings = Model.CurrentTutorial.CurrentPage.CurrentMaskingSettings;
            try
            {
                if (maskingSettings == null || !maskingSettings.Enabled)
                {
                    MaskingManager.Unmask();
                }
                else
                {
                    bool foundAncestorProperty;
                    var unmaskedViews = UnmaskedView.GetViewsAndRects(maskingSettings.UnmaskedViews, out foundAncestorProperty);
                    if (foundAncestorProperty)
                    {
                        // Keep updating mask when target property is not unfolded
                        QueueMaskUpdate();
                    }

                    /*
                    // When going back, don't reapply the page's masking settings if the page has completion criteria:
                    // criterion logic introduces complexity and potential changes in the UI, but if we don't have such,
                    // it's fairly safe to re-unmask/highlight UI elements.
                    if (currentTutorial.CurrentPageIndex <= m_FarthestPageCompleted && currentTutorial.CurrentPage.HasCriteria())
                    {
                        unmaskedViews = new UnmaskedView.MaskData();
                    }*/

                    UnmaskedView.MaskData highlightedViews;

                    if (unmaskedViews.Count > 0) //Unmasked views should be highlighted
                    {
                        highlightedViews = (UnmaskedView.MaskData)unmaskedViews.Clone();
                    }
                    else if (Model.CanMoveToNextPage) // otherwise, if the current page is completed, highlight this window
                    {
                        highlightedViews = new UnmaskedView.MaskData();
                        highlightedViews.AddParentFullyUnmasked(Application);
                    }
                    else // otherwise, highlight manually specified control rects if there are any
                    {
                        var unmaskedControls = new List<GuiControlSelector>();
                        var unmaskedViewsWithControlsSpecified =
                            maskingSettings.UnmaskedViews.Where(v => v.GetUnmaskedControls(unmaskedControls) > 0).ToArray();
                        // if there are no manually specified control rects, highlight all unmasked views
                        highlightedViews = UnmaskedView.GetViewsAndRects(
                            unmaskedViewsWithControlsSpecified.Length == 0 ?
                            maskingSettings.UnmaskedViews : unmaskedViewsWithControlsSpecified
                        );
                    }

                    // ensure tutorial window's HostView and tooltips are not masked
                    unmaskedViews.AddParentFullyUnmasked(Application);
                    unmaskedViews.AddTooltipViews();

                    // tooltip views should not be highlighted
                    highlightedViews.RemoveTooltipViews();

                    /* note: For some reason, when highlighting is applied because HierarchyChanged is triggered because a new object is created
                     * and the editor is not attached to a debugger, the object won't be highlighted even if its name matches
                     any of the masking settings. If the editor is attached to a debugger, the highlighting will work as expected. */
                    MaskingManager.Mask(
                        unmaskedViews,
                        Model.Styles == null ? Color.magenta * new Color(1f, 1f, 1f, 0.8f) : Model.Styles.MaskingColor,
                        highlightedViews,
                        Model.Styles == null ? Color.cyan * new Color(1f, 1f, 1f, 0.8f) : Model.Styles.HighlightColor,
                        Model.Styles == null ? new Color(1, 1, 1, 0.5f) : Model.Styles.BlockedInteractionColor,
                        Model.Styles == null ? 3f : Model.Styles.HighlightThickness
                    );
                }
            }
            catch (ArgumentException e)
            {
                if (TutorialModel.s_AuthoringModeEnabled)
                {
                    Debug.LogException(e, Model.CurrentTutorial.CurrentPage);
                }
                else
                {
                    Console.WriteLine(StackTraceUtility.ExtractStringFromException(e));
                }
                MaskingManager.Unmask();
            }
        }

        void ExitTutorial()
        {
            Application.Broadcast(new TutorialQuitEvent());
        }

        void ScrollToTop()
        {
            tutorialScrollview.scrollOffset = Vector2.zero;
        }

        void SetupParagraphUI(VisualElement paragraphUI, TutorialParagraph paragraph, string pageName)
        {
            const string instructionContainerElementName = "InstructionContainer";
            const string startLinkedTutorialElementName = "btnStartLinkedTutorial";
            const string tutorialMediaContainerElementName = "TutorialMediaContainer";

            paragraphUI.name = paragraph.Type.ToString();
            switch (paragraph.Type)
            {
                case ParagraphType.Narrative:
                    RichTextParser.RichTextToVisualElements(paragraph.Text, paragraphUI.Q("TutorialStepBox1"));
                    break;
                case ParagraphType.Instruction:
                    if (string.IsNullOrEmpty(paragraph.Text) && string.IsNullOrEmpty(paragraph.Title))
                    {
                        UIElementsUtils.Hide(instructionContainerElementName, paragraphUI);
                    }
                    else
                    {
                        UIElementsUtils.Show(instructionContainerElementName, paragraphUI);
                        if (string.IsNullOrEmpty(paragraph.Title))
                        {
                            UIElementsUtils.Hide("InstructionTitle", paragraphUI);
                        }
                        else
                        {
                            UIElementsUtils.Show("InstructionTitle", paragraphUI);
                        }
                        UIElementsUtils.SetupLabel("InstructionTitle", paragraph.Title, paragraphUI, false);
                        RichTextParser.RichTextToVisualElements(paragraph.Text, paragraphUI.Q("InstructionDescription"));
                        instructionParagraphs.Add(paragraph, paragraphUI);
                        UpdateInstructionBoxForParagraph(paragraph);
                    }
                    break;
                case ParagraphType.SwitchTutorial:
                    if (paragraph.m_Tutorial == null)
                    {
                        UIElementsUtils.Hide(startLinkedTutorialElementName, paragraphUI);
                        Debug.LogError($"Target tutorial of paragraph 'Switch Tutorial' of the page '{pageName}' is null. Did you forget to specify to which tutorial the user should transition to?");
                    }
                    else if (paragraph.m_Tutorial == Model.CurrentTutorial)
                    {
                        UIElementsUtils.Hide(startLinkedTutorialElementName, paragraphUI);
                        Debug.LogError($"Target tutorial of paragraph 'Switch Tutorial' of the page '{pageName}' is the current tutorial, but it needs to be a different one.");
                    }
                    else
                    {
                        UIElementsUtils.SetupButton(startLinkedTutorialElementName, () => SwitchTutorial(paragraph.m_Tutorial), true, paragraphUI, paragraph.Text, localize: true);
                    }
                    break;
                case ParagraphType.Image:
                    if (paragraph.Image != null)
                    {
                        UIElementsUtils.Show(tutorialMediaContainerElementName, paragraphUI);
                        paragraphUI.Q("TutorialMedia").style.backgroundImage = paragraph.Image;
                    }
                    else
                    {
                        UIElementsUtils.Hide(tutorialMediaContainerElementName, paragraphUI);
                    }
                    break;
                case ParagraphType.Video:
                    if (paragraph.Video != null)
                    {
                        UIElementsUtils.Show(tutorialMediaContainerElementName, paragraphUI);
                        EditorCoroutineUtility.StartCoroutine(UpdateVideo(paragraph.Video, paragraphUI.Q("videoPlayer")), Application);
                    }
                    else
                    {
                        UIElementsUtils.Hide(tutorialMediaContainerElementName, paragraphUI);
                    }
                    break;
                default: break;
            }
        }

        void SwitchTutorial(Tutorial newTutorial)
        {
            Application.Broadcast(new TutorialStartRequestedEvent(newTutorial, Model.CurrentTutorial));
        }

        IEnumerator UpdateVideo(VideoClip video, VisualElement paragraphUI)
        {
            while (Application && Model.CurrentTutorial)
            {
                paragraphUI.style.backgroundImage = VideoPlaybackManager.GetTextureForVideoClip(video);
                paragraphUI.MarkDirtyRepaint(); //needed, otherwise the video will be laggy as images will be updated only on the next editor update
                yield return null;
            }
        }

        void ShowCurrentTutorialContent()
        {
            instructionParagraphs.Clear();
            var lblPageTitle = UIElementsUtils.SetupLabel("lblPageTitle", PageTitle, tutorialScrollview, false);
            for (int i = tutorialParagraphsContainer.childCount - 1; i >= 0; i--) //remove all containers
            {
                if (tutorialParagraphsContainer.ElementAt(i) != lblPageTitle)
                {
                    tutorialParagraphsContainer.Remove(tutorialParagraphsContainer.ElementAt(i));
                }
            }

            ScrollToTop();
            string currentPageName = Model.CurrentTutorial.CurrentPage.name;
            foreach (TutorialParagraph paragraph in Model.CurrentTutorial.CurrentPage.Paragraphs)
            {
                VisualElement paragraphUI = paragraphsRepresentationsPrefabs[paragraph.Type].CloneTree();
                SetupParagraphUI(paragraphUI, paragraph, currentPageName);
                tutorialParagraphsContainer.Add(paragraphUI);
            }

            if (Model.CurrentTutorial.CurrentPageIsFirst())
            {
                Application.RestartEditorCoroutine(ref m_NextButtonBlinkRoutine, MakeNextButtonBlink());
            }
            else
            {
                Application.StopAndNullifyEditorCoroutine(ref m_NextButtonBlinkRoutine);
                UIElementsUtils.ShowOrHide(k_NextButtonBorderElementName, btnNext, false);
            }
        }

        IEnumerator MakeNextButtonBlink()
        {
            bool highlightBorder = true;
            while (Model.CurrentTutorial)
            {
                UIElementsUtils.ShowOrHide(k_NextButtonBorderElementName, btnNext, highlightBorder && Model.CanMoveToNextPage);
                highlightBorder = !highlightBorder;
                yield return new EditorWaitForSeconds(1);
            }
        }

        void OnPreviousButtonClicked()
        {
            Application.Broadcast(new TutorialNavigationEvent(false));
        }

        void OnNextButtonClicked()
        {
            Application.Broadcast(new TutorialNavigationEvent(true));
        }

        /// <summary>
        /// Sets the instruction highlight to green or blue and toggles between arrow and checkmark
        /// </summary>
        internal void UpdateInstructionBoxForParagraph(TutorialParagraph paragraph)
        {
            if (instructionParagraphs.ContainsKey(paragraph)) //we double check this as some pages might not have a visual instructive paragraph, depending on the author's preferences
            {
                UpdateInstructionBox(instructionParagraphs[paragraph], paragraph.Completed);
            }
        }

        void UpdateInstructionBox(VisualElement paragraphUI, bool allParagraphCriteriaCompleted)
        {
            UIElementsUtils.ShowOrHide("green", paragraphUI, allParagraphCriteriaCompleted);
            UIElementsUtils.ShowOrHide("imgCheckmark", paragraphUI, allParagraphCriteriaCompleted);
            UIElementsUtils.ShowOrHide("blue", paragraphUI, !allParagraphCriteriaCompleted);
            UIElementsUtils.ShowOrHide("imgArrow", paragraphUI, !allParagraphCriteriaCompleted);
        }

        internal void UpdateStateOfFooterButtons()
        {
            btnNext.SetEnabled(Model.CanMoveToNextPage);
        }
    }
}
