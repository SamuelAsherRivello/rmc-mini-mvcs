using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using NUnit.Framework;
using UnityObject = UnityEngine.Object;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class TutorialPageTests
    {
        class MockCriterion : Criterion
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

        TutorialPage m_Page;
        MockCriterion m_Criterion;

        [SetUp]
        public void SetUp()
        {
            m_Page = ScriptableObject.CreateInstance<TutorialPage>();

            var instructionParagraph = new TutorialParagraph();
            instructionParagraph.m_Type = ParagraphType.Instruction;

            m_Criterion = ScriptableObject.CreateInstance<MockCriterion>();
            m_Criterion.Completed = new CriterionEvent();
            m_Criterion.Invalidated = new CriterionEvent();

            var typedCriterion = new TypedCriterion(new SerializedType(typeof(MockCriterion)), m_Criterion);
            instructionParagraph.m_Criteria = new TypedCriterionCollection(new[] { typedCriterion });

            m_Page.m_Paragraphs = new TutorialParagraphCollection(new[] { instructionParagraph });

            m_Page.Initiate();
        }

        [TearDown]
        public void TearDown()
        {
            if (m_Page == null)
                return;

            foreach (var paragraph in m_Page.Paragraphs)
            {
                foreach (var criterion in paragraph.Criteria)
                {
                    if (criterion != null && criterion.Criterion != null)
                        UnityObject.DestroyImmediate(criterion.Criterion);
                }
            }

            UnityObject.DestroyImmediate(m_Page);
        }

        [Test]
        public void PageMarkedIncomplete_WhenACriterionIsInvalidated()
        {
            Assert.IsFalse(m_Page.AreAllCriteriaSatisfied);
            m_Criterion.Complete(true);
            Assert.IsTrue(m_Page.AreAllCriteriaSatisfied);
            m_Criterion.Complete(false);
            Assert.IsFalse(m_Page.AreAllCriteriaSatisfied);
        }

        [Test]
        public void CriterionInvalidated_PageCompletionStatusUpdates()
        {
            Assume.That(!m_Page.AreAllCriteriaSatisfied);

            m_Criterion.Complete(true);
            Assert.IsTrue(m_Page.AreAllCriteriaSatisfied);

            m_Criterion.Complete(false);
            Assert.IsFalse(m_Page.AreAllCriteriaSatisfied);
        }

        [Test]
        public void PageEvents_OnBeforeTutorialQuit_RunsWhenInvoked()
        {
            m_Page.m_OnBeforeTutorialQuit = new UnityEngine.Events.UnityEvent();
            m_Page.m_OnBeforeTutorialQuit.AddListener(() => Assert.Pass());
            m_Page.RaiseOnBeforeTutorialQuit();
            Assert.Fail();
        }

        [Test]
        public void PageEvents_OnTutorialPageStay_RunsWhenInvoked()
        {
            m_Page.Staying = new TutorialPageEvent();
            m_Page.Staying.AddListener((sender) => Assert.AreEqual(m_Page, sender));

            m_Page.m_OnTutorialPageStay = new UnityEngine.Events.UnityEvent();
            m_Page.m_OnTutorialPageStay.AddListener(() => Assert.Pass());

            m_Page.RaiseStaying();

            Assert.Fail();
        }
    }
}
