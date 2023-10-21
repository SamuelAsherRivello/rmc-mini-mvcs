using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor
{
    class InstantiatePrefabCriterionDrawers
    {
        [CustomPropertyDrawer(typeof(InstantiatePrefabCriterion.FuturePrefabInstance))]
        class FuturePrefabInstanceDrawer : PropertyDrawer
        {
            const string k_PrefabParentPropertyPath = "m_PrefabParent";

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                var prefabParentProperty = property.FindPropertyRelative(k_PrefabParentPropertyPath);
                return EditorGUI.GetPropertyHeight(prefabParentProperty);
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                position.height = GetPropertyHeight(property, label);

                var prefabParentProperty = property.FindPropertyRelative(k_PrefabParentPropertyPath);
                var obj = prefabParentProperty.objectReferenceValue;

                EditorGUI.BeginProperty(position, GUIContent.none, prefabParentProperty);
                EditorGUI.BeginChangeCheck();

                var newObj = EditorGUI.ObjectField(position, obj, typeof(UnityObject), true);

                if (EditorGUI.EndChangeCheck())
                {
                    // Replace prefab instance with its prefab parent
                    if (newObj != null && PrefabUtility.GetPrefabInstanceStatus(newObj) != PrefabInstanceStatus.NotAPrefab)
                        newObj = PrefabUtilityShim.GetCorrespondingObjectFromSource(newObj);

                    prefabParentProperty.objectReferenceValue = newObj;
                }
                EditorGUI.EndProperty();

                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}
