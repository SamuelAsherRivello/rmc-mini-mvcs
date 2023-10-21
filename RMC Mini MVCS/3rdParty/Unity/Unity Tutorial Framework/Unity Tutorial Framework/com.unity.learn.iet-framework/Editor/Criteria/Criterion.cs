using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// A generic event for signaling changes in a criterion.
    /// Parameters: sender.
    /// </summary>
    [Serializable]
    public class CriterionEvent : UnityEvent<Criterion>
    {
    }

    /// <summary>
    /// Base class for Criterion implementations.
    /// </summary>
    public abstract class Criterion : ScriptableObject
    {
        /// <summary>
        /// Raised when any Criterion is completed.
        /// </summary>
        public static CriterionEvent CriterionCompleted = new CriterionEvent();

        /// <summary>
        /// Raised when any Criterion is invalidated.
        /// </summary>
        public static CriterionEvent CriterionInvalidated = new CriterionEvent();

        /// <summary>
        /// Raised when this criterion is completed.
        /// </summary>
        [Header("Events")]
        public CriterionEvent Completed = new CriterionEvent();

        /// <summary>
        /// Raised when this criterion is invalidated.
        /// </summary>
        public CriterionEvent Invalidated = new CriterionEvent();

        bool m_Completed;

        /// <summary>
        /// Is the Criterion completed. Setting this raises CriterionCompleted/CriterionInvalidated.
        /// </summary>
        public bool IsCompleted
        {
            get { return m_Completed; }
            internal set
            {
                if (performedAtLeastOneEvaluationSinceTestingStarted
                && (value == m_Completed))
                {
                    return;
                }

                m_Completed = value;
                if (m_Completed)
                {
                    Completed?.Invoke(this);
                    CriterionCompleted?.Invoke(this);
                }
                else
                {
                    Invalidated?.Invoke(this);
                    CriterionInvalidated?.Invoke(this);
                }
                performedAtLeastOneEvaluationSinceTestingStarted = true;
            }
        }

        /// <summary>
        /// Resets the completion state.
        /// </summary>
        public void ResetCompletionState()
        {
            IsCompleted = false;
        }

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public virtual void StartTesting()
        {
            isTesting = true;
            performedAtLeastOneEvaluationSinceTestingStarted = false;
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public virtual void StopTesting()
        {
            isTesting = false;
        }

        /// <summary>
        /// Is this criterion being tested right now?
        /// </summary>
        [SerializeField, HideInInspector]
        protected bool isTesting = false;

        /// <summary>
        /// Has at least one evaluation been performed since testing started?
        /// </summary>
        protected bool performedAtLeastOneEvaluationSinceTestingStarted = false;

        /// <summary>
        /// Runs update logic for the criterion.
        /// </summary>
        public virtual void UpdateCompletion()
        {
            if (!isTesting)
            {
                return;
            }
            IsCompleted = EvaluateCompletion();
        }

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected virtual bool EvaluateCompletion()
        {
            throw new NotImplementedException($"Missing implementation of EvaluateCompletion in: {GetType()}");
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public abstract bool AutoComplete();

        /// <summary>
        /// Returns FutureObjectReference for this Criterion.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<FutureObjectReference> GetFutureObjectReferences()
        {
            return Enumerable.Empty<FutureObjectReference>();
        }

        /// <summary>
        /// Destroys unreferenced future references.
        /// </summary>
        /// <see>
        /// https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnValidate.html
        /// </see>
        protected virtual void OnValidate()
        {
            // Find instanceIDs of referenced future references
            var referencedFutureReferenceInstanceIDs = new HashSet<int>();
            foreach (var futureReference in GetFutureObjectReferences())
                referencedFutureReferenceInstanceIDs.Add(futureReference.GetInstanceID());

            // Destroy unreferenced future references
            var assetPath = AssetDatabase.GetAssetPath(this);
            var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            foreach (var asset in assets)
            {
                if (asset is FutureObjectReference
                    && ((FutureObjectReference)asset).Criterion == this
                    && !referencedFutureReferenceInstanceIDs.Contains(asset.GetInstanceID()))
                {
                    DestroyImmediate(asset, true);
                }
            }
        }

        /// <summary>
        /// Creates a default FutureObjectReference for this Criterion.
        /// </summary>
        /// <returns></returns>
        protected FutureObjectReference CreateFutureObjectReference()
        {
            return CreateFutureObjectReference("Future Reference");
        }

        /// <summary>
        /// Creates a FutureObjectReference by specific name for this Criterion.
        /// </summary>
        /// <param name="referenceName"></param>
        /// <returns></returns>
        protected FutureObjectReference CreateFutureObjectReference(string referenceName)
        {
            var futureReference = CreateInstance<FutureObjectReference>();
            futureReference.Criterion = this;
            futureReference.ReferenceName = referenceName;

            var assetPath = AssetDatabase.GetAssetPath(this);
            AssetDatabase.AddObjectToAsset(futureReference, assetPath);

            return futureReference;
        }

        /// <summary>
        /// Updates names of the references.
        /// </summary>
        protected void UpdateFutureObjectReferenceNames()
        {
            // Update future reference names in next editor update due to AssetDatase interactions
            EditorApplication.update += UpdateFutureObjectReferenceNamesPostponed;
        }

        void UpdateFutureObjectReferenceNamesPostponed()
        {
            // Unsubscribe immediately since it should only be called once
            EditorApplication.update -= UpdateFutureObjectReferenceNamesPostponed;

            var assetPath = AssetDatabase.GetAssetPath(this);
            var tutorialPage = (TutorialPage)AssetDatabase.LoadMainAssetAtPath(assetPath);
            var futureReferences = AssetDatabase.LoadAllAssetsAtPath(assetPath)
                .Where(o => o is FutureObjectReference)
                .Cast<FutureObjectReference>();
            foreach (var futureReference in futureReferences)
                tutorialPage.UpdateFutureObjectReferenceName(futureReference);
        }
    }
}
