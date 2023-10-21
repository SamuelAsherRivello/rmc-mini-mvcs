using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class InstantiatePrefabCriterionTests : CriterionTestBase<InstantiatePrefabCriterion>
    {
        GameObject m_Prefab1;
        GameObject m_Prefab2;
        GameObject m_PrefabWithGrandchild;

        string m_TempFolderPath;

        [OneTimeSetUp]
        public void CreatePrefabAssets()
        {
            var tempFolderGUID = AssetDatabase.CreateFolder("Assets", "Temp");
            m_TempFolderPath = AssetDatabase.GUIDToAssetPath(tempFolderGUID);

            var prefab1Path = m_TempFolderPath + "/Prefab1.prefab";
            m_Prefab1 = PrefabUtility.SaveAsPrefabAsset(new GameObject("Prefab1"), prefab1Path);

            var prefab2Path = m_TempFolderPath + "/Prefab2.prefab";
            m_Prefab2 = PrefabUtility.SaveAsPrefabAsset(new GameObject("Prefab2"), prefab2Path);

            var root = new GameObject("Root");
            var child = new GameObject("Child");
            var grandchild = new GameObject("Grandchild");
            child.transform.parent = root.transform;
            grandchild.transform.parent = child.transform;
            var prefabWithGrandchildPath = m_TempFolderPath + "/PrefabWithGrandchild.prefab";
            m_PrefabWithGrandchild = PrefabUtility.SaveAsPrefabAsset(root, prefabWithGrandchildPath);
        }

        [OneTimeTearDown]
        public void DestroyPrefabAssets()
        {
            AssetDatabase.DeleteAsset(m_TempFolderPath);
        }

        [SetUp]
        public void OpenScene()
        {
            EditorSceneManager.OpenScene(GetTestAssetPath("EmptyTestScene.unity"));
        }

        // TODO Pretty much all of these tets crash 2019.1
#if !UNITY_2019_1_OR_NEWER
        [Test]
        public void WhenPrefabParentIsNull_IsNotCompleted()
        {
            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenPrefabParentIsInstantiated_IsCompleted()
        {
            m_Criterion.prefabParent = m_Prefab1;

            Selection.activeObject = PrefabUtility.InstantiatePrefab(m_Prefab1);
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenDifferentPrefabIsInstantiated_IsNotCompleted()
        {
            m_Criterion.prefabParent = m_Prefab1;

            Selection.activeObject = PrefabUtility.InstantiatePrefab(m_Prefab2);
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenPrefabInstanceExistsBeforePrefabParentIsSet_IsNotCompleted()
        {
            var prefabInstance = PrefabUtility.InstantiatePrefab(m_Prefab1);
            Selection.activeObject = prefabInstance;
            yield return null;
            Selection.activeObject = null;
            yield return null;

            m_Criterion.prefabParent = m_Prefab1;
            Selection.activeObject = prefabInstance;
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenPrefabInstanceExistsBeforeTesting_IsNotCompleted()
        {
            m_Criterion.StopTesting();
            m_Criterion.prefabParent = m_Prefab1;
            var prefabInstance = PrefabUtility.InstantiatePrefab(m_Prefab1);
            Selection.activeObject = prefabInstance;
            yield return null;
            Selection.activeObject = null;
            yield return null;
            m_Criterion.StartTesting();

            Selection.activeObject = prefabInstance;
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenPrefabInstanceIsDestroyed_IsNotCompleted()
        {
            var tempFolderGUID = AssetDatabase.CreateFolder("Assets", "Temp");
            var tempFolderPath = AssetDatabase.GUIDToAssetPath(tempFolderGUID);
            var prefabPath = tempFolderPath + "/Prefab.prefab";
            var prefab = PrefabUtility.SaveAsPrefabAsset(new GameObject("Prefab"), prefabPath);

            m_Criterion.prefabParent = prefab;

            var prefabInstance = PrefabUtility.InstantiatePrefab(prefab);
            Selection.activeObject = prefabInstance;
            yield return null;

            Assert.IsTrue(m_Criterion.completed);

            UnityObject.DestroyImmediate(prefabInstance);
            yield return null;

            Assert.IsFalse(m_Criterion.completed);

            AssetDatabase.DeleteAsset(tempFolderPath);
        }

        [Test]
        public void SetFuturePrefabInstances_FutureReferencesAreCreated()
        {
            m_Criterion.prefabParent = m_Prefab1;
            var prefabParents = new UnityObject[] { m_Prefab1, m_Prefab1.GetComponent<Transform>() };

            m_Criterion.SetFuturePrefabInstances(prefabParents);

            Assert.AreEqual(2, futureReferences.Count());
            CollectionAssert.AllItemsAreNotNull(futureReferences);
            CollectionAssert.AreEquivalent(new[] { "1: Prefab1 (GameObject)", "2: Prefab1 (Transform)" },
                futureReferences.Select(f => f.referenceName));
        }

        [UnityTest]
        public IEnumerator OnCompletion_FutureReferencesAreUpdatedToPrefabInstances()
        {
            m_Criterion.prefabParent = m_PrefabWithGrandchild;
            var prefabParents = new UnityObject[]
            {
                m_PrefabWithGrandchild,
                m_PrefabWithGrandchild.transform,
                m_PrefabWithGrandchild.transform.GetChild(0).gameObject,
                m_PrefabWithGrandchild.transform.GetChild(0),
                m_PrefabWithGrandchild.transform.GetChild(0).GetChild(0).gameObject,
                m_PrefabWithGrandchild.transform.GetChild(0).GetChild(0),
            };
            m_Criterion.SetFuturePrefabInstances(prefabParents);

            var prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(m_PrefabWithGrandchild);
            Selection.activeObject = prefabInstance;
            yield return null;

            CollectionAssert.AreEqual(new[]
            {
                futureReferences.First(f => f.referenceName == "1: PrefabWithGrandchild (GameObject)"),
                futureReferences.First(f => f.referenceName == "2: PrefabWithGrandchild (Transform)"),
                futureReferences.First(f => f.referenceName == "3: Child (GameObject)"),
                futureReferences.First(f => f.referenceName == "4: Child (Transform)"),
                futureReferences.First(f => f.referenceName == "5: Grandchild (GameObject)"),
                futureReferences.First(f => f.referenceName == "6: Grandchild (Transform)"),
            }.Select(f => f.sceneObjectReference.ReferencedObject), new UnityObject[]
                {
                    prefabInstance,
                    prefabInstance.transform,
                    prefabInstance.transform.GetChild(0).gameObject,
                    prefabInstance.transform.GetChild(0),
                    prefabInstance.transform.GetChild(0).GetChild(0).gameObject,
                    prefabInstance.transform.GetChild(0).GetChild(0),
                });
        }

        [UnityTest]
        public IEnumerator AutoComplete_WhenPrefabParentIsNull_ReturnsFalseAndIsNotCompleted()
        {
            Assert.IsFalse(m_Criterion.AutoComplete());
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator AutoComplete_WhenPrefabParentIsNotNull_ReturnsTrueAndIsCompleted()
        {
            m_Criterion.prefabParent = m_Prefab1;

            Assert.IsTrue(m_Criterion.AutoComplete());
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

#endif
    }
}
