using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(TypedCriterion))]
    class TypedCriterionDrawer : PropertyDrawer
    {
        // criterionProperty is a SerializedProperty on the SerializedObject for the Criterion
        delegate void PropertyIteratorCallback(SerializedProperty criterionProperty);

        const string k_TypeField = nameof(TypedCriterion.Type);
        const string k_CriterionField = nameof(TypedCriterion.Criterion);
        // Base class properties we want to draw after the derived class properties
        static readonly List<string> k_BaseClassProperties = new List<string>
        {
            nameof(Criterion.Completed),
            nameof(Criterion.Invalidated),
        };
        static readonly List<string> k_PropertiesToIgnore = new List<string> { "m_Script" };

        Dictionary<string, SerializedObject> m_PerPropertyCriterionSerializedObjects =
            new Dictionary<string, SerializedObject>();

        Rect m_CriterionPropertyRect;
        bool m_InspectorRedrawn = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            property.isExpanded = true;
            var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            IterateCriterion(
                property.FindPropertyRelative(k_CriterionField),
                p => height += EditorGUI.GetPropertyHeight(p) + EditorGUIUtility.standardVerticalSpacing
            );
            height -= EditorGUIUtility.standardVerticalSpacing;
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            m_InspectorRedrawn = true;

            Rect typeFieldPosition = position;
            typeFieldPosition.height = EditorGUIUtility.singleLineHeight;

            property.serializedObject.ApplyModifiedProperties(); //otherwise the Update() will remove all changes that happened on the object before this property was drawn
            property.serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(typeFieldPosition, property.FindPropertyRelative(k_TypeField));

            property.serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                OnCriterionTypeChanged(property);
            }

            position.y += typeFieldPosition.height + EditorGUIUtility.standardVerticalSpacing;
            position.height -= typeFieldPosition.height + EditorGUIUtility.standardVerticalSpacing;
            m_CriterionPropertyRect = position;
            IterateCriterion(property.FindPropertyRelative(k_CriterionField), OnGUIIterateCriterion);
        }

        SerializedObject GetSerializedObject(SerializedProperty criterionProperty)
        {
            if (criterionProperty.objectReferenceValue == null)
                return null;

            string key = criterionProperty.propertyPath;
            SerializedObject serializedObject;
            var found = m_PerPropertyCriterionSerializedObjects.TryGetValue(key, out serializedObject);
            if (!found || serializedObject.targetObject == null)
            {
                serializedObject = new SerializedObject(criterionProperty.objectReferenceValue);
                m_PerPropertyCriterionSerializedObjects[key] = serializedObject;
            }

            return serializedObject;
        }

        void IterateCriterion(SerializedProperty criterion, PropertyIteratorCallback onIterateChildProperty)
        {
            if (criterion.objectReferenceValue == null)
                return;

            var serializedObject = GetSerializedObject(criterion);
            if (serializedObject == null)
                return;

            // First pass: draw properties of the derived class.
            var childProperty = serializedObject.GetIterator();
            childProperty.NextVisible(true);
            while (childProperty.NextVisible(childProperty.isExpanded))
            {
                if (k_PropertiesToIgnore.Contains(childProperty.propertyPath))
                    continue;
                if (k_BaseClassProperties.Contains(childProperty.propertyPath))
                    continue;

                onIterateChildProperty(childProperty);
            }

            // Second pass: draw properties of the base class.
            childProperty = serializedObject.GetIterator();
            childProperty.NextVisible(true);
            while (childProperty.NextVisible(childProperty.isExpanded))
            {
                if (k_BaseClassProperties.Contains(childProperty.propertyPath))
                    onIterateChildProperty(childProperty);
            }
        }

        void OnGUIIterateCriterion(SerializedProperty criterionProperty)
        {
            criterionProperty.serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            m_CriterionPropertyRect.height = EditorGUI.GetPropertyHeight(criterionProperty);
            EditorGUI.PropertyField(m_CriterionPropertyRect, criterionProperty, true);
            m_CriterionPropertyRect.y += m_CriterionPropertyRect.height + EditorGUIUtility.standardVerticalSpacing;

            if (EditorGUI.EndChangeCheck())
            {
                criterionProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        void OnCriterionTypeChanged(SerializedProperty parentProperty)
        {
            var criterionProperty = parentProperty.FindPropertyRelative(k_CriterionField);

            if (criterionProperty.objectReferenceValue != null)
                Undo.DestroyObjectImmediate(criterionProperty.objectReferenceValue);

            var criterionType = System.Type.GetType(
                parentProperty.FindPropertyRelative(k_TypeField).FindPropertyRelative("m_TypeName").stringValue
            );

            if (criterionType != null)
            {
                var criterion = ScriptableObject.CreateInstance(criterionType);
                Undo.RegisterCreatedObjectUndo(criterion, "Change Criterion");
                criterion.hideFlags |= HideFlags.HideInHierarchy;

                AssetDatabase.AddObjectToAsset(criterion, parentProperty.serializedObject.targetObject);
                string parentAssetPath = AssetDatabase.GetAssetPath(parentProperty.serializedObject.targetObject);

                // Work around "NullReferenceException: SerializedObject of SerializedProperty has been Disposed.",
                // https://fogbugz.unity3d.com/f/cases/1318338/
#if UNITY_2020_2_6 || UNITY_2020_2_7 || (UNITY_2020_3_OR_NEWER && !UNITY_2021)
                EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutineOwnerless(ImportCriterionParentAssetWhenReady(criterionProperty, criterion, parentAssetPath));
#else
                AssetDatabase.ImportAsset(parentAssetPath);
#endif
                criterionProperty.objectReferenceValue = criterion;

                m_PerPropertyCriterionSerializedObjects.Clear();
            }
            else
            {
                criterionProperty.objectReferenceValue = null;
            }
        }

        IEnumerator ImportCriterionParentAssetWhenReady(SerializedProperty criterionProperty, ScriptableObject criterion, string parentAssetPath)
        {
            do
            {
                yield return null;
            }
            while (criterionProperty.objectReferenceValue != criterion);

            //this seems to be necessary in order to prevent errors when multiple criteria are on the same tutorial page
            m_InspectorRedrawn = false;

            do
            {
                yield return null;
            }
            while (!m_InspectorRedrawn);

            AssetDatabase.ImportAsset(parentAssetPath);
        }
    }
}
