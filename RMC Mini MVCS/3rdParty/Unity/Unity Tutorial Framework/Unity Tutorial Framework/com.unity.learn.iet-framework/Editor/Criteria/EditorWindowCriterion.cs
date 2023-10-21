using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that a specific EditorWindow is opened.
    /// </summary>
    public class EditorWindowCriterion : Criterion
    {
        [SerializedTypeFilter(typeof(EditorWindow), false)]
        [SerializeField]
        SerializedType m_EditorWindowType = new SerializedType(null);

        /// <summary>
        /// The EditorWindow type we want to test for.
        /// </summary>
        public SerializedType EditorWindowType { get => m_EditorWindowType; set => m_EditorWindowType = value; }

        [SerializeField]
        bool m_CloseIfAlreadyOpen;

        EditorWindow m_WindowInstance;

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
            if (m_EditorWindowType.Type == null)
            {
                return false;
            }
            if (!m_WindowInstance)
            {
                var windows = Resources.FindObjectsOfTypeAll(m_EditorWindowType.Type);

                foreach (var w in windows)
                {
                    if (w.GetType() == m_EditorWindowType.Type)
                    {
                        m_WindowInstance = (EditorWindow)w;

                        m_WindowInstance.Focus();
                        return true;
                    }
                }
                return false;
            }
            if (m_WindowInstance.GetType() != m_EditorWindowType.Type)
            {
                m_WindowInstance = null;
            }
            return true;
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            if (m_EditorWindowType.Type == null)
            {
                return false;
            }

            EditorWindow.GetWindow(m_EditorWindowType.Type);
            return true;
        }
    }
}
