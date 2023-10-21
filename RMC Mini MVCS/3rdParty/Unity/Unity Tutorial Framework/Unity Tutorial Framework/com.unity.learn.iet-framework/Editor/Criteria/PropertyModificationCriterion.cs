using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking a property modification.
    /// </summary>
    public class PropertyModificationCriterion : Criterion
    {
        internal enum ValueMode
        {
            TargetValue = 0,
            DifferentThanInitial
        }

        internal enum ValueType
        {
            Integer,
            Decimal,
            Text,
            Boolean,
            Color,
        }

        internal string PropertyPath { get => m_PropertyPath; set => m_PropertyPath = value; }
        [SerializeField]
        string m_PropertyPath;

        internal ValueMode TargetValueMode { get => m_TargetValueMode; set => m_TargetValueMode = value; }

        [SerializeField]
        ValueMode m_TargetValueMode = ValueMode.TargetValue;

        // TODO: Make this more like TypedCriterion
        internal string TargetValue { get => m_TargetValue; set => m_TargetValue = value; }
        [SerializeField]
        [Tooltip("This value only applies if the TargetValueMode is set to TargetValue. This field will have no effects in other modes.")]
        string m_TargetValue;

        internal ValueType TargetValueType { get => m_TargetValueType; set => m_TargetValueType = value; }
        [SerializeField]
        ValueType m_TargetValueType;

        internal SceneObjectReference Target { get => m_Target.SceneObjectReference; set => m_Target.SceneObjectReference = value; }
        [SerializeField]
        ObjectReference m_Target = new ObjectReference();

        [NonSerialized]
        string m_InitialValue;

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public override void StartTesting()
        {
            base.StartTesting();
            var target = m_Target.SceneObjectReference.ReferencedObject;
            if (m_TargetValueMode == ValueMode.TargetValue)
                IsCompleted = PropertyFulfillCriterion(target, m_PropertyPath);
            else
            {
                var so = new SerializedObject(target);
                var sp = so.FindProperty(PropertyPath);

                if (sp == null)
                    Debug.LogWarningFormat("PropertyModificationCriterion: Cannot find property \"{0}\" on \"{1}\"", PropertyPath, target);
                else
                    m_InitialValue = GetPropertyValueAsString(sp);
            }

            Undo.postprocessModifications += PostprocessModifications;
            Undo.undoRedoPerformed += UpdateCompletion;
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
            Undo.postprocessModifications -= PostprocessModifications;
            Undo.undoRedoPerformed -= UpdateCompletion;
        }

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            var targetObject = m_Target.SceneObjectReference.ReferencedObject;
            return PropertyFulfillCriterion(targetObject, m_PropertyPath);
        }

        UndoPropertyModification[] PostprocessModifications(UndoPropertyModification[] modifications)
        {
            var targetObject = m_Target.SceneObjectReference.ReferencedObject;
            var modificationsToTest = GetPropertiesToTest(modifications, targetObject);
            if (modificationsToTest.Any())
            {
                IsCompleted = modificationsToTest.Any(m => PropertyFulfillCriterion(m.target, m.propertyPath));
            }

            return modifications;
        }

        IEnumerable<PropertyModification> GetPropertiesToTest(UndoPropertyModification[] modifications, UnityObject target)
        {
            var result = new List<PropertyModification>();
            foreach (var m in modifications)
            {
                if (m.currentValue.target == target)
                {
                    if (IsCompoundPropertyMatch(m.currentValue.propertyPath))
                    {
                        var propertyModification = m.currentValue;
                        propertyModification.propertyPath = PropertyPath;
                        result.Add(m.currentValue);
                    }
                    else if (m.currentValue.propertyPath == m_PropertyPath)
                        result.Add(m.currentValue);
                }
            }
            return result;
        }

        bool IsCompoundPropertyMatch(string propertyPath)
        {
            if (m_TargetValueType == ValueType.Color)
            {
                Regex coloRegex = new Regex(m_PropertyPath + "\\.[rgba]");
                if (coloRegex.IsMatch(propertyPath))
                    return true;
            }
            return propertyPath == m_PropertyPath;
        }

        bool DoPropertyTypeMatches(SerializedProperty property)
        {
            switch (m_TargetValueType)
            {
                case ValueType.Decimal:
                    return property.propertyType == SerializedPropertyType.Float;
                case ValueType.Integer:
                    return property.propertyType == SerializedPropertyType.Integer;
                case ValueType.Text:
                    return property.propertyType == SerializedPropertyType.String;
                case ValueType.Boolean:
                    return property.propertyType == SerializedPropertyType.Boolean;
                case ValueType.Color:
                    return property.propertyType == SerializedPropertyType.Color;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            throw new Exception("unknown TargetValueType");
        }

        string GetPropertyValueAsString(SerializedProperty property)
        {
            switch (TargetValueType)
            {
                case ValueType.Decimal:
                    return property.floatValue.ToString();
                case ValueType.Integer:
                    return property.intValue.ToString();
                case ValueType.Text:
                    return property.stringValue;
                case ValueType.Boolean:
                    return property.boolValue.ToString();
                case ValueType.Color:
                    return property.colorValue.ToString();
            }

            throw new Exception("unknown TargetValueType");
        }

        bool DoesPropertyMatches(SerializedProperty property, string value)
        {
            switch (TargetValueType)
            {
                case ValueType.Decimal:
                {
                    float convertedValue;
                    return float.TryParse(value, out convertedValue) &&
                        Mathf.Approximately(property.floatValue, convertedValue);
                }

                case ValueType.Integer:
                {
                    int convertedValue;
                    return int.TryParse(value, out convertedValue) && property.intValue == convertedValue;
                }
                case ValueType.Text:
                {
                    return property.stringValue == value;
                }
                case ValueType.Boolean:
                {
                    bool convertedValue;
                    return bool.TryParse(value, out convertedValue) && property.boolValue == convertedValue;
                }
                case ValueType.Color:
                {
                    Color convertedValue;
                    return ColorUtility.TryParseHtmlString(value, out convertedValue) && property.colorValue == convertedValue;
                }
            }

            return false;
        }

        bool SetPropertyTo(SerializedProperty property, string value)
        {
            switch (TargetValueType)
            {
                case ValueType.Decimal:
                {
                    float convertedTargetValue;
                    if (!float.TryParse(value, out convertedTargetValue))
                        return false;

                    property.floatValue = convertedTargetValue;
                    return true;
                }
                case ValueType.Integer:
                {
                    int convertedTargetValue;
                    if (!int.TryParse(value, out convertedTargetValue))
                        return false;

                    property.intValue = convertedTargetValue;
                    return true;
                }
                case ValueType.Text:
                {
                    property.stringValue = value;
                    return true;
                }
                case ValueType.Boolean:
                {
                    bool convertedTargetValue;
                    if (!bool.TryParse(value, out convertedTargetValue))
                        return false;
                    property.boolValue = convertedTargetValue;
                    return true;
                }
                case ValueType.Color:
                {
                    Color convertedTargetValue;
                    if (!ColorUtility.TryParseHtmlString(value, out convertedTargetValue))
                        return false;
                    property.colorValue = convertedTargetValue;
                    return true;
                }
            }
            return false;
        }

        bool SetPropertyToDifferentValueThan(SerializedProperty property, string value)
        {
            switch (TargetValueType)
            {
                case ValueType.Decimal:
                {
                    float convertedTargetValue;
                    if (!float.TryParse(value, out convertedTargetValue))
                        return false;

                    property.floatValue = convertedTargetValue + 1.0f;
                    return true;
                }
                case ValueType.Integer:
                {
                    int convertedTargetValue;
                    if (!int.TryParse(value, out convertedTargetValue))
                        return false;

                    property.intValue = convertedTargetValue + 1;
                    return true;
                }
                case ValueType.Text:
                {
                    property.stringValue = value + "different ";
                    return true;
                }
                case ValueType.Boolean:
                {
                    bool convertedTargetValue;
                    if (!bool.TryParse(value, out convertedTargetValue))
                        return false;
                    property.boolValue = !convertedTargetValue;
                    return true;
                }
                case ValueType.Color:
                {
                    Color convertedTargetValue;
                    if (!ColorUtility.TryParseHtmlString(value, out convertedTargetValue))
                        return false;
                    property.colorValue = convertedTargetValue + Color.gray;
                    return true;
                }
            }
            return false;
        }

        bool PropertyFulfillCriterion(UnityObject target, string propertyPath)
        {
            if (target == null)
                return false;

            if (m_TargetValueMode == ValueMode.TargetValue &&  m_TargetValueType != ValueType.Text && string.IsNullOrEmpty(m_TargetValue))
                return true;

            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(propertyPath);

            if (property == null)
                return false;

            if (!DoPropertyTypeMatches(property))
                return false;

            switch (m_TargetValueMode)
            {
                case ValueMode.TargetValue:
                    return DoesPropertyMatches(property, m_TargetValue);
                case ValueMode.DifferentThanInitial:
                    return !DoesPropertyMatches(property, m_InitialValue);
            }

            return false;
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            var target = m_Target.SceneObjectReference.ReferencedObject;
            if (target == null)
                return false;

            if (m_TargetValueMode == ValueMode.TargetValue && m_TargetValueType != ValueType.Text && string.IsNullOrEmpty(m_TargetValue))
                return false;

            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(m_PropertyPath);

            if (property == null)
                return false;

            if (!DoPropertyTypeMatches(property))
                return false;

            switch (m_TargetValueMode)
            {
                case ValueMode.TargetValue:
                {
                    if (!SetPropertyTo(property, TargetValue))
                        return false;
                    break;
                }
                case ValueMode.DifferentThanInitial:
                {
                    if (!SetPropertyToDifferentValueThan(property, m_InitialValue))
                        return false;
                    break;
                }
            }

            serializedObject.ApplyModifiedProperties();

            return true;
        }
    }
}
