using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class SceneViewCameraMovedCriterionTests : CriterionTestBase<SceneViewCameraMovedCriterion>
    {
        [SetUp]
        public void Setup()
        {
            //ensure a sceneview for tests, we have a tests that tests what happens when no SceneView is present.
            EditorWindow.GetWindow<SceneView>().Show(true);
        }

        // TODO Pretty much all of these tets crash 2019.1
#if !UNITY_2019_1_OR_NEWER
        [UnityTest]
        public IEnumerator CameraDoesNotMove_IsNotComplete()
        {
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenCameraViewChangesPosition_IsComplete()
        {
            var position = SceneView.lastActiveSceneView.camera.transform.position;
            SceneView.lastActiveSceneView.camera.transform.position = position + Vector3.down;
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator WhenCameraViewChangesOrientation_IsComplete()
        {
            var localRotation = SceneView.lastActiveSceneView.camera.transform.localRotation;
            SceneView.lastActiveSceneView.camera.transform.localRotation = localRotation * Quaternion.Euler(0, 15, 0);
            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator AutoComplete_ReturnsTrue_IsComplete()
        {
            Assert.IsTrue(m_Criterion.AutoComplete());

            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator NoLastActiveSceneView_ThenSceneViewIsActivated_ShouldNotComplete()
        {
            m_Criterion.StopTesting();
            var sceneViews = Resources.FindObjectsOfTypeAll<SceneView>();
            foreach (var sceneView in sceneViews)
            {
                sceneView.Close();
            }

            m_Criterion.StartTesting();
            yield return null;

            Assert.IsFalse(m_Criterion.completed);

            EditorWindow.GetWindow<SceneView>().Show(true);
            yield return null;

            Assert.IsFalse(m_Criterion.completed);
        }

        [UnityTest]
        public IEnumerator NoLastActiveSceneView_ThenSceneViewIsActivated_ThenUserMovesCamera_IsComplete()
        {
            m_Criterion.StopTesting();
            var sceneViews = Resources.FindObjectsOfTypeAll<SceneView>();
            foreach (var sceneView in sceneViews)
            {
                sceneView.Close();
            }

            m_Criterion.StartTesting();
            EditorWindow.GetWindow<SceneView>().Show(true);

            yield return null;

            var position = SceneView.lastActiveSceneView.camera.transform.position;
            SceneView.lastActiveSceneView.camera.transform.position = position + Vector3.down;

            yield return null;

            Assert.IsTrue(m_Criterion.completed);
        }

#endif
    }
}
