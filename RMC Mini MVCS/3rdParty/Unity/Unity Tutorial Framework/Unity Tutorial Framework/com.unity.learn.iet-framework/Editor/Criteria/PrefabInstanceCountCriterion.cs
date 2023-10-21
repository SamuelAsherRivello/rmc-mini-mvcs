using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that a specific amount of prefab instances are created.
    /// </summary>
    public class PrefabInstanceCountCriterion : Criterion
    {
        /// <summary>
        /// Different comparison modes.
        /// </summary>
        public enum InstanceCountComparison
        {
            /// <summary>
            /// At least X amount of instances required.
            /// </summary>
            AtLeast,
            /// <summary>
            /// Exactly X amount of instances required.
            /// </summary>
            Exactly,
            /// <summary>
            /// No more than X amount of instances required.
            /// </summary>
            NoMoreThan,
        }

        /// <summary>
        /// The prefab of instances we want to count.
        /// </summary>
        [FormerlySerializedAs("prefabParent")]
        public GameObject PrefabParent;

        /// <summary>
        /// The wanted comparison mode.
        /// </summary>
        [FormerlySerializedAs("comparisonMode")]
        public InstanceCountComparison ComparisonMode = InstanceCountComparison.AtLeast;

        /// <summary>
        /// The desired amount of instances.
        /// </summary>
        [Range(0, 100)]
        [FormerlySerializedAs("instanceCount")]
        public int InstanceCount = 1;

        [SerializeField, HideInInspector]
        FutureObjectReference m_FutureReference;

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
            if (PrefabParent == null)
                return false;

            var matches = FindObjectsOfType<GameObject>().Where(go => PrefabUtilityShim.GetCorrespondingObjectFromSource(go) == PrefabParent);
            var count = matches.Count();
            switch (ComparisonMode)
            {
                case InstanceCountComparison.AtLeast:
                    return count >= InstanceCount;

                case InstanceCountComparison.Exactly:
                    var complete = count == InstanceCount;
                    if (complete && InstanceCount == 1 && m_FutureReference != null)
                        m_FutureReference.SceneObjectReference.Update(matches.First());
                    return complete;

                case InstanceCountComparison.NoMoreThan:
                    return count <= InstanceCount;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns FutureObjectReference for this Criterion.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<FutureObjectReference> GetFutureObjectReferences()
        {
            if (m_FutureReference == null)
                yield break;

            yield return m_FutureReference;
        }

        /// <summary>
        /// Destroys unused future reference assets and updates future references.
        /// </summary>
        protected override void OnValidate()
        {
            // Destroy unreferenced future reference assets
            base.OnValidate();

            // Update future reference
            var needsUpdate = false;
            if (ComparisonMode == InstanceCountComparison.Exactly && InstanceCount == 1)
            {
                if (m_FutureReference == null)
                {
                    m_FutureReference = CreateFutureObjectReference();
                    m_FutureReference.ReferenceName = "Prefab Instance";
                    needsUpdate = true;
                }
            }
            else
                DestroyImmediate(m_FutureReference, true);

            if (needsUpdate)
                UpdateFutureObjectReferenceNames();
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            var prefabInstances = FindObjectsOfType<GameObject>().Where(go => PrefabUtilityShim.GetCorrespondingObjectFromSource(go) == PrefabParent);
            var actualInstanceCount = prefabInstances.Count();
            var difference = actualInstanceCount - InstanceCount;

            if (difference == 0)
                return true;

            switch (ComparisonMode)
            {
                case InstanceCountComparison.AtLeast:
                    difference = Math.Min(0, difference);
                    break;

                case InstanceCountComparison.NoMoreThan:
                    difference = Math.Max(0, difference);
                    break;
            }

            if (difference < 0)
            {
                for (var i = 0; i < -difference; i++)
                    PrefabUtility.InstantiatePrefab(PrefabParent);
            }
            else
            {
                foreach (var prefabInstance in prefabInstances.Take(difference))
                    DestroyImmediate(prefabInstance);
            }

            return true;
        }
    }
}
