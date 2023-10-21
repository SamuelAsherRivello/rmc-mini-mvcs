using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class ArbitraryCriterionTests : CriterionTestBase<ArbitraryCriterion>
    {
        class CallbackHandler : ScriptableObject
        {
            public bool DoesFooExist()
            {
                return GameObject.Find("Foo") != null;
            }

            public bool AutoComplete()
            {
                var foo = GetOrCreateFoo();
                return foo != null;
            }

            public static GameObject GetOrCreateFoo()
            {
                var foo = GameObject.Find("Foo");
                if (!foo)
                    foo = new GameObject("Foo");
                return foo;
            }

            public static void DeleteFoo()
            {
                var foo = GameObject.Find("Foo");
                if (foo)
                    DestroyImmediate(foo);
            }
        }

        CallbackHandler m_CallbackHandler;

        [SetUp]
        public void Setup()
        {
            m_CallbackHandler = ScriptableObject.CreateInstance<CallbackHandler>();
            m_CallbackHandler.hideFlags = HideFlags.HideAndDontSave;
            CallbackHandler.DeleteFoo();
        }

        [UnityTest]
        public IEnumerator NullCallback_IsNotCompleted()
        {
            m_Criterion.Callback = null;
            yield return null;
            Assert.IsFalse(m_Criterion.IsCompleted);
        }

        [UnityTest]
        public IEnumerator ValidCallback_IsCompleted()
        {
            m_Criterion.Callback = new ArbitraryCriterion.BoolCallback();
            m_Criterion.Callback.SetMethod(m_CallbackHandler, "DoesFooExist", dynamic: false);
            CallbackHandler.GetOrCreateFoo();
            yield return null;
            Assert.IsTrue(m_Criterion.IsCompleted);
        }

        [UnityTest]
        public IEnumerator NullAutoCompleteCallback_IsNotCompleted()
        {
            m_Criterion.AutoCompleteCallback = null;
            Assert.IsFalse(m_Criterion.AutoComplete());
            yield return null;
            Assert.IsFalse(m_Criterion.IsCompleted);
        }

        [UnityTest]
        public IEnumerator ValidAutoCompleteCallback_IsCompleted()
        {
            m_Criterion.Callback = new ArbitraryCriterion.BoolCallback();
            m_Criterion.Callback.SetMethod(m_CallbackHandler, "DoesFooExist", dynamic: false);
            m_Criterion.AutoCompleteCallback = new ArbitraryCriterion.BoolCallback();
            m_Criterion.AutoCompleteCallback.SetMethod(m_CallbackHandler, "AutoComplete", dynamic: false);
            Assert.IsTrue(m_Criterion.AutoComplete());
            yield return null;
            Assert.IsTrue(m_Criterion.IsCompleted);
        }
    }
}
