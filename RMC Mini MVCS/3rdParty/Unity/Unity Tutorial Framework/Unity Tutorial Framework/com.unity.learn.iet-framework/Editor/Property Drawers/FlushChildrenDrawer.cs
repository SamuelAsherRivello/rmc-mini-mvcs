using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    class FlushChildrenDrawer : PropertyDrawer
    {
        private static GUIContent s_Label = new GUIContent();

        private static HashSet<SerializedPropertyType> s_ExpandableTypes = new HashSet<SerializedPropertyType>(new[]
        {
            SerializedPropertyType.Generic, SerializedPropertyType.Quaternion, SerializedPropertyType.Vector4
        });

        public bool ShouldDisplayFoldout { get { return false; } }

        protected virtual void DisplayChildProperty(Rect position, SerializedProperty parentProperty,
            SerializedProperty childProperty, GUIContent label)
        {
            EditorGUI.PropertyField(position, childProperty, label, true);
        }

        protected virtual float GetChildPropertyHeight(SerializedProperty parentProperty,
            SerializedProperty childProperty)
        {
            s_Label.text = childProperty.displayName;
            return EditorGUI.GetPropertyHeight(childProperty, s_Label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var result = 0f;
            var childProperty = property.Copy();
            var endProperty = property.GetEndProperty();
            childProperty.NextVisible(true);
            while (!SerializedProperty.EqualContents(childProperty, endProperty))
            {
                result +=
                    GetChildPropertyHeight(property, childProperty.Copy()) + EditorGUIUtility.standardVerticalSpacing;
                childProperty.NextVisible(false);
            }
            if (result > 0f)
                result -= EditorGUIUtility.standardVerticalSpacing;
            return result;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // if the property is not an expandable type, then use its default drawer
            if (!s_ExpandableTypes.Contains(property.propertyType))
                EditorGUI.PropertyField(position, property, label, property.hasVisibleChildren && property.isExpanded);
            else
            {
                var childProperty = property.Copy();
                var endProperty = property.GetEndProperty();
                childProperty.NextVisible(true);
                while (!SerializedProperty.EqualContents(childProperty, endProperty))
                {
                    position.height = GetChildPropertyHeight(property, childProperty);
                    DisplayChildProperty(position, property, childProperty.Copy(), null);
                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                    childProperty.NextVisible(false);
                }
            }
        }
    }
}
