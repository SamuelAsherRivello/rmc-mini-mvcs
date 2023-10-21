using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(UnmaskedView))]
    class UnmaskedViewDrawer : PropertyDrawer
    {
        const string k_SelectorTypePath = nameof(UnmaskedView.m_SelectorType);
        const string k_ViewTypePath = nameof(UnmaskedView.m_ViewType);
        const string k_EditorWindowTypePath = nameof(UnmaskedView.m_EditorWindowType);
        const string k_AlternateEditorWindowTypesPath = nameof(UnmaskedView.m_AlternateEditorWindowTypes);
        const string k_UnmaskedControlsPath = nameof(UnmaskedView.m_UnmaskedControls);
        const string k_UnmaskTypePath = nameof(UnmaskedView.m_MaskType);
        const string k_MaskSizeModifierPath = nameof(UnmaskedView.m_MaskSizeModifier);

        readonly Dictionary<string, ReorderableList> m_UnmaskedControlsPerPropertyPath =
            new Dictionary<string, ReorderableList>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var selectorType = property.FindPropertyRelative(k_SelectorTypePath);
            var height = EditorGUI.GetPropertyHeight(selectorType) + EditorGUIUtility.standardVerticalSpacing;
            switch ((UnmaskedView.SelectorType)selectorType.intValue)
            {
                case UnmaskedView.SelectorType.EditorWindow:
                    height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_EditorWindowTypePath), true);
                    height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_AlternateEditorWindowTypesPath), true);
                    break;
                case UnmaskedView.SelectorType.GUIView:
                    height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_ViewTypePath), true);
                    break;
            }
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_UnmaskTypePath));
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(k_MaskSizeModifierPath));
            var listControl = GetListControl(property);
            height += EditorGUIUtility.standardVerticalSpacing + listControl.GetHeight();
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var selectorType = property.FindPropertyRelative(k_SelectorTypePath);
            position.height = EditorGUI.GetPropertyHeight(selectorType, true);
            EditorGUI.PropertyField(position, selectorType);

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            SerializedProperty typeProperty = null;
            switch ((UnmaskedView.SelectorType)selectorType.intValue)
            {
                case UnmaskedView.SelectorType.EditorWindow:
                    typeProperty = property.FindPropertyRelative(k_EditorWindowTypePath);
                    break;
                case UnmaskedView.SelectorType.GUIView:
                    typeProperty = property.FindPropertyRelative(k_ViewTypePath);
                    break;
            }
            position.height = EditorGUI.GetPropertyHeight(typeProperty, true);
            EditorGUI.PropertyField(position, typeProperty);

            if ((UnmaskedView.SelectorType)selectorType.intValue == UnmaskedView.SelectorType.EditorWindow)
            {
                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                var alternativeEditorWindowTypes = property.FindPropertyRelative(k_AlternateEditorWindowTypesPath);
                position.height = EditorGUI.GetPropertyHeight(alternativeEditorWindowTypes, true);
                EditorGUI.PropertyField(position, alternativeEditorWindowTypes);
            }

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            var unmaskType = property.FindPropertyRelative(k_UnmaskTypePath);
            position.height = EditorGUI.GetPropertyHeight(unmaskType, true);
            EditorGUI.PropertyField(position, unmaskType);

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            var maskSizeModifier = property.FindPropertyRelative(k_MaskSizeModifierPath);
            position.height = EditorGUI.GetPropertyHeight(maskSizeModifier, true);
            EditorGUI.PropertyField(position, maskSizeModifier);

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            var listControl = GetListControl(property);
            position.height = listControl.GetHeight();
            position = EditorGUI.IndentedRect(position);
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
                listControl.DoList(position);
        }

        ReorderableList GetListControl(SerializedProperty parentProperty)
        {
            string key = parentProperty.propertyPath;
            ReorderableList list;
            if (!m_UnmaskedControlsPerPropertyPath.TryGetValue(key, out list))
            {
                list = new ReorderableList(parentProperty.serializedObject, parentProperty.FindPropertyRelative(k_UnmaskedControlsPath));
                list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Unmasked Controls");
                list.elementHeightCallback = (index) => GetElementHeightForListIndex(list, index);
                list.drawElementCallback = (rect, index, isActive, isFocused) =>
                    EditorGUI.PropertyField(rect, list.serializedProperty.GetArrayElementAtIndex(index), true);
                m_UnmaskedControlsPerPropertyPath[key] = list;
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
