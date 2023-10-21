using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    internal class MockCriterion : Criterion
    {
        public void Complete(bool complete)
        {
            IsCompleted = complete;
        }

        protected override bool EvaluateCompletion()
        {
            return IsCompleted;
        }

        public override bool AutoComplete()
        {
            return true;
        }
    }

    internal static class TutorialTestsUtils
    {
        static string DoneButtonText => $"{TestContext.CurrentContext.Test.FullName}-DONE";
        static string NextButtonText => $"{TestContext.CurrentContext.Test.FullName}-NEXT";

        internal static TutorialParagraph CreateNarrativeParagraph()
        {
            return TutorialParagraph.CreateNarrativeParagraph("NarrativeTitle", "NarrativeText");
        }
        internal static TutorialParagraph CreateInstructiveParagraphWithMockedCriteria(int criteriaCount = 1)
        {
            var paragraphCriteria = new TypedCriterionCollection();
            for (int i = 0; i < criteriaCount; i++)
            {
                paragraphCriteria.AddItem(GenerateCriterion<MockCriterion>());
            }
            return TutorialParagraph.CreateInstructionParagraph("InstructiveTitle", "InstructiveText", paragraphCriteria);
        }

        internal static TypedCriterion GenerateCriterion<T>() where T : Criterion
        {
            return new TypedCriterion(new SerializedType(typeof(T)), ScriptableObject.CreateInstance<T>());
        }

        internal static Tutorial CreateMockTutorial(params TutorialPage[] pages)
        {
            AssetDatabase.Refresh();
            AssetDatabase.CreateFolder(TutorialTestsSetup.s_TempFolderPath, TestContext.CurrentContext.Test.MethodName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Tutorial.TutorialPageCollection pagesCollection = new Tutorial.TutorialPageCollection(pages);
            var tutorial = ScriptableObject.CreateInstance<Tutorial>();
            tutorial.name = $"{TestContext.CurrentContext.Test.FullName}-Tutorial";
            tutorial.ProgressTrackingEnabled = false;
            tutorial.PagesCollection = pagesCollection;
            tutorial.SceneManagementBehavior = Tutorial.SceneManagementBehaviorType.UseActiveScene;
            for (int i = 0; i < tutorial.PagesCollection.Count; i++)
            {
                tutorial.PagesCollection[i].name = $"{TestContext.CurrentContext.Test.FullName}-PAGE-{i + 1}";
                tutorial.PagesCollection[i].DoneButton = DoneButtonText;
                tutorial.PagesCollection[i].NextButton = NextButtonText;
                AssetDatabase.CreateAsset(tutorial.PagesCollection[i], $"{TutorialTestsSetup.s_TempFolderPath}/{TestContext.CurrentContext.Test.MethodName}/Page{i}.asset");
                EditorUtility.SetDirty(tutorial.PagesCollection[i]);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            AssetDatabase.CreateAsset(tutorial, $"{TutorialTestsSetup.s_TempFolderPath}/{TestContext.CurrentContext.Test.MethodName}/Tutorial.asset");
            EditorUtility.SetDirty(tutorial);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return tutorial;
        }

        internal static Tutorial CreateMockTutorial()
        {
            var instructivePage = TutorialPage.Create(CreateInstructiveParagraphWithMockedCriteria());
            var narrativePage = TutorialPage.Create(CreateNarrativeParagraph());
            return CreateMockTutorial(instructivePage, narrativePage);
        }

        internal static void DestroyTutorial(Tutorial tutorial)
        {
            if (tutorial == null)
            {
                return;
            }

            foreach (var page in tutorial.PagesCollection)
            {
                if (page == null)
                {
                    continue;
                }

                foreach (var paragraph in page.Paragraphs)
                {
                    if (paragraph == null)
                    {
                        continue;
                    }

                    foreach (var criterion in paragraph.Criteria)
                    {
                        if (criterion != null)
                        {
                            if (criterion.Criterion != null)
                            {
                                criterion.Criterion.StopTesting();
                                UnityEngine.Object.DestroyImmediate(criterion.Criterion, true);
                            }
                        }
                    }
                }
                UnityEngine.Object.DestroyImmediate(page, true);
            }
            UnityEngine.Object.DestroyImmediate(tutorial, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    public class TutorialTests
    {
        [Test]
        public void AllCriteriaSatisfied_CanMoveToNextPage()
        {
            var tutorial = TutorialTestsUtils.CreateMockTutorial();
            Assume.That(!tutorial.CurrentPageIsLast());

            var criterion = tutorial.PagesCollection[0].Paragraphs[0].CriteriaList[0].Criterion as MockCriterion;

            criterion.Complete(false);
            Assume.That(!tutorial.CurrentPage.AreAllCriteriaSatisfied);
            Assert.IsFalse(tutorial.CanMoveToNextPage);
            Assert.IsFalse(tutorial.TryGoToNextPage());

            criterion.Complete(true);
            Assume.That(tutorial.CurrentPage.AreAllCriteriaSatisfied);
            Assert.IsTrue(tutorial.CanMoveToNextPage);
            Assert.IsTrue(tutorial.TryGoToNextPage());
            TutorialTestsUtils.DestroyTutorial(tutorial);
        }

        [Test]
        public void CriteraInvalid_CanNotMoveToNextPageAnymore()
        {
            var instructivePage = TutorialPage.Create(TutorialTestsUtils.CreateInstructiveParagraphWithMockedCriteria());
            var narrativePage = TutorialPage.Create(TutorialTestsUtils.CreateNarrativeParagraph());
            var tutorial = TutorialTestsUtils.CreateMockTutorial(instructivePage, narrativePage);
            var criterion = instructivePage.Paragraphs[0].CriteriaList[0].Criterion as MockCriterion;
            Assume.That(!tutorial.CurrentPageIsLast());
            Assume.That(!tutorial.CurrentPage.AreAllCriteriaSatisfied);

            criterion.Complete(true);
            Assert.IsTrue(tutorial.CanMoveToNextPage);

            criterion.Complete(false);
            Assert.IsFalse(tutorial.CanMoveToNextPage);
            Assert.IsFalse(tutorial.TryGoToNextPage());
            TutorialTestsUtils.DestroyTutorial(tutorial);
        }

#if UNITY_2022_1_OR_NEWER
        [Ignore("Works fine locally, fails on Katana")]
#endif
        [Test]
        public void EnoughCriteriaSatisfied_CanMoveToNextPage()
        {
            TutorialParagraph paragraph = TutorialTestsUtils.CreateInstructiveParagraphWithMockedCriteria(2);
            paragraph.m_CriteriaCompletion = CompletionType.CompletedWhenAllAreTrue;

            var instructivePage = TutorialPage.Create(paragraph);
            var narrativePage = TutorialPage.Create(TutorialTestsUtils.CreateNarrativeParagraph());
            var tutorial = TutorialTestsUtils.CreateMockTutorial(instructivePage, narrativePage);
            var firstCriterion = instructivePage.Paragraphs[0].CriteriaList[0].Criterion as MockCriterion;
            var secondCriterion = instructivePage.Paragraphs[0].CriteriaList[1].Criterion as MockCriterion;
            Assume.That(!tutorial.CurrentPageIsLast());

            firstCriterion.Complete(false);
            secondCriterion.Complete(false);
            Assume.That(!tutorial.CurrentPage.AreAllCriteriaSatisfied);
            Assert.IsFalse(tutorial.CanMoveToNextPage);
            Assert.IsFalse(tutorial.TryGoToNextPage());

            firstCriterion.Complete(true);
            Assume.That(!tutorial.CurrentPage.AreAllCriteriaSatisfied);
            Assert.IsFalse(tutorial.CanMoveToNextPage);

            paragraph.m_CriteriaCompletion = CompletionType.CompletedWhenAnyIsTrue;
            Assert.IsTrue(tutorial.CanMoveToNextPage);

            secondCriterion.Complete(true);
            Assert.IsTrue(tutorial.CanMoveToNextPage);

            paragraph.m_CriteriaCompletion = CompletionType.CompletedWhenAllAreTrue;
            Assert.IsTrue(tutorial.CanMoveToNextPage);

            TutorialTestsUtils.DestroyTutorial(tutorial);
        }
    }
}
