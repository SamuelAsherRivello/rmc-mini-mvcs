using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class FrameSelectedCriterionTests : CriterionTestBase<FrameSelectedCriterion>
    {
        GameObject m_ReferencedObject;
        ObjectReference m_ObjectReference;

        [SetUp]
        public void OpenSceneAndSetUpCriterion()
        {
            EditorSceneManager.OpenScene(GetTestAssetPath("EmptyTestScene.unity"));

            m_ReferencedObject = new GameObject();

            m_ObjectReference = new ObjectReference();
            m_ObjectReference.SceneObjectReference.Update(m_ReferencedObject);

            m_Criterion.SetObjectReferences(new[] { m_ObjectReference });

            // Ensure we have a last active scene view
            EditorWindow.GetWindow<SceneView>();
            // TODO: Might need to interact with it here to have it set
        }

        // TODO Pretty much all of these tets crash 2019.1
#if !UNITY_2019_1_OR_NEWER
        [UnityTest]
        public IEnumerator IsNotCompletedInitially()
        {
            Assert.That(m_Criterion.completed, Is.False);
            yield return null;

            Assert.That(m_Criterion.completed, Is.False);
        }

        [UnityTest]
        public IEnumerator WhenSelectionMatchesObjectReferences_IsNotCompleted()
        {
            Selection.objects = new Object[] { m_ReferencedObject };
            yield return null;

            Assert.That(m_Criterion.completed, Is.False);
        }

        [UnityTest]
        public IEnumerator WhenFrameLastActiveSceneViewIsCalled_AndEmptySelectionMatchesObjectReferences_IsNotCompleted()
        {
            m_Criterion.SetObjectReferences(Enumerable.Empty<ObjectReference>());
            Selection.objects = new Object[0];

            SceneView.FrameLastActiveSceneView();
            yield return null;

            Assert.That(m_Criterion.completed, Is.False);
        }

        [UnityTest]
        public IEnumerator WhenFrameLastActiveSceneViewIsCalled_AndSelectionDoesNotMatchObjectReferences_IsNotCompleted()
        {
            var gameObject = new GameObject();
            Selection.objects = new Object[] { gameObject };

            SceneView.FrameLastActiveSceneView();
            yield return null;

            Assert.That(m_Criterion.completed, Is.False);
        }

        [UnityTest]
        public IEnumerator WhenFrameLastActiveSceneViewIsCalled_AndSelectionMatchesObjectReferences_IsCompleted()
        {
            Selection.objects = new Object[] { m_ReferencedObject };

            SceneView.FrameLastActiveSceneView();
            yield return null;

            Assert.That(m_Criterion.completed, Is.True);
        }

        public IEnumerator AutoComplete_WhenObjectReferencesIsEmpty_ReturnsFalseAndIsNotCompleted()
        {
            m_Criterion.SetObjectReferences(Enumerable.Empty<ObjectReference>());

            Assert.That(m_Criterion.AutoComplete(), Is.False);
            yield return null;

            Assert.That(m_Criterion.completed, Is.False);
        }

        [UnityTest]
        public IEnumerator AutoComplete_WhenReferencedObjectIsNull_ReturnsFalseAndIsNotCompleted()
        {
            var nullObjectReference = new ObjectReference();
            nullObjectReference.sceneObjectReference.Update(null);
            m_Criterion.SetObjectReferences(new[] { nullObjectReference });

            Assert.That(m_Criterion.AutoComplete(), Is.False);
            yield return null;

            Assert.That(m_Criterion.completed, Is.False);
        }

        [UnityTest]
        public IEnumerator AutoComplete_WhenThereIsNoLastActiveSceneView_ReturnsFalseAndIsNotCompleted()
        {
            // Close all Scene Views
            var sceneViews = SceneView.sceneViews.ToArray().Cast<SceneView>();
            foreach (var sceneView in sceneViews)
            {
                sceneView.Close();
            }

            Assert.That(m_Criterion.AutoComplete(), Is.False);
            yield return null;

            Assert.That(m_Criterion.completed, Is.False);
        }

        public IEnumerator AutoComplete_WhenReferencedObjectIsGameObject_ReturnsTrueAndIsCompleted()
        {
            Assert.That(m_Criterion.AutoComplete(), Is.True);
            yield return null;

            Assert.That(m_Criterion.completed, Is.True);
        }

#endif
    }
}
