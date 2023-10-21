using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class EditorWindowCriterionTests : CriterionTestBase<EditorWindowCriterion>
    {
        class TestWindowA : EditorWindow
        {
        }

        [TearDown]
        public void Teardown()
        {
            var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();

            foreach (var w in windows)
            {
                if (w.GetType() == typeof(TestWindowA))
                {
                    w.Close();
                }
            }
        }

        // TODO Pretty much all of these tets crash 2019.1
#if !UNITY_2019_1_OR_NEWER
        [UnityTest]
        public IEnumerator EditorWindowTypeIsNotNull_WindowOfTypeExists_IsCompleted()
        {
            m_Criterion.editorWindowType = new SerializedType(typeof(TestWindowA));
            var w = EditorWindow.GetWindow<TestWindowA>();

            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator EditorWindowTypeIsNotNull_WindowOfTypeDoesNotExist_IsNotCompleted()
        {
            m_Criterion.editorWindowType = new SerializedType(typeof(TestWindowA));

            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator EditorWindowTypeIsNull_IsNotCompleted()
        {
            yield return null;
            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator EditorWindowInstanceIsNotFound_IsNotCompleted()
        {
            m_Criterion.editorWindowType = new SerializedType(typeof(TestWindowA));
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator Autocomplete_EditorWindowTypeIsNotNull_IsCompleted()
        {
            m_Criterion.editorWindowType = new SerializedType(typeof(TestWindowA));

            Assert.IsTrue(m_Criterion.AutoComplete());
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator Autocomplete_EditorWindowTypeIsNull_IsNotCompleted()
        {
            Assert.IsFalse(m_Criterion.AutoComplete());
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

#endif
    }
}
