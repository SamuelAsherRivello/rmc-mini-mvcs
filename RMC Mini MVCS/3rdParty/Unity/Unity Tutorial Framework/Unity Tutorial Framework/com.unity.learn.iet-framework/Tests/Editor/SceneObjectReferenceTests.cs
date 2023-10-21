using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class SceneObjectReferenceTests
    {
        SceneObjectReference reference;
        private string tempScenePath = "Assets/TempScene.unity";

        [SetUp]
        public void InitManager()
        {
            reference = new SceneObjectReference();
            SaveScene();
        }

        public void SaveScene()
        {
            var scene = EditorSceneManager.GetActiveScene();
            EditorSceneManager.SaveScene(scene, tempScenePath);
        }

        [OneTimeTearDown]
        public void DeleteScene()
        {
            AssetDatabase.DeleteAsset(tempScenePath);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [Test]
        public void Reference_WhenNotSet_IsNull()
        {
            Assert.IsNull(reference.ReferencedObject);
        }

        [Test]
        public void Reference_WhenNoSceneIsSet_IsResolved()
        {
            Assert.IsTrue(reference.ReferenceResolved);
        }

        [Test]
        public void Reference_WhenSceneIsSet_IsNotResolved()
        {
            var go = CreateGameObject();
            reference.Update(go);

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

            Assert.IsFalse(reference.ReferenceResolved);
        }

        [Test]
        public void ReferenceGO_WhenExistOnScene_ReturnsTheObject()
        {
            var go = CreateGameObject();
            reference.Update(go);

            Assert.IsTrue(reference.ReferenceResolved);
            Assert.IsNotNull(reference.ReferencedObject);
            Assert.IsNotNull(reference.ReferencedObjectAsGameObject);
            Assert.IsNull(reference.ReferencedObjectAsComponent);
            Assert.AreEqual(go, reference.ReferencedObject);
            Assert.AreEqual(go, reference.ReferencedObjectAsGameObject);
        }

        [Test]
        public void ReferenceComponent_WhenExistOnScene_ReturnsTheObject()
        {
            var go = CreateGameObject();
            reference.Update(go.transform);

            Assert.IsTrue(reference.ReferenceResolved);
            Assert.IsNotNull(reference.ReferencedObject);
            Assert.IsNotNull(reference.ReferencedObjectAsComponent);
            Assert.IsNull(reference.ReferencedObjectAsGameObject);
            Assert.AreEqual(go.transform, reference.ReferencedObject);
            Assert.AreEqual(go.transform, reference.ReferencedObjectAsComponent);
        }

        [Test]
        public void ReferencePrefab_WhenExistOnScene_ReturnsTheObject()
        {
            var go = CreateGameObject();
            var tempPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder("Assets", "Temp"));
            var testPrefabPath = tempPath + "/TestPrefab.prefab";
            var prefab = PrefabUtility.SaveAsPrefabAsset(go, testPrefabPath);

            reference.Update(prefab);

            Assert.IsTrue(reference.ReferenceResolved);
            Assert.IsTrue(reference.IsPrefabReference);
            Assert.IsNotNull(reference.ReferencedObject);
            Assert.IsNotNull(reference.ReferencedObjectAsGameObject);
            Assert.AreEqual(prefab, reference.ReferencedObject);
            Assert.AreEqual(prefab, reference.ReferencedObjectAsGameObject);

            AssetDatabase.DeleteAsset(tempPath);
        }

        [Test]
        public void ReferencePrefab_WhenReferencingInstance_ReturnsTheInstance()
        {
            var go = CreateGameObject();
            var tempPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder("Assets", "Temp"));
            var testPrefabPath = tempPath + "/TestPrefab.prefab";
            var prefab = PrefabUtility.SaveAsPrefabAsset(go, testPrefabPath);
            var prefabInstance = PrefabUtility.InstantiatePrefab(prefab);

            reference.Update(prefabInstance);

            Assert.IsTrue(reference.ReferenceResolved);
            Assert.IsFalse(reference.IsPrefabReference);
            Assert.IsNotNull(reference.ReferencedObject);
            Assert.IsNotNull(reference.ReferencedObjectAsGameObject);
            Assert.AreEqual(prefabInstance, reference.ReferencedObject);
            Assert.AreEqual(prefabInstance, reference.ReferencedObjectAsGameObject);

            AssetDatabase.DeleteAsset(tempPath);
        }

        [Test]
        public void ReferencePrefab_WhenReferencingPrefabVariant_returnsThePrefabAsset()
        {
            string tempPath = "";
            string testPrefabVariantPath = "";
            try
            {
                var go = CreateGameObject();
                tempPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder("Assets", "Temp"));
                var testPrefabPath = tempPath + "/TestPrefab.prefab";
                var prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(go, testPrefabPath, InteractionMode.AutomatedAction);
                testPrefabVariantPath = tempPath + "/TestPrefabVariant.prefab";
                var prefabInstance = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
                var prefabAssetVariant = PrefabUtility.SaveAsPrefabAsset(prefabInstance, testPrefabVariantPath);

                reference.Update(prefabAssetVariant);

                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsTrue(reference.IsPrefabReference);
                Assert.IsNotNull(reference.ReferencedObject);
                Assert.IsNotNull(reference.ReferencedObjectAsGameObject);
                Assert.AreEqual(prefabAssetVariant, reference.ReferencedObject);
                Assert.AreEqual(prefabAssetVariant, reference.ReferencedObjectAsGameObject);
            }
            finally
            {
                AssetDatabase.DeleteAsset(tempPath);
                AssetDatabase.DeleteAsset(testPrefabVariantPath);
            }
        }

        [Test]
        public void Reference_WhenNotExistOnScene_ReturnsNull()
        {
            var go = CreateGameObject();
            reference.Update(go);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

            Assert.IsFalse(reference.ReferenceResolved);
            Assert.IsNull(reference.ReferencedObject);
        }

        [UnityTest]
        public IEnumerator Reference_WhenInvalidSceneIsLoaded_IsNotResolved()
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            var go = CreateGameObject();
            var scenePath = "Assets/TestScene.unity";
            EditorSceneManager.SaveScene(newScene, scenePath, false);
            reference.Update(go);
            EditorSceneManager.SaveScene(newScene);

            try
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                yield return null;
                Assert.IsFalse(reference.ReferenceResolved);
                Assert.IsNull(reference.ReferencedObject);
                Assert.IsNotNull(reference.ReferenceScene);

                var sceneAssetPath = AssetDatabase.GetAssetOrScenePath(reference.ReferenceScene);
                EditorSceneManager.OpenScene(sceneAssetPath);
                yield return null;
                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsNotNull(reference.ReferencedObject);
                Assert.IsNotNull(reference.ReferencedObject);
            }
            finally
            {
                AssetDatabase.DeleteAsset(scenePath);
            }
        }

        [Test]
        public void Reference_WhenChanged_WillReturnNewState()
        {
            var go = CreateGameObject();
            var goPrefab = CreateGameObject();
            var testPrefabPath = "Assets/TestPrefab.prefab";
            var prefab = PrefabUtility.SaveAsPrefabAsset(goPrefab, testPrefabPath);
            var prefabInstance = PrefabUtility.InstantiatePrefab(prefab);
            var component = go.transform;
            var so = ScriptableObject.CreateInstance<TestAsset>();
            var assetPath = "Assets/TestAsset.asset";
            AssetDatabase.CreateAsset(so, assetPath);
            var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(TestAsset));

            try
            {
                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsFalse(reference.IsAssetReference);
                Assert.IsFalse(reference.IsComponentReference);
                Assert.IsFalse(reference.IsGameObjectReference);
                Assert.IsFalse(reference.IsPrefabReference);

                reference.Update(go);

                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsFalse(reference.IsAssetReference);
                Assert.IsFalse(reference.IsComponentReference);
                Assert.IsTrue(reference.IsGameObjectReference);
                Assert.IsFalse(reference.IsPrefabReference);

                reference.Update(component);

                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsFalse(reference.IsAssetReference);
                Assert.IsTrue(reference.IsComponentReference);
                Assert.IsFalse(reference.IsGameObjectReference);
                Assert.IsFalse(reference.IsPrefabReference);

                reference.Update(asset);

                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsTrue(reference.IsAssetReference);
                Assert.IsFalse(reference.IsComponentReference);
                Assert.IsFalse(reference.IsGameObjectReference);
                Assert.IsFalse(reference.IsPrefabReference);

                reference.Update(prefab);

                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsFalse(reference.IsAssetReference);
                Assert.IsFalse(reference.IsComponentReference);
                Assert.IsFalse(reference.IsGameObjectReference);
                Assert.IsTrue(reference.IsPrefabReference);

                reference.Update(prefabInstance);

                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsFalse(reference.IsAssetReference);
                Assert.IsFalse(reference.IsComponentReference);
                Assert.IsTrue(reference.IsGameObjectReference);
                Assert.IsFalse(reference.IsPrefabReference);

                reference.Update(component);

                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsFalse(reference.IsAssetReference);
                Assert.IsTrue(reference.IsComponentReference);
                Assert.IsFalse(reference.IsGameObjectReference);
                Assert.IsFalse(reference.IsPrefabReference);

                reference.Update(null);

                Assert.IsTrue(reference.ReferenceResolved);
                Assert.IsFalse(reference.IsAssetReference);
                Assert.IsFalse(reference.IsComponentReference);
                Assert.IsFalse(reference.IsGameObjectReference);
                Assert.IsFalse(reference.IsPrefabReference);
            }
            finally
            {
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.DeleteAsset(testPrefabPath);
            }
        }

        private static GameObject CreateGameObject()
        {
            var go = new GameObject();
            Undo.RegisterCreatedObjectUndo(go, "Test GO created");
            return go;
        }
    }
}
