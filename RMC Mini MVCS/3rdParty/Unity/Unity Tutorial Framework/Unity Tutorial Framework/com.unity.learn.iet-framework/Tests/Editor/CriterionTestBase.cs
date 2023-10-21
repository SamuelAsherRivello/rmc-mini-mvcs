using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class CriterionTestBase<T> : TestBase where T : Criterion
    {
        const string k_TempScenePath = "Assets/TempScene.unity";
        const string k_TempTutorialPagePath = "Assets/TempTutorialPage.asset";

        TutorialPage m_TutorialPage;
        protected T m_Criterion;

        [SetUp]
        public void SetUpTutorialPageAndCriterion()
        {
            // Criterion is assumed to be subasset of TutorialPage
            m_TutorialPage = ScriptableObject.CreateInstance<TutorialPage>();
            AssetDatabase.CreateAsset(m_TutorialPage, k_TempTutorialPagePath);

            m_Criterion = ScriptableObject.CreateInstance<T>();
            AssetDatabase.AddObjectToAsset(m_Criterion, k_TempTutorialPagePath);

            m_Criterion.StartTesting();
        }

        [TearDown]
        public virtual void TearDown()
        {
            m_Criterion.StopTesting();

            AssetDatabase.DeleteAsset(k_TempTutorialPagePath);
        }

        [OneTimeTearDown]
        public void DeleteTempScene()
        {
            if (AssetDatabase.DeleteAsset(k_TempScenePath))
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        public void SaveActiveScene()
        {
            var scene = EditorSceneManager.GetActiveScene();
            EditorSceneManager.SaveScene(scene, k_TempScenePath);
        }

        protected IEnumerable<FutureObjectReference> futureReferences
        {
            get
            {
                return AssetDatabase.LoadAllAssetsAtPath(k_TempTutorialPagePath)
                    .Where(a => a is FutureObjectReference)
                    .Cast<FutureObjectReference>()
                    .Where(f => f.Criterion == m_Criterion);
            }
        }

        protected void TriggerValidation()
        {
            // Ensure OnValidate is invoked
            var serializedObject = new SerializedObject(m_Criterion);
            var enabledProperty = serializedObject.FindProperty("m_Enabled");
            var enabled = enabledProperty.boolValue;
            enabledProperty.boolValue = !enabled;
            serializedObject.ApplyModifiedProperties();
            enabledProperty.boolValue = enabled;
            serializedObject.ApplyModifiedProperties();
        }

        protected IEnumerable TriggerSelectionChanged()
        {
            var activeObject = Selection.activeObject;
            var objects = Selection.objects;

            if (objects.Length == 0)
            {
                Selection.activeObject = m_Criterion;
                yield return null;
            }
            else
            {
                Selection.activeObject = null;
                yield return null;
            }

            Selection.activeObject = activeObject;
            if (objects.Length > 1)
                Selection.objects = objects;
            yield return null;
        }
    }
}
