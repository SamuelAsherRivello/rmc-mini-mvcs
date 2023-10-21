using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(CollectionWrapper), true)]
    class CollectionWrapperDrawer : PropertyDrawer
    {
        const string k_ItemsPath = "m_Items";

        Dictionary<string, ReorderableList> m_Widgets = new Dictionary<string, ReorderableList>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetListWidget(property, label).GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GetListWidget(property, label).DoList(position);
        }

        protected virtual void OnReorderableListCreated(ReorderableList list)
        {
        }

        ReorderableList GetListWidget(SerializedProperty property, GUIContent label)
        {
            string key = property.propertyPath;
            if (m_Widgets.ContainsKey(key))
            {
                return m_Widgets[key];
            }
            var reorderableList = new ReorderableList(property.serializedObject, property.FindPropertyRelative(k_ItemsPath));
            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
                EditorGUI.PropertyField(rect, reorderableList.serializedProperty.GetArrayElementAtIndex(index), true);
            label = label != null ? new GUIContent(label) : new GUIContent(property.displayName);
            reorderableList.drawHeaderCallback = delegate(Rect rect) {
                int oldIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                EditorGUI.LabelField(rect, label);
                EditorGUI.indentLevel = oldIndent;
            };
            reorderableList.elementHeightCallback = (int index) => GetElementHeightForListIndex(reorderableList, index);
            reorderableList.onAddCallback = delegate(ReorderableList lst) {
                ++lst.serializedProperty.arraySize;
                lst.serializedProperty.serializedObject.ApplyModifiedProperties();
            };
            reorderableList.onRemoveCallback = delegate(ReorderableList lst) {
                int index = lst.index;
                if (index >= lst.serializedProperty.arraySize)
                {
                    return;
                }
                SerializedProperty element = lst.serializedProperty.GetArrayElementAtIndex(index);
                if (
                    element.propertyType == SerializedPropertyType.ObjectReference &&
                    element.objectReferenceValue != null
                )
                {
                    lst.serializedProperty.DeleteArrayElementAtIndex(index);
                }
                lst.serializedProperty.DeleteArrayElementAtIndex(index);
                lst.serializedProperty.serializedObject.ApplyModifiedProperties();
            };
            m_Widgets[key] = reorderableList;
            OnReorderableListCreated(reorderableList);
            return reorderableList;
        }

        float GetElementHeightForListIndex(ReorderableList list, int index)
        {
            if (list.count <= index) //this happens from 2021 LTS onward
            {
                return EditorGUIUtility.standardVerticalSpacing * 2f + 1f;
            }
            return EditorGUI.GetPropertyHeight(list.serializedProperty.GetArrayElementAtIndex(index)) +
                    EditorGUIUtility.standardVerticalSpacing * 2f + 1f;
        }
    }
}
