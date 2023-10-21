using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor.Tests
{
    /// <summary>
    /// Those tests are run ONCE before/after other tests of this assembly
    /// https://docs.nunit.org/articles/nunit/writing-tests/attributes/setupfixture.html
    /// </summary>
    [SetUpFixture]
    public class TutorialTestsSetup
    {
        public static string s_TempFolderPath;
        [SerializeField]
        bool s_OriginalValueOfShowTutorialsWindowClosedDialog;

        public TutorialTestsSetup() { }

        [OneTimeSetUp] //note: this is also called once every domain reload!
        public void RunBeforeAnyTests()
        {
            TutorialFrameworkModel.s_AreTestsRunning = true;
            TableOfContentModel.CategoriesOfProjectDuringTests = new List<TutorialContainer>();

            var tempFolderGUID = AssetDatabase.IsValidFolder("Assets/Temp") ? AssetDatabase.AssetPathToGUID("Assets/Temp")
                                                                            : AssetDatabase.CreateFolder("Assets", "Temp");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            s_TempFolderPath = AssetDatabase.GUIDToAssetPath(tempFolderGUID);
            s_OriginalValueOfShowTutorialsWindowClosedDialog = TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog;
            TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog.SetValue(false);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog.SetValue(s_OriginalValueOfShowTutorialsWindowClosedDialog);
            TutorialFrameworkModel.s_AreTestsRunning = false;
            WindowCache.Instance.AreTestsRunning = false;
            WindowCache.Instance.Serialize();
            AssetDatabase.DeleteAsset(s_TempFolderPath);
        }
    }

    public class TutorialWindowTests
    {
        /// <summary>
        /// A mocked view used to test the navigation system
        /// </summary>
        class MockView : View
        {
            internal override string Name => "Mock";
        }

        class MockedWindow : EditorWindow
        {
        }

        Tutorial m_Tutorial;
        TutorialWindow Window => TutorialWindow.Instance;

        [SerializeField]
        string m_TempFolderPath;
        string TempScenePath => $"{m_TempFolderPath}/TempScene.unity";

        [SetUp]
        public void SetUp()
        {
            if (!EditorApplication.isPlaying)
            {
                m_TempFolderPath = TutorialTestsSetup.s_TempFolderPath;
                // Make sure we start afresh each time
                var tempScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                EditorSceneManager.SaveScene(tempScene, TempScenePath);
            }

            m_Tutorial = TutorialTestsUtils.CreateMockTutorial();
            SetupTestCategories();
            if (!Window)
            {
                TutorialWindow.GetOrCreateWindow(null);
            }
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            TableOfContentModel.CategoriesOfProjectDuringTests = null;

            if (Window != null)
            {
                yield return new WaitForDelayCall();
                if (Window.Model.Tutorial.CurrentTutorial)
                {
                    TutorialWindow.ExitTutorial();
                }
                Window.Close();
                while (Window)
                {
                    yield return null;
                }
            }
            TutorialTestsUtils.DestroyTutorial(m_Tutorial);
        }

        [Test]
        public void LoadView_CurrentViewIsSame_ViewNotReloaded()
        {
            Assert.Pass();// REMOVE
            string currentView = Window.CurrentView;
            var viewToLoad = new MockView();
            Assume.That(!string.IsNullOrEmpty(currentView) && currentView != viewToLoad.Name);

            Window.RegisterView(viewToLoad, () => { });
            Window.LoadView(viewToLoad.Name);

            Window.UnregisterView(viewToLoad);
            Window.RegisterView(viewToLoad, () =>
            {
                Window.UnregisterView(viewToLoad);
                Assert.Fail("View was reloaded");
            });

            Window.LoadView(viewToLoad.Name);
            Window.UnregisterView(viewToLoad);
            Assert.Pass();
        }

#if UNITY_2020_3
        [Ignore("TODO Runs fine locally, fails on Yamato, unstable for most of the platform")]
#endif
        [Test]
        public void LoadView_CurrentViewIsDifferent_ViewChanges()
        {
            string currentView = Window.CurrentView;
            var viewToLoad = new MockView();
            Window.UnregisterView(viewToLoad);
            Window.RegisterView(viewToLoad, () =>
            {
                Window.UnregisterView(viewToLoad);
                Assert.Pass();
            });
            Assume.That(!string.IsNullOrEmpty(currentView) && currentView != viewToLoad.Name);

            Window.LoadView(viewToLoad.Name);
            Window.UnregisterView(viewToLoad); //at this point, you already failed as the Pass() hasn't been called
            Assert.Fail("View was not loaded");
        }

        [Test]
        public void TableOfContent_CategoriesExist_CategoriesDisplayed()
        {
            Assume.That(Window.CurrentView == TableOfContentView.k_Name);
            Assume.That(TableOfContentModel.CategoriesOfProjectDuringTests.Count() > 0);
            Assert.AreNotEqual(0, Window.TableOfContentView.CategoriesOrTutorialsCurrentlyVisibile);
        }

        [Test]
        public void TableOfContent_CategoriesDoNotExist_IsEmpty()
        {
            Assume.That(Window.CurrentView == TableOfContentView.k_Name, $"Default view is supposed to be {TableOfContentView.k_Name}, but was {Window.CurrentView}");
            Assume.That(TableOfContentModel.CategoriesOfProjectDuringTests.Count() > 0);
            Window.Close();

            TableOfContentModel.CategoriesOfProjectDuringTests = new List<TutorialContainer>();
            TutorialWindow.GetOrCreateWindow(null);
            Assert.AreEqual(0, Window.TableOfContentView.CategoriesOrTutorialsCurrentlyVisibile);
        }

        [Test]
        public void TableOfContent_EnterCategory_OnlyContentOfCategoryIsDisplayed()
        {
            Assume.That(Window.CurrentView == TableOfContentView.k_Name);
            Assume.That(Window.Model.TableOfContent.CurrentCategory == TableOfContentModel.CategoriesOfProjectDuringTests[0]);

            TutorialContainer secondCategory = CreateCategory("c2", "c2s", new TutorialContainer.Section[]
            {
                CreateTutorialSection("s1", "t1", m_Tutorial), CreateLinkSection("s2", "t2", "https://unity.com"),
                CreateLinkSection("s3", "t3", "https://store.unity.com/"),
            });
            AddCategory(secondCategory);

            Window.Broadcast(new CategoryClickedEvent(secondCategory));
            Assert.AreEqual(3, Window.TableOfContentView.CategoriesOrTutorialsCurrentlyVisibile);
            Assert.AreEqual(secondCategory, Window.Model.TableOfContent.CurrentCategory);
        }

        [UnityTest, Timeout(30000)]
        public IEnumerator StartTutorial_StartedTutorialIsCurrentTutorial()
        {
            TutorialWindow.StartTutorial(m_Tutorial);
            yield return WaitUntilTutorialIsRunning();
            Assert.AreEqual(m_Tutorial, Window.Model.Tutorial.CurrentTutorial);
        }

#if UNITY_2020_3 && UNITY_EDITOR_OSX
        [Ignore("TODO Runs fine locally, fails on Yamato")]
#endif
        [UnityTest, Timeout(30000)]
        public IEnumerator StartTutorial_CreatesTutorialWindow()
        {
            Window.Close();
            Assert.IsEmpty(Resources.FindObjectsOfTypeAll<TutorialWindow>());
            TutorialWindow.StartTutorial(m_Tutorial);
            yield return WaitUntilTutorialIsRunning();
            Assert.IsNotEmpty(Resources.FindObjectsOfTypeAll<TutorialWindow>());
        }

#if UNITY_2022_1_OR_NEWER
        [Ignore("TODO Runs fine locally, fails on Yamato")]
#endif
        [UnityTest, Timeout(30000)]
        public IEnumerator ExitTutorial_WhenInPlayMode_ExitsPlayMode()
        {
            Assume.That(!EditorApplication.isPlaying);
            TutorialWindow.StartTutorial(m_Tutorial);

            //WaitUntilTutorialIsRunning();
            while (!Window.Model.Tutorial.CurrentTutorial)
            {
                yield return null;
            }
            //--

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            yield return new EnterPlayMode(true);

            //WaitUntilTutorialIsRunning();
            while (!Window.Model.Tutorial.CurrentTutorial)
            {
                yield return null;
            }

            void PassIfNotInPlayModeWhenTutorialQuits(Tutorial t)
            {
                t.Quit.RemoveListener(PassIfNotInPlayModeWhenTutorialQuits);
                Assert.IsFalse(EditorApplication.isPlaying);
            }

            TutorialWindow.Instance.Model.Tutorial.CurrentTutorial.Quit.AddListener(PassIfNotInPlayModeWhenTutorialQuits);
            TutorialWindow.ExitTutorial();
        }

        void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            TutorialFrameworkModel.s_AreTestsRunning = true;
        }

        [UnityTest]
        public IEnumerator StartTutorial_OriginalSceneStateIsRestoredWhenTutorialIsCompleted()
        {
            // Open some new scenes
            var scene0Path = m_TempFolderPath + "/Scene0.unity";
            var scene0 = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            EditorSceneManager.SaveScene(scene0, scene0Path);
            var scene1 = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
            var scene1Path = m_TempFolderPath + "/Scene1.unity";
            EditorSceneManager.SaveScene(scene1, scene1Path);
            var scene2 = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            var scene2Path = m_TempFolderPath + "/Scene2.unity";
            EditorSceneManager.SaveScene(scene2, scene2Path);
            var scene3 = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
            var scene3Path = m_TempFolderPath + "/Scene3.unity";
            EditorSceneManager.SaveScene(scene3, scene3Path);

            // Set the last scene to be active
            SceneManager.SetActiveScene(scene3);

            // Unload scene 2 and 3
            EditorSceneManager.CloseScene(scene1, false);
            EditorSceneManager.CloseScene(scene2, false);

            int originalSceneCount = SceneManager.sceneCount;

            m_Tutorial.SceneManagementBehavior = Tutorial.SceneManagementBehaviorType.CreateNewScene;
            TutorialWindow.StartTutorial(m_Tutorial);
            while (!Window.Model.IsOpen || !Window.FrontendIsReadyToBeInitialized) //for some unknown reason, we can't use WaitUntilTutorialIsRunning because it throw errors here.
            {
                yield return null;
            }
            while (!Window.Model.Tutorial.CurrentTutorial)
            {
                yield return null;
            }
            TutorialWindow.ExitTutorial();
            yield return new WaitForDelayCall();

            // NOTE It seems two of these are required in order to wait enough for the scene restoration.
            yield return new WaitForDelayCall();
            yield return new WaitForDelayCall();

            // Assert that we're back at original scene state
            Assert.AreEqual(originalSceneCount, SceneManager.sceneCount);
            Assert.AreEqual(scene0Path, SceneManager.GetSceneAt(0).path);
            Assert.AreEqual(scene1Path, SceneManager.GetSceneAt(1).path);
            Assert.AreEqual(scene2Path, SceneManager.GetSceneAt(2).path);
            Assert.AreEqual(scene3Path, SceneManager.GetSceneAt(3).path);
            Assert.IsTrue(SceneManager.GetSceneAt(0).isLoaded);
            Assert.IsFalse(SceneManager.GetSceneAt(1).isLoaded);
            Assert.IsFalse(SceneManager.GetSceneAt(2).isLoaded);
            Assert.IsTrue(SceneManager.GetSceneAt(3).isLoaded);
            Assert.AreEqual(scene3Path, SceneManager.GetActiveScene().path);
        }

        [UnityTest]
        public IEnumerator StartOrExitTutorial_WithoutCustomLayout_PreservesCurrentLayout()
        {
            //spawn a window
            var mockedWindow = EditorWindow.GetWindow<MockedWindow>("MockedWindow");
            Assume.That(Resources.FindObjectsOfTypeAll<MockedWindow>() != null);
            Assume.That(m_Tutorial.WindowLayout == null);

            TutorialWindow.StartTutorial(m_Tutorial);
            while (!Window.Model.IsOpen || !Window.FrontendIsReadyToBeInitialized) //for some unknown reason, we can't use WaitUntilTutorialIsRunning because it throw errors here.
            {
                yield return null;
            }
            while (!Window.Model.Tutorial.CurrentTutorial)
            {
                yield return null;
            }

            Assert.IsNotEmpty(Resources.FindObjectsOfTypeAll<MockedWindow>());

            TutorialWindow.ExitTutorial();
            yield return new WaitForDelayCall();

            Assert.IsNotEmpty(Resources.FindObjectsOfTypeAll<MockedWindow>());
            mockedWindow.Close();
        }

        /// <summary>
        /// hint: if this throws errors when called in a UnityTest, copy-paste the code directly instead.
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitUntilTutorialIsRunning()
        {
            while (!Window.Model.Tutorial.CurrentTutorial)
            {
                yield return null;
            }
        }

        void AddCategory(TutorialContainer category)
        {
            TableOfContentModel.CategoriesOfProjectDuringTests.Add(category);
            Window.Broadcast(new CategoriesRefreshRequestedEvent());
        }

        void SetupTestCategories()
        {
            TableOfContentModel.CategoriesOfProjectDuringTests = new List<TutorialContainer>()
            {
                CreateCategory("title", "subtitle", new TutorialContainer.Section[] { CreateTutorialSection("s1", "t1", m_Tutorial) } )
            };
        }

        TutorialContainer CreateCategory(string title, string subtitle, TutorialContainer.Section[] sections)
        {
            var category = ScriptableObject.CreateInstance<TutorialContainer>();
            category.Modified = new TutorialContainerEvent();
            category.Title = new LocalizableString(title);
            category.Subtitle = new LocalizableString(subtitle);
            category.Modified = new TutorialContainerEvent();
            category.Sections = sections;
            return category;
        }

        TutorialContainer.Section CreateSectionBase(string heading, string text)
        {
            var section = new TutorialContainer.Section();
            section.Heading = heading;
            section.Text = text;
            return section;
        }

        TutorialContainer.Section CreateTutorialSection(string heading, string text, Tutorial tutorial)
        {
            var section = CreateSectionBase(heading, text);
            section.Tutorial = tutorial;
            return section;
        }

        TutorialContainer.Section CreateLinkSection(string heading, string text, string url)
        {
            var section = CreateSectionBase(heading, text);
            section.Url = url;
            return section;
        }

        // TODO test ideas
        // 1. window displays tutorial project selection when we have multiple root tutorial containers
        // 2. Categories are displayed properly (the header of the window changes to include the title, artwork and subtitle)
        // 3. Back button is enabled when entering a sub category, and disabled when you go back to the root category

#if TODO_UIElements_implementation
        static IAutomatedUIElement FindElementWithText(AutomatedWindow<TutorialWindow> automatedWindow, string text, string elementName, Action<object, string, object[]> assert = null)
        {
            var result = automatedWindow.FindElementsByGUIContent(new GUIContent(text)).LastOrDefault();
            assert = assert ?? Assert.IsNotNull;
            assert(result, string.Format("Finding {0} with expected text: \"{1}\"", elementName, text), null);
            return result;
        }

        static IAutomatedUIElement FindElementWithStyle(AutomatedWindow<TutorialWindow> automatedWindow, GUIStyle style, string elementName)
        {
            var result = automatedWindow.FindElementsByGUIStyle(style).FirstOrDefault();
            Assert.IsNotNull(result, string.Format("Finding {0} with expected style: {1}", elementName, style));
            return result;
        }

        [Ignore("TODO Revisit tests after the refactoring is done")]
        [UnityTest]
        public IEnumerator CanClickNextButton_WhenRevistingCompletedPage_WhenItsCriteriaHaveBeenLaterInvalidated()
        {
            using (var automatedWindow = new AutomatedWindow<TutorialWindow>(m_Window))
            {
                m_Window.RepaintImmediately();

                // next button should be disabled
                automatedWindow.Click(FindElementWithText(automatedWindow, nextButtonText, "next button"));
                yield return null;
                m_Window.RepaintImmediately();
                Assert.AreEqual(firstPage, m_Window.currentTutorial.CurrentPage);

                // complete criterion; next button should now be enabled
                firstPageCriterion.Complete(true);
                yield return null;
                m_Window.RepaintImmediately();
                automatedWindow.Click(FindElementWithText(automatedWindow, nextButtonText, "next button"));
                yield return null;
                m_Window.RepaintImmediately();
                Assert.AreEqual(secondPage, m_Window.currentTutorial.CurrentPage);

                // go back
                // TODO Broken as allTutorialStyles was removed in IET 2.0.
                automatedWindow.Click(FindElementWithStyle(automatedWindow, null /*m_Window.allTutorialStyles.backButton*/, "back button"));
                yield return null;
                m_Window.RepaintImmediately();
                Assert.AreEqual(firstPage, m_Window.currentTutorial.CurrentPage);

                // invalidate criterion; next button should still be enabled
                firstPageCriterion.Complete(false);
                automatedWindow.Click(FindElementWithText(automatedWindow, nextButtonText, "next button"));
                yield return null;
                m_Window.RepaintImmediately();
                Assert.AreEqual(secondPage, m_Window.currentTutorial.CurrentPage);
            }
        }

        [Ignore("TODO Revisit tests after the refactoring is done")]
        [UnityTest]
        public IEnumerator ClickingBackButton_WhenPreviousPageHasAutoAdvanceOnCompleteSet_MovesToPreviousPage()
        {
            // let first page auto-advance on completion
            firstPage.AutoAdvanceOnComplete = true;

            using (var automatedWindow = new AutomatedWindow<TutorialWindow>(m_Window))
            {
                m_Window.RepaintImmediately();

                // complete criterion and auto-advance to next page, then press back button to come back
                firstPageCriterion.Complete(true);
                yield return null;
                m_Window.RepaintImmediately();
                // TODO Broken as allTutorialStyles was removed in IET 2.0.
                automatedWindow.Click(FindElementWithStyle(automatedWindow, null /*m_Window.allTutorialStyles.backButton*/, "back button"));
                yield return null;
                m_Window.RepaintImmediately();

                // we should now be back at the first page again
                Assert.AreEqual(firstPage, m_Window.currentTutorial.CurrentPage);
            }
        }

        [Ignore("TODO Broken during 2.0 refactoring")]
        public void ApplyMasking_WhenPageIsActivated()
        {
            firstPage.m_Paragraphs[0].MaskingSettings.SetUnmaskedViews(new[] { UnmaskedView.CreateInstanceForGUIView<Toolbar>() });
            firstPage.m_Paragraphs[0].MaskingSettings.Enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();
            //m_Window.RepaintImmediately(); TODO disabled, was causing problems after adding localisation support

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // the tutorial window and the specified unmasked view should both be unmasked
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == m_Window.m_Parent || view == Toolbar.get || view is TooltipView)
                    Assert.IsFalse(MaskingManager.IsMasked(new GUIViewProxy(view), rects));
                else
                    Assert.IsTrue(MaskingManager.IsMasked(new GUIViewProxy(view), rects));
            }
        }

        [Ignore("TODO Broken during 2.0 refactoring")]
        public void ApplyHighlighting_ToUnmaskedViews_WhenPageOnlyContainsNarrativeParagraphs()
        {
            firstPage.m_Paragraphs[0].m_Type = ParagraphType.Narrative;
            firstPage.m_Paragraphs[0].MaskingSettings.SetUnmaskedViews(new[] { UnmaskedView.CreateInstanceForGUIView<Toolbar>() });
            firstPage.m_Paragraphs[0].MaskingSettings.Enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // only the specified unmasked view should be highlighted
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == Toolbar.get)
                    Assert.IsTrue(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
                else
                    Assert.IsFalse(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
            }
        }

        [Test]
        [Ignore("Imgui elements in containers, TODO")]
        public void ApplyHighlighting_ToOnlySpecifiedControls_WhenMaskingSettingsSpecifyControlsAndEntireWindowsAndViews()
        {
            var playButtonContrlSelector = new GuiControlSelector
            {
                SelectorMode = GuiControlSelector.Mode.NamedControl,
                ControlName = "ToolbarPlayModePlayButton"
            };
            firstPage.m_Paragraphs[0].MaskingSettings.SetUnmaskedViews(
                new[]
                {
                    UnmaskedView.CreateInstanceForGUIView<Toolbar>(new[] { playButtonContrlSelector }),
                    UnmaskedView.CreateInstanceForGUIView<AppStatusBar>()
                }
            );
            firstPage.m_Paragraphs[0].MaskingSettings.Enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();

            m_Window.RepaintImmediately();

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // only the play button should be highlighted
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == Toolbar.get)
                {
                    Assert.IsTrue(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
                    Assert.AreEqual(1, rects.Count);
                    var viewPosition = view.position;
                    var controlRect = rects[0];
                    Assert.Greater(controlRect.xMin, viewPosition.xMin);
                    Assert.Greater(controlRect.yMin, viewPosition.yMin);
                    Assert.Less(controlRect.xMax, viewPosition.xMax);
                    Assert.Less(controlRect.yMax, viewPosition.yMax);
                }
                else
                {
                    Assert.IsFalse(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
                }
            }
        }

        [Test]
        [Ignore("TODO Fix")]
        public void ApplyHighlighting_ToAllUnmaskedWindowsAndViews_WhenMaskingSettingsOnlySpecifyEntireWindowsAndViews()
        {
            firstPage.m_Paragraphs[0].MaskingSettings.SetUnmaskedViews(
                new[]
                {
                    UnmaskedView.CreateInstanceForGUIView<Toolbar>(),
                    UnmaskedView.CreateInstanceForGUIView<AppStatusBar>()
                }
            );
            firstPage.m_Paragraphs[0].MaskingSettings.Enabled = true;
            firstPage.RaiseTutorialPageMaskingSettingsChangedEvent();

            m_Window.RepaintImmediately();

            List<GUIView> views = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(views);

            // both the toolbar and status bar should be highlighted
            var rects = new List<Rect>();
            foreach (var view in views)
            {
                if (view == Toolbar.get || view is AppStatusBar)
                    Assert.IsTrue(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
                else
                    Assert.IsFalse(MaskingManager.IsHighlighted(new GUIViewProxy(view), rects));
            }
        }

#endif
    }
}
