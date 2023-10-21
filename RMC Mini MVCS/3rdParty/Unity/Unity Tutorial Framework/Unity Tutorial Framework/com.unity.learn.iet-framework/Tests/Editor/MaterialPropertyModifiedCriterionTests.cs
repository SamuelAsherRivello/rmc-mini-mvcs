using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using Object = System.Object;
using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class MaterialPropertyModifiedCriterionTests : CriterionTestBase<MaterialPropertyModifiedCriterion>
    {
        Material m_Material = null;
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.OpenScene(GetTestAssetPath("EmptyTestScene.unity"));
            SaveActiveScene();

            m_Material = new Material(Shader.Find("Standard"));
        }

        [TearDown]
        public void Teardown()
        {
            UnityObject.DestroyImmediate(m_Material);
        }

        // TODO Pretty much all of these tets crash 2019.1
#if !UNITY_2019_1_OR_NEWER
        [UnityTest]
        public IEnumerator WhenPropertyPathIsEmpty_IsNotCompleted()
        {
            m_Criterion.target.Update(m_Material);
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenTargetIsEmpty_IsNotCompleted()
        {
            m_Criterion.target = null;
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        MaterialProperty FindProperty(string path, Material material)
        {
            UnityObject[] mats = new[] { material };
            return MaterialEditor.GetMaterialProperty(mats, path);
        }

        [UnityTest]
        public IEnumerator WhenDifferentPropertyOnTheSameTargetIsModified_IsNotCompleted()
        {
            m_Criterion.target.Update(m_Material);
            m_Criterion.materialPropertyPath = "_Color";
            var matallicProperty = FindProperty("_Metallic", m_Material);
            matallicProperty.floatValue = 0.5f;
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenSamePropertyOnDifferentTargetIsModified_IsNotCompleted()
        {
            m_Criterion.target.Update(m_Material);
            m_Criterion.materialPropertyPath = "_Color";
            var otherMaterial = new Material(Shader.Find("Standard"));
            var colorProperty = FindProperty("_Color", otherMaterial);
            colorProperty.colorValue = Color.red;
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenSamePropertyOnTargetIsModified_IsCompleted()
        {
            m_Criterion.target.Update(m_Material);
            m_Criterion.materialPropertyPath = "_Color";
            m_Criterion.StopTesting();
            m_Criterion.StartTesting();
            var colorProperty = FindProperty("_Color", m_Material);
            colorProperty.colorValue = Color.red;
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

#endif
    }
}
