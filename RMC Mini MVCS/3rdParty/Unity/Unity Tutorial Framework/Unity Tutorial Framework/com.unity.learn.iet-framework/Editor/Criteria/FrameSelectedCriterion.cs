using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that a specific object is framed in the Scene view.
    /// </summary>
    public class FrameSelectedCriterion : Criterion
    {
        [Serializable]
        class ObjectReferenceCollection : CollectionWrapper<ObjectReference>
        {
        }

        [SerializeField]
        ObjectReferenceCollection m_ObjectReferences = new ObjectReferenceCollection();

        /// <summary>
        /// Sets object references.
        /// </summary>
        /// <param name="objectReferences"></param>
        public void SetObjectReferences(IEnumerable<ObjectReference> objectReferences)
        {
            m_ObjectReferences.SetItems(objectReferences);
            UpdateCompletion();
        }

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public override void StartTesting()
        {
            base.StartTesting();
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
        }

        /// <summary>
        /// Runs update logic for the criterion.
        /// </summary>
        /// <remarks>
        /// Overriding the update completion state, as this criterion is not state based
        /// </remarks>
        public override void UpdateCompletion()
        {
        }

        void OnSceneGUI(SceneView sceneView)
        {
            var evt = Event.current;
            if (evt.type == EventType.ExecuteCommand && evt.commandName == "FrameSelected")
            {
                if (!m_ObjectReferences.Any())
                    return;

                if (m_ObjectReferences.Count() != Selection.objects.Length)
                    return;

                foreach (var objectReference in m_ObjectReferences)
                {
                    var referencedObject = objectReference.SceneObjectReference.ReferencedObject;
                    if (referencedObject == null)
                        return;

                    if (!Selection.objects.Contains(referencedObject))
                        return;
                }

                IsCompleted = true;
            }
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            if (!m_ObjectReferences.Any())
            {
                Debug.LogWarning("Cannot auto-complete FrameSelectedCriterion with no object references");
                return false;
            }

            var referencedObjects = m_ObjectReferences.Select(or => or.SceneObjectReference.ReferencedObject);
            if (referencedObjects.Any(obj => obj == null))
            {
                Debug.LogWarning("Cannot auto-complete FrameSelectedCriterion with unresolved object references");
                return false;
            }

            if (SceneView.lastActiveSceneView == null)
            {
                Debug.LogWarning("Cannot auto-complete FrameSelectedCriterion when there is no last active Scene View");
                return false;
            }

            var previousActiveObject = Selection.activeObject;
            var previousSelection = Selection.objects.ToArray();

            Selection.objects = referencedObjects.ToArray();

            if (!SceneView.FrameLastActiveSceneView())
            {
                Debug.LogWarning("Cannot auto-complete FrameSelectedCriterion since framing last active Scene View failed");

                // Restore selection
                Selection.activeObject = previousActiveObject;
                Selection.objects = previousSelection;

                return false;
            }

            return true;
        }
    }
}
