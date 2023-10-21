using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class SceneAddedToBuildCriterionTests : CriterionTestBase<SceneAddedToBuildCriterion>
    {
        const string m_TestSceneName = "EmptyTestScene";

        // TODO Pretty much all of these tets crash 2019.1
#if !UNITY_2019_1_OR_NEWER
        [SetUp]
        public void SetUp()
        {
            var scene = GetSceneAsset(m_TestSceneName);
            RemoveSceneFromBuildSettings(scene);
        }

        [UnityTest]
        public IEnumerator SceneIsNotNull_WasAddedToBuild_IsCompleted()
        {
            m_Criterion.scene = GetSceneAsset(m_TestSceneName);

            Assert.IsFalse(SceneAddedToBuildCriterion.SceneIsAddedToBuildSettings(m_Criterion.scene));
            yield return null;

            SceneAddedToBuildCriterion.AddSceneToBuildSettings(m_Criterion.scene);

            Assert.IsTrue(SceneAddedToBuildCriterion.SceneIsAddedToBuildSettings(m_Criterion.scene));

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator SceneIsNotNull_WasAddedToBuild_IsDisabled_IsNotCompleted()
        {
            m_Criterion.scene = GetSceneAsset(m_TestSceneName);

            yield return null;

            SceneAddedToBuildCriterion.AddSceneToBuildSettings(m_Criterion.scene, false);

            Assert.IsTrue(SceneAddedToBuildCriterion.SceneIsAddedToBuildSettings(m_Criterion.scene));

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator SceneIsNotNull_WasNotAddedToBuild_IsNotCompleted()
        {
            m_Criterion.scene = GetSceneAsset(m_TestSceneName);

            yield return null;

            Assert.IsFalse(SceneAddedToBuildCriterion.SceneIsAddedToBuildSettings(m_Criterion.scene));

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator SceneIsNull_IsNotCompleted()
        {
            Assert.IsFalse(SceneAddedToBuildCriterion.SceneIsAddedToBuildSettings(m_Criterion.scene));
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator Autocomplete_SceneIsNotNull_IsCompleted()
        {
            m_Criterion.scene = GetSceneAsset(m_TestSceneName);
            Assert.IsTrue(m_Criterion.AutoComplete());
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator Autocomplete_SceneIsNull_IsNotCompleted()
        {
            Assert.IsFalse(m_Criterion.AutoComplete());
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        static SceneAsset GetSceneAsset(string name)
        {
            var guid = AssetDatabase.FindAssets(m_TestSceneName + " t:scene")[0];
            var sceneAssetPath = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneAssetPath);
        }

        public static void RemoveSceneFromBuildSettings(SceneAsset scene)
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(scene)))
            {
                return;
            }

            if (!SceneAddedToBuildCriterion.SceneIsAddedToBuildSettings(scene))
            {
                return;
            }

            var sceneAsset = new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(scene), true);
            var scenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length - 1];
            var j = 0;
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
                if (EditorBuildSettings.scenes[i].guid != sceneAsset.guid)
                {
                    scenes[j++] = EditorBuildSettings.scenes[i];
                }
            }
            EditorBuildSettings.scenes = scenes;
        }

#endif
    }
}
