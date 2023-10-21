using System;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that Scene View Camera has moved.
    /// </summary>
    public class SceneViewCameraMovedCriterion : Criterion
    {
        [NonSerialized]
        bool m_InitialPositionInitialized = false;
        [NonSerialized]
        Vector3 m_InitialCameraPosition;
        [NonSerialized]
        Quaternion m_InitialCameraOrientation;

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public override void StartTesting()
        {
            base.StartTesting();
            UpdateInitialCameraPositionIfNeeded();
            UpdateCompletion();

            EditorApplication.update += UpdateCompletion;
        }

        void UpdateInitialCameraPositionIfNeeded()
        {
            if (m_InitialPositionInitialized)
                return;

            if (SceneView.lastActiveSceneView == null)
                return;

            m_InitialPositionInitialized = true;
            m_InitialCameraPosition = SceneView.lastActiveSceneView.camera.transform.position;
            m_InitialCameraOrientation = SceneView.lastActiveSceneView.camera.transform.localRotation;
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
            EditorApplication.update -= UpdateCompletion;
            m_InitialPositionInitialized = false;
        }

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            if (SceneView.lastActiveSceneView == null)
                return false;

            UpdateInitialCameraPositionIfNeeded();
            var currentPosition = SceneView.lastActiveSceneView.camera.transform.position;
            var currentOrientation = SceneView.lastActiveSceneView.camera.transform.localRotation;
            return m_InitialCameraPosition != currentPosition || m_InitialCameraOrientation != currentOrientation;
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            // NOTE SceneView.lastActiveSceneView.camera.transform.position should not be used for moving the camera,
            // must use the SceneView.lastActiveSceneView.pivot instead, https://forum.unity.com/threads/moving-scene-view-camera-from-editor-script.64920/#post-415327
            SceneView.lastActiveSceneView.pivot = m_InitialCameraPosition + Vector3.back;
            return true;
        }
    }
}
