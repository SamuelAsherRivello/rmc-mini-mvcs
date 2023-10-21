using System;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// The Scene View camera mode
    /// </summary>
    public enum SceneViewCameraMode
    {
        /// <summary>
        /// 2D view.
        /// </summary>
        SceneView2D,
        /// <summary>
        /// 3D view.
        /// </summary>
        SceneView3D
    };

    /// <summary>
    /// Determines how the camera position is applied when loaded
    /// </summary>
    public enum SceneViewFocusMode
    {
        /// <summary>
        /// The camera is positioned as specified by the various settings.
        /// </summary>
        Manual,
        /// <summary>
        /// A specific object is framed automatically.
        /// </summary>
        FrameObject
    };

    /// <summary>
    /// Used to store and apply scene view camera settings
    /// </summary>
    [Serializable]
    public class SceneViewCameraSettings
    {
        /// <summary>
        /// The Scene View camera mode
        /// </summary>
        public SceneViewCameraMode CameraMode => m_CameraMode;
        [SerializeField]
        SceneViewCameraMode m_CameraMode = SceneViewCameraMode.SceneView2D;

        /// <summary>
        /// Determines how the camera position is applied when loaded
        /// </summary>
        public SceneViewFocusMode FocusMode => m_FocusMode;
        [SerializeField]
        SceneViewFocusMode m_FocusMode = SceneViewFocusMode.Manual;

        /// <summary>
        /// Is the camera ortographic?
        /// </summary>
        public bool Orthographic => m_Orthographic;
        [SerializeField]
        bool m_Orthographic = false;

        /// <summary>
        /// Ortographic size of the camera
        /// </summary>
        public float Size => m_Size;
        [SerializeField]
        float m_Size = default;

        /// <summary>
        /// The point the camera will look at
        /// </summary>
        public Vector3 Pivot => m_Pivot;
        [SerializeField]
        Vector3 m_Pivot = default;

        /// <summary>
        /// The rotation of the camera
        /// </summary>
        public Quaternion Rotation => m_Rotation;
        [SerializeField]
        Quaternion m_Rotation = default;

        /// <summary>
        /// The object that can be framed by the camera.
        /// Applicable if SceneViewFocusMode.FrameObject used as FocusMode.
        /// </summary>
        public SceneObjectReference FrameObject => m_FrameObject;
        [SerializeField]
        SceneObjectReference m_FrameObject = null;

        /// <summary>
        /// Are these camera settings going to be used?
        /// </summary>
        public bool Enabled => m_Enabled;
        [SerializeField]
        bool m_Enabled = false;

        /// <summary>
        /// Applies the saved camera settings to the current scene camera
        /// </summary>
        public void Apply()
        {
            var sceneView = EditorWindow.GetWindow<SceneView>(null, false);
            sceneView.in2DMode = (CameraMode == SceneViewCameraMode.SceneView2D);
            switch (FocusMode)
            {
                case SceneViewFocusMode.FrameObject:
                    GameObject go = FrameObject.ReferencedObjectAsGameObject;
                    if (go == null)
                        throw new InvalidOperationException("Error looking up frame object");
                    sceneView.Frame(GameObjectProxy.CalculateBounds(go), true);
                    break;
                case SceneViewFocusMode.Manual:
                    sceneView.LookAt(Pivot, Rotation, Size, Orthographic, false);
                    break;
                default:
                    throw new NotImplementedException(string.Format("Focus mode {0} not supported", FocusMode));
            }
        }
    }
}
