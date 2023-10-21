using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that a specific Editor Tool is selected.
    /// </summary>
    public class ActiveToolCriterion : Criterion
    {
        /// <summary>
        /// The Tool we wish to test for.
        /// </summary>
        public Tool TargetTool { get { return m_TargetTool; } set { m_TargetTool = value; } }
        [SerializeField]
        Tool m_TargetTool;

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
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            return Tools.current == m_TargetTool;
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            Tools.current = m_TargetTool;
            return true;
        }
    }
}
