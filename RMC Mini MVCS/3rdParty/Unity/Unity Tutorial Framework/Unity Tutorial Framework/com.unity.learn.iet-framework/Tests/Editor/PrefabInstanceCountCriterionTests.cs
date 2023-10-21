using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class PrefabInstanceCountCriterionTests : CriterionTestBase<PrefabInstanceCountCriterion>
    {
        List<UnityObject> m_PrefabInstances = new List<UnityObject>();

        [SetUp]
        public void OpenSceneAndSetUpCriterion()
        {
            EditorSceneManager.OpenScene(GetTestAssetPath("EmptyTestScene.unity"));

            m_Criterion.PrefabParent = AssetDatabase.LoadAssetAtPath<GameObject>(GetTestAssetPath("TestPrefab1.prefab"));
            Assert.IsNotNull(m_Criterion.PrefabParent, "Cannot load test prefab");
        }

        [TearDown]
        public void DestroyPrefabInstances()
        {
            foreach (var prefabInstance in m_PrefabInstances)
                UnityObject.DestroyImmediate(prefabInstance);
            m_PrefabInstances.Clear();
        }

        // TODO Pretty much all of these tets crash 2019.1
#if !UNITY_2019_1_OR_NEWER
        [UnityTest]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.AtLeast, 2, 0, false, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.AtLeast, 2, 1, false, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.AtLeast, 2, 2, true, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.AtLeast, 2, 3, true, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.Exactly, 2, 0, false, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.Exactly, 2, 1, false, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.Exactly, 2, 2, true, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.Exactly, 2, 3, false, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.NoMoreThan, 2, 0, true, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.NoMoreThan, 2, 1, true, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.NoMoreThan, 2, 2, true, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.NoMoreThan, 2, 3, false, ExpectedResult = null)]
        public IEnumerator IsCompletedWhenExpected(PrefabInstanceCountCriterion.InstanceCountComparison comparisonMode, int instanceCount, int actualNumberOfInstances, bool expectedCompletion)
        {
            for (var i = 0; i < actualNumberOfInstances; i++)
                m_PrefabInstances.Add(PrefabUtility.InstantiatePrefab(m_Criterion.prefabParent));

            m_Criterion.comparisonMode = comparisonMode;
            m_Criterion.instanceCount = instanceCount;
            foreach (var _ in TriggerSelectionChanged())
                yield return null;

            Assert.AreEqual(expectedCompletion, m_Criterion.completed);
        }

        [UnityTest]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.AtLeast, 2, 0, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.AtLeast, 2, 1, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.AtLeast, 2, 2, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.AtLeast, 2, 3, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.Exactly, 2, 0, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.Exactly, 2, 1, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.Exactly, 2, 2, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.Exactly, 2, 3, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.NoMoreThan, 2, 0, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.NoMoreThan, 2, 1, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.NoMoreThan, 2, 2, ExpectedResult = null)]
        [TestCase(PrefabInstanceCountCriterion.InstanceCountComparison.NoMoreThan, 2, 3, ExpectedResult = null)]
        public IEnumerator AutoComplete_IsCompleted(PrefabInstanceCountCriterion.InstanceCountComparison comparisonMode, int instanceCount, int actualNumberOfInstances)
        {
            for (var i = 0; i < actualNumberOfInstances; i++)
                m_PrefabInstances.Add(PrefabUtility.InstantiatePrefab(m_Criterion.prefabParent));

            m_Criterion.comparisonMode = comparisonMode;
            m_Criterion.instanceCount = instanceCount;

            m_Criterion.AutoComplete();

            // Completion is evaulated when selection changes
            Selection.objects = new UnityObject[] { m_Criterion.prefabParent };
            yield return null;
            Selection.objects = new UnityObject[0];
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

        public void WhenPrefabInstanceExistsAlready_IsCompleted()
        {
            m_Criterion.StopTesting();

            var prefabInstance = PrefabUtility.InstantiatePrefab(m_Criterion.prefabParent);
            m_PrefabInstances.Add(prefabInstance);

            m_Criterion.comparisonMode = PrefabInstanceCountCriterion.InstanceCountComparison.AtLeast;
            m_Criterion.instanceCount = 1;

            m_Criterion.StartTesting();

            Assert.IsTrue(m_Criterion.completed);
        }

        public void WhenInstanceCountComparisonIsExactlyOne_FutureReferenceIsCreated()
        {
            m_Criterion.comparisonMode = PrefabInstanceCountCriterion.InstanceCountComparison.Exactly;
            m_Criterion.instanceCount = 1;
            TriggerValidation();

            Assert.AreEqual(1, futureReferences.Count());
            Assert.IsNotNull(futureReferences.First());
            Assert.AreEqual("Prefab Instance", futureReferences.First().referenceName);
        }

        [UnityTest]
        public IEnumerator OnCompletion_WhenInstanceCountComparisonIsExactlyOne_FutureReferenceIsUpdatedToPrefabInstance()
        {
            m_Criterion.comparisonMode = PrefabInstanceCountCriterion.InstanceCountComparison.Exactly;
            m_Criterion.instanceCount = 1;
            TriggerValidation();

            var prefabInstance = PrefabUtility.InstantiatePrefab(m_Criterion.prefabParent);
            m_PrefabInstances.Add(prefabInstance);
            foreach (var _ in TriggerSelectionChanged())
                yield return null;

            Assert.AreEqual(prefabInstance, futureReferences.First().sceneObjectReference.ReferencedObject);
        }

#endif
    }
}
