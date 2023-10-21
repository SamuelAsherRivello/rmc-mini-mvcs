using System;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking a specific Play Mode state.
    /// </summary>
    public class PlayModeStateCriterion : Criterion
    {
        enum PlayModeState
        {
            Playing,
            NotPlaying
        }

        [SerializeField]
        PlayModeState m_RequiredPlayModeState = PlayModeState.Playing;

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public override void StartTesting()
        {
            base.StartTesting();
            UpdateCompletion();

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            switch (playModeStateChange)
            {
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                    UpdateCompletion();
                    break;
            }
        }

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            switch (m_RequiredPlayModeState)
            {
                case PlayModeState.NotPlaying:
                    return !EditorApplication.isPlaying;

                case PlayModeState.Playing:
                    return EditorApplication.isPlaying;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            EditorApplication.isPlaying = m_RequiredPlayModeState == PlayModeState.Playing;

            return true;
        }
    }
}
