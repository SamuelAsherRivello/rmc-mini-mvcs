using System;
using System.Linq;
using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class RequiredSelectionCriterionTests : CriterionTestBase<RequiredSelectionCriterion>
    {
        GameObject m_ReferencedObject;
        ObjectReference m_ObjectReference;

        [SetUp]
        public void OpenSceneAndSetUpCriterion()
        {
            EditorSceneManager.OpenScene(GetTestAssetPath("EmptyTestScene.unity"));

            m_ReferencedObject = new GameObject();

            m_ObjectReference = new ObjectReference();
            m_ObjectReference.SceneObjectReference.Update(m_ReferencedObject);

            m_Criterion.SetObjectReferences(new[] { m_ObjectReference });
        }

        // TODO Pretty much all of these tets crash 2019.1
#if !UNITY_2019_1_OR_NEWER
        [Test]
        public void WhenObjectReferencesIsEmpty_IsCompleted()
        {
            Selection.objects = Enumerable.Empty<UnityEngine.Object>().ToArray();
            m_Criterion.SetObjectReferences(Enumerable.Empty<ObjectReference>());

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenSelectionIsEmpty_IsNotCompleted()
        {
            Selection.objects = new UnityObject[0];
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenReferencedObjectIsSelected_IsCompleted()
        {
            Selection.objects = new UnityObject[] { m_ReferencedObject };
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenReferencedObjectWasPreviouslySelected_IsNotCompleted()
        {
            Selection.objects = new UnityObject[] { m_ReferencedObject };
            yield return null;
            Selection.objects = new UnityObject[0];
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenSelectedObjectIsNotReferencedObject_IsNotCompleted()
        {
            var gameObject = new GameObject();
            Selection.objects = new UnityObject[] { gameObject };
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenReferencedObjectIsAlreadySelected_IsCompleted()
        {
            m_Criterion.StopTesting();

            Selection.objects = new UnityObject[] { m_ReferencedObject };
            yield return null;

            m_Criterion.StartTesting();

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator AutoComplete_WhenReferencedObjectIsNull_ReturnsFalseAndIsNotCompleted()
        {
            var nullObjectReference = new ObjectReference();
            nullObjectReference.sceneObjectReference.Update(null);
            m_Criterion.SetObjectReferences(new[] { nullObjectReference });

            Assert.IsFalse(m_Criterion.AutoComplete());
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator AutoComplete_WhenReferencedObjectIsGameObject_ReturnsTrueAndIsCompleted()
        {
            Assert.IsTrue(m_Criterion.AutoComplete());
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

#endif
    }
}
