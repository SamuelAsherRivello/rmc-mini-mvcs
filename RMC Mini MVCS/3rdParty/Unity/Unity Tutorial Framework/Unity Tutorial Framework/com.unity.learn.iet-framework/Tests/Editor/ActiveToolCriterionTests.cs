using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class ActiveToolCriterionTests : CriterionTestBase<ActiveToolCriterion>
    {
        [SetUp]
        public void Setup()
        {
            Tools.current = Tool.None;
        }

        [UnityTest]
        public IEnumerator WhenToolsDoNotMatch_IsNotCompleted()
        {
            m_Criterion.TargetTool = Tool.Move;
            yield return null;

            Assert.IsFalse(m_Criterion.IsCompleted);
        }

        [UnityTest]
        public IEnumerator ActivatingRequiredTool_IsCompleted()
        {
            m_Criterion.TargetTool = Tool.Move;
            Tools.current = Tool.Move;

            yield return null;

            Assert.IsTrue(m_Criterion.IsCompleted);
        }

        [UnityTest]
        public IEnumerator AutoComplete_IsCompleted()
        {
            m_Criterion.TargetTool = Tool.Rotate;
            Assert.IsTrue(m_Criterion.AutoComplete());
            yield return null;

            Assert.IsTrue(m_Criterion.IsCompleted);
        }
    }
}
