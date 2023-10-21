using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(SceneViewCameraSettings))]
    class SceneViewCameraSettingsDrawer : PropertyDrawer
    {
        const string k_CameraModePath = "m_CameraMode";
        const string k_FocusModePath = "m_FocusMode";
        const string k_OrthographicPath = "m_Orthographic";
        const string k_PivotPath = "m_Pivot";
        const string k_RotationPath = "m_Rotation";
        const string k_SizePath = "m_Size";
        const string k_FrameObjectPath = "m_FrameObject";
        const string k_EnabledPath = "m_Enabled";

        static readonly float k_PaddedLineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float originalHeight = EditorGUI.GetPropertyHeight(property, label, true);
            Rect position = new Rect(0, 0, EditorGUIUtility.currentViewWidth, originalHeight);
            return DoGUI(position, property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DoGUI(position, property, label, false);
        }

        Rect DrawSubProperties(Rect original, SerializedProperty property, bool measureOnly, params string[] subpropertyNames)
        {
            Rect current = new Rect(original.x, original.y, original.width, EditorGUIUtility.singleLineHeight);
            foreach (string name in subpropertyNames)
            {
                SerializedProperty subProperty = property.FindPropertyRelative(name);
                // Showing raw quaternions to the user is useless, use Euler angles instead.
                bool isQuat = subProperty.type == "Quaternion";
                current.height = EditorGUI.GetPropertyHeight(subProperty, !isQuat);
                if (!measureOnly)
                {
                    if (isQuat)
                    {
                        Vector3 euler = subProperty.quaternionValue.eulerAngles;
                        EditorGUI.BeginProperty(current, new GUIContent(subProperty.name), subProperty);
                        EditorGUI.BeginChangeCheck();
                        euler = EditorGUI.Vector3Field(current, subProperty.displayName, euler);
                        if (EditorGUI.EndChangeCheck())
                            subProperty.quaternionValue = Quaternion.Euler(euler);
                        EditorGUI.EndProperty();
                    }
                    else
                    {
                        EditorGUI.PropertyField(current, subProperty, true);
                    }
                }
                current.y += current.height + EditorGUIUtility.standardVerticalSpacing;
            }
            current.height = original.height - (current.y - original.y);
            return current;
        }

        float DoGUI(Rect position, SerializedProperty property, GUIContent label, bool measureOnly)
        {
            Rect current = position;
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                SerializedProperty enabled = property.FindPropertyRelative(k_EnabledPath);
                if (!measureOnly)
                {
                    enabled.boolValue = EditorGUI.ToggleLeft(
                        new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label, enabled.boolValue
                    );
                    property.isExpanded = enabled.boolValue;
                }
                current.y += k_PaddedLineHeight;
                current.height -= k_PaddedLineHeight;
                if (!property.isExpanded)
                    return current.y - position.y;

                using (new EditorGUI.IndentLevelScope())
                {
                    current = DrawSubProperties(current, property, measureOnly, k_CameraModePath, k_FocusModePath);

                    var focusMode = (SceneViewFocusMode)property.FindPropertyRelative(k_FocusModePath).enumValueIndex;
                    switch (focusMode)
                    {
                        case SceneViewFocusMode.Manual:
                            current = DrawSubProperties(current, property, measureOnly, k_OrthographicPath, k_PivotPath, k_RotationPath, k_SizePath);
                            break;
                        case SceneViewFocusMode.FrameObject:
                            current = DrawSubProperties(current, property, measureOnly, k_FrameObjectPath);
                            break;
                        default:
                            break;
                    }

                    if (focusMode == SceneViewFocusMode.Manual)
                    {
                        current.height = EditorGUIUtility.singleLineHeight;
                        current = EditorGUI.IndentedRect(current);
                        if (!measureOnly && GUI.Button(current, "Store Current Scene View Camera Settings"))
                        {
                            SceneView sceneView = EditorWindow.GetWindow<SceneView>();
                            property.FindPropertyRelative(k_CameraModePath).intValue = (int)(
                                sceneView.in2DMode ?
                                SceneViewCameraMode.SceneView2D : SceneViewCameraMode.SceneView3D
                            );
                            property.FindPropertyRelative(k_OrthographicPath).boolValue = sceneView.orthographic;
                            property.FindPropertyRelative(k_SizePath).floatValue = sceneView.size;
                            property.FindPropertyRelative(k_PivotPath).vector3Value = sceneView.pivot;
                            property.FindPropertyRelative(k_RotationPath).quaternionValue = sceneView.rotation;
                        }
                        current.y += k_PaddedLineHeight;
                    }
                }
            }
            return current.y - position.y;
        }
    }
}
