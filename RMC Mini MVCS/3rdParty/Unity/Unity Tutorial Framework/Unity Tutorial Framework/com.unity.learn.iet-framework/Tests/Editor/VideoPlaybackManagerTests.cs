using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class VideoPlaybackManagerTests : TestBase
    {
        VideoPlaybackManager m_VideoPlaybackManager;
        VideoClip m_VideoClip1;
        VideoClip m_VideoClip2;

        [SetUp]
        public void SetUp()
        {
            // NOTE If opening a new scene, temporary render textures won't get disposed properly.
            //EditorSceneManager.OpenScene(GetTestAssetPath("EmptyTestScene.unity"));

            m_VideoPlaybackManager = new VideoPlaybackManager();
            m_VideoClip1 = AssetDatabase.LoadAssetAtPath<VideoClip>(GetTestAssetPath("TestVideoClip1.webm"));
            m_VideoClip2 = AssetDatabase.LoadAssetAtPath<VideoClip>(GetTestAssetPath("TestVideoClip2.webm"));
        }

        [TearDown]
        public void TearDown()
        {
            m_VideoPlaybackManager.OnDisable();
        }

        [Test]
        public void GetTextureForVideoClip_BeforeOnEnableIsCalled_ThrowsNullReferenceException()
        {
            Assert.That(() => m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1), Throws.Exception);
        }

        [Test]
        public void GetTextureForVideoClip_AfterOnEnableIsCalled_ReturnsValidTexture()
        {
            m_VideoPlaybackManager.OnEnable();

            var texture = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);

            Assert.That(texture != null, Is.True);
        }

        [Test]
        public void GetTextureForVideoClip_AfterOnDisableIsCalled_ThrowsNullReferenceException()
        {
            m_VideoPlaybackManager.OnEnable();
            m_VideoPlaybackManager.OnDisable();

            Assert.That(() => m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1), Throws.Exception);
        }

        [Test]
        public void GetTextureForVideoClip_ReturnsSameTextureForSameVideoClip()
        {
            m_VideoPlaybackManager.OnEnable();

            var texture1 = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);
            var texture2 = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);

            Assert.That(texture1 == texture2, Is.True);
        }

        [Test]
        public void GetTextureForVideoClip_ReturnsDifferentTextureForDifferentVideoClip()
        {
            m_VideoPlaybackManager.OnEnable();

            var texture1 = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);
            var texture2 = m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip2);

            Assert.That(texture1 == texture2, Is.False);
        }

        [Test]
        public void GetTextureForVideoClip_OnDisable_DestroysAllObjectsCreatedByManager()
        {
            var objectsBefore = Resources.FindObjectsOfTypeAll<Object>();
            m_VideoPlaybackManager.OnEnable();
            m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);
            m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip2);
            var newObjects = Resources.FindObjectsOfTypeAll<Object>().Except(objectsBefore);

            m_VideoPlaybackManager.OnDisable();

            foreach (var newObject in newObjects)
            {
                Assert.That(newObject == null, Is.True, "Object not destroyed: " + newObject);
            }
        }

        [Test]
        public void GetTextureForVideoClip_ClearCache_DestroysAllObjectsCreatedByCallsToGetTextureForVideoClip()
        {
            m_VideoPlaybackManager.OnEnable();
            var objectsBefore = Resources.FindObjectsOfTypeAll<Object>();
            m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip1);
            m_VideoPlaybackManager.GetTextureForVideoClip(m_VideoClip2);
            var newObjects = Resources.FindObjectsOfTypeAll<Object>().Except(objectsBefore);

            m_VideoPlaybackManager.ClearCache();

            foreach (var newObject in newObjects)
            {
                Assert.That(newObject == null, Is.True, "Object not destroyed: " + newObject);
            }
        }
    }
}
