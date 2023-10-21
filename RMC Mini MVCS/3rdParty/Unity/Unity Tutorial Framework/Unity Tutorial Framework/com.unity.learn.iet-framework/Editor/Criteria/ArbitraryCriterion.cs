using System;
using UnityEditor;
using UnityEngine;
using SerializableCallback;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Allows tutorial author to specify arbitrary completion criterion.
    /// Create a new ScriptableObject for your criterion and provide e.g. "bool IsMyCriterionSatisfied()"
    /// function as Callback to evalute the criterion. Provide a function that completes your criterion as
    /// AutoCompleteCallback if you wish to be able to auto-complete the page.
    /// </summary>
    public class ArbitraryCriterion : Criterion
    {
        /// <summary>
        /// Needed for serialization.
        /// </summary>
        [Serializable]
        public class BoolCallback : SerializableCallback<bool>
        {
        }

        /// <summary>
        /// The callback for criterion evalution logic.
        /// </summary>
        public BoolCallback Callback { get => m_Callback; set => m_Callback = value; }
        [SerializeField]
        BoolCallback m_Callback = default;

        /// <summary>
        /// The callback for auto-completion logic.
        /// </summary>
        public BoolCallback AutoCompleteCallback { get => m_AutoCompleteCallback; set => m_AutoCompleteCallback = value; }
        [SerializeField]
        BoolCallback m_AutoCompleteCallback = default;

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            // TODO revisit the logic here -- should AutoCompleteCallback take a precedence over Callback?
            // Or set some internal state to completed state?
            if (m_Callback != null)
                return m_Callback.Invoke();
            else
                return false;
        }

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public override void StartTesting()
        {
            base.StartTesting();
            UpdateCompletion();

            EditorApplication.update += UpdateCompletion;
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
            EditorApplication.update -= UpdateCompletion;
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            if (m_AutoCompleteCallback != null)
                return m_AutoCompleteCallback.Invoke();
            else
                return false;
        }
    }
}
