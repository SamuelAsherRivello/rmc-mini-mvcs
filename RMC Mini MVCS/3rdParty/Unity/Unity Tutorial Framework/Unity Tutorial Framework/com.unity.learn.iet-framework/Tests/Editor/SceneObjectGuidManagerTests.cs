using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class SceneObjectGuidManagerTests
    {
        SceneObjectGuidManager m_Manager;

        [SetUp]
        public void InitManager()
        {
            m_Manager = SceneObjectGuidManager.Instance;
        }

        [Test]
        public void Manager_WithNoRegisteredComponents_WontReturnAnyComponents()
        {
            Assert.IsFalse(m_Manager.Contains("non_existing_id"));
        }

        [Test]
        public void Manager_WithOneRegisteredComponent_ReturnTheComponent()
        {
            var c = CreateGameObjectWithSceneObjectGuid();

            Assert.IsTrue(m_Manager.Contains(c.Id));
            Assert.IsNotNull(m_Manager.GetComponent(c.Id));
        }

        [Test]
        public void Manager_WithManyComponentsRegistered_WillReturnTheCorrectOnes()
        {
            var c1 = CreateGameObjectWithSceneObjectGuid();
            var c2 = CreateGameObjectWithSceneObjectGuid();
            var c3 = CreateGameObjectWithSceneObjectGuid();

            Assert.IsTrue(m_Manager.Contains(c2.Id));
            Assert.AreEqual(c2, m_Manager.GetComponent(c2.Id));
            Assert.AreNotEqual(c1, m_Manager.GetComponent(c2.Id));
            Assert.AreNotEqual(c3, m_Manager.GetComponent(c2.Id));
        }

        [Test]
        public void Manager_WithManyComponentsRegistered_WillReturnNullForNotExisting()
        {
            CreateGameObjectWithSceneObjectGuid();
            CreateGameObjectWithSceneObjectGuid();
            CreateGameObjectWithSceneObjectGuid();

            Assert.IsNull(m_Manager.GetComponent("Not_Existing_id"));
        }

        [Test]
        public void Manager_WithComponentAddedAndRemoved_WillReturnNull()
        {
            var c = CreateGameObjectWithSceneObjectGuid();
            var id = c.Id;
            Object.DestroyImmediate(c);

            Assert.IsNull(m_Manager.GetComponent(id));
        }

        [Test]
        public void Manager_WithManyComponentsAddedAndOneRemoved_WillOnlyReturnTheExisting()
        {
            var c1 = CreateGameObjectWithSceneObjectGuid();
            var c2 = CreateGameObjectWithSceneObjectGuid();
            var c2Id = c2.Id;
            Object.DestroyImmediate(c2);
            var c3 = CreateGameObjectWithSceneObjectGuid();

            Assert.IsNotNull(m_Manager.GetComponent(c1.Id));
            Assert.IsNotNull(m_Manager.GetComponent(c3.Id));
            Assert.IsNull(m_Manager.GetComponent(c2Id));
        }

        static SceneObjectGuid CreateGameObjectWithSceneObjectGuid()
        {
            var go = new GameObject();
            Undo.RegisterCreatedObjectUndo(go, "Created test GO");
            return go.AddComponent<SceneObjectGuid>();
        }
    }
}
