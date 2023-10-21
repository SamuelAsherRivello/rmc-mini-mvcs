using NUnit.Framework;
using UnityEngine;
using UnityEditor;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class CommonTutorialCallbacksTests
    {
        CommonTutorialCallbacks m_Callbacks;
        GameObject m_GameObject;
        const string m_GameObjectName = "Try to find me!";

        [SetUp]
        public void SetUp()
        {
            m_Callbacks = ScriptableObject.CreateInstance<CommonTutorialCallbacks>();
            m_GameObject = new GameObject(m_GameObjectName);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(m_Callbacks);
            Object.DestroyImmediate(m_GameObject);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetAudioMasterMute(bool mute)
        {
            m_Callbacks.SetAudioMasterMute(mute);
            Assert.AreEqual(EditorUtility.audioMasterMute, mute);
        }

        // NOTE Confirming that the asset was actually pinged is not trivial
        // (it's not added to Selection, for example) so we settle for testing
        // that all kind of input is handled gracefully.
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("       ")]
        [TestCase("Assets/DoesNotExist.asset")]
        [TestCase("Packages/com.unity.learn.iet-framework.authoring/Readme.md")]
        public void PingFolderOrAsset_DoesNotThrowOnAnyInput(string path)
        {
            m_Callbacks.PingFolderOrAsset(path);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("       ")]
        [TestCase("Assets/DoesNotExist.asset")]
        [TestCase("Packages/com.unity.learn.iet-framework.authoring/Readme.md")]
        public void PingFolderOrFirstAsset_DoesNotThrowOnAnyInput(string path)
        {
            m_Callbacks.PingFolderOrFirstAsset(path);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("       ")]
        [TestCase("DoesNotExist")]
        public void SelectGameObject_DoesNotThrowOnAnyInput(string path)
        {
            m_Callbacks.SelectGameObject(path);
        }

        [Test]
        public void SelectGameObject_Succeeds()
        {
            m_Callbacks.SelectGameObject(m_GameObjectName);
            Assert.AreEqual(Selection.activeGameObject, m_GameObject);
        }

        [Test]
        public void SelectGameObject_KeepsSelectionIntactIfGameObjectNotFound()
        {
            Selection.activeGameObject = m_GameObject;
            m_Callbacks.SelectGameObject("does not exist");
            Assert.AreEqual(Selection.activeGameObject, m_GameObject);
        }
    }
}
