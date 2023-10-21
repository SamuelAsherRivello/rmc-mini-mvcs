using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that specific objects are selected.
    /// </summary>
    public class RequiredSelectionCriterion : Criterion
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
            UpdateCompletion();
            Selection.selectionChanged += UpdateCompletion;
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
            Selection.selectionChanged -= UpdateCompletion;
        }

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            if (m_ObjectReferences.Count() != Selection.objects.Length)
                return false;

            foreach (var objectReference in m_ObjectReferences)
            {
                var referencedObject = objectReference.SceneObjectReference.ReferencedObject;
                if (referencedObject == null)
                    return false;

                if (!Selection.objects.Contains(referencedObject))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            var referencedObjects = m_ObjectReferences.Select(or => or.SceneObjectReference.ReferencedObject);
            if (referencedObjects.Any(obj => obj == null))
            {
                Debug.LogWarning("Cannot auto-complete RequiredSelectionCriterion with unresolved object references");
                return false;
            }

            Selection.objects = referencedObjects.ToArray();
            return true;
        }
    }
}
