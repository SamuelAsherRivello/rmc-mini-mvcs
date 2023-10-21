using System;
using UnityEngine;
using UnityEditor;

namespace Unity.Tutorials.Core.Editor
{
    class ComponentAddedCriterionDrawers
    {
        [CustomPropertyDrawer(typeof(ComponentAddedCriterion.TypeAndFutureReference))]
        class TypeAndFutureReferenceDrawer : PropertyDrawer
        {
            static string s_SerializedTypeField = "SerializedType";

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                var serializedTypeProperty = property.FindPropertyRelative(s_SerializedTypeField);
                return EditorGUI.GetPropertyHeight(serializedTypeProperty);
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                var serializedTypeProperty = property.FindPropertyRelative(s_SerializedTypeField);
                EditorGUI.PropertyField(position, serializedTypeProperty, GUIContent.none);
            }
        }

        [CustomPropertyDrawer(typeof(ComponentAddedCriterion.SerializedTypeCollection))]
        class TypedCriterionCollectionDrawer : CollectionWrapperDrawer
        {
            const string k_FutureReferencePath = "FutureReference";

            protected override void OnReorderableListCreated(UnityEditorInternal.ReorderableList list)
            {
                base.OnReorderableListCreated(list);
                list.onAddCallback = lst =>
                {
                    ++lst.serializedProperty.arraySize;
                    lst.serializedProperty.serializedObject.ApplyModifiedProperties();
                    var lastElement = lst.serializedProperty.GetArrayElementAtIndex(lst.serializedProperty.arraySize - 1);
                    lastElement.FindPropertyRelative(k_FutureReferencePath).objectReferenceValue = null;
                    list.serializedProperty.serializedObject.ApplyModifiedProperties();
                };
            }
        }
    }
}
