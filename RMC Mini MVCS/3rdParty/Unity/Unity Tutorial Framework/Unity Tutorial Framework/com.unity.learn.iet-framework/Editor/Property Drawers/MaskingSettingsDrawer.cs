using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(MaskingSettings))]
    class MaskingSettingsDrawer : PropertyDrawer
    {
        const string k_EnabledPath = "m_MaskingEnabled";
        const string k_UnmaskedViewsPath = "m_UnmaskedViews";

        readonly Dictionary<string, ReorderableList> m_UnmaskedViewsPerPropertyPath =
            new Dictionary<string, ReorderableList>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var enabled = property.FindPropertyRelative(k_EnabledPath);
            var height = EditorGUI.GetPropertyHeight(enabled);
            if (enabled.boolValue)
                height += EditorGUIUtility.standardVerticalSpacing + GetListControl(property).GetHeight();
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enabled = property.FindPropertyRelative(k_EnabledPath);
            position.height = EditorGUI.GetPropertyHeight(enabled);
            using (new EditorGUI.PropertyScope(position, label, enabled))
                property.isExpanded = enabled.boolValue = EditorGUI.ToggleLeft(position, label, enabled.boolValue);

            if (!property.isExpanded)
                return;

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            var listControl = GetListControl(property);
            position.height = listControl.GetHeight();
            using (new EditorGUI.IndentLevelScope())
                position = EditorGUI.IndentedRect(position);
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
                listControl.DoList(position);
        }

        ReorderableList GetListControl(SerializedProperty parentProperty)
        {
            string key = parentProperty.propertyPath;
            ReorderableList list;
            if (!m_UnmaskedViewsPerPropertyPath.TryGetValue(key, out list))
            {
                list = new ReorderableList(parentProperty.serializedObject, parentProperty.FindPropertyRelative(k_UnmaskedViewsPath));
                list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Unmasked Views");
                list.elementHeightCallback = (index) => GetElementHeightForListIndex(list, index);
                list.drawElementCallback = (rect, index, isActive, isFocused) =>
                    EditorGUI.PropertyField(rect, list.serializedProperty.GetArrayElementAtIndex(index), true);
                m_UnmaskedViewsPerPropertyPath[key] = list;
            }
            return list;
        }

        float GetElementHeightForListIndex(ReorderableList list, int index)
        {
            if (list.count <= index) //this happens from 2021 LTS onward
            {
                return EditorGUIUtility.standardVerticalSpacing;
            }
            return EditorGUI.GetPropertyHeight(list.serializedProperty.GetArrayElementAtIndex(index), true) +
            EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
