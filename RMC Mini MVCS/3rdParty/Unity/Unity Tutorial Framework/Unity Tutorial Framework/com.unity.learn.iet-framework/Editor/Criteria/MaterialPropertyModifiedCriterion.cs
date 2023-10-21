using System;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking a Material's property modification.
    /// </summary>
    public class MaterialPropertyModifiedCriterion : Criterion
    {
        internal SceneObjectReference Target { get => m_Target.SceneObjectReference; set => m_Target.SceneObjectReference = value; }
        [SerializeField]
        ObjectReference m_Target = new ObjectReference();

        internal string MaterialPropertyPath { get => m_MaterialPropertyPath; set => m_MaterialPropertyPath = value; }
        [SerializeField]
        string m_MaterialPropertyPath = "";

        string m_InitialValue = null;

        static MaterialProperty FindProperty(string path, Material material)
        {
            UnityObject[] mats = new[] { material };
            return MaterialEditor.GetMaterialProperty(mats, path);
        }

        static string GetPropertyValueToString(MaterialProperty property)
        {
            switch (property.type)
            {
                case MaterialProperty.PropType.Color:
                    return property.colorValue.ToString();
                case MaterialProperty.PropType.Vector:
                    return property.vectorValue.ToString();
                case MaterialProperty.PropType.Float:
                    return property.floatValue.ToString();
                case MaterialProperty.PropType.Range:
                    return property.rangeLimits.ToString();
                case MaterialProperty.PropType.Texture:
                    return property.textureValue.GetInstanceID().ToString();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public override void StartTesting()
        {
            base.StartTesting();
            InitializeRequiredStateIfNeeded();

            EditorApplication.update += UpdateCompletion;
        }

        void InitializeRequiredStateIfNeeded()
        {
            if (m_InitialValue != null)
                return;

            if (string.IsNullOrEmpty(m_MaterialPropertyPath) || Target.ReferencedObject == null)
                return;

            var property = FindProperty(m_MaterialPropertyPath, (Material)Target.ReferencedObject);

            m_InitialValue = GetPropertyValueToString(property);
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
            m_InitialValue = null;
            EditorApplication.update -= UpdateCompletion;
        }

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            InitializeRequiredStateIfNeeded();

            if (m_InitialValue == null)
                return false;

            if (m_MaterialPropertyPath == null || Target.ReferencedObject == null)
                return false;

            var property = FindProperty(m_MaterialPropertyPath, (Material)Target.ReferencedObject);

            if (property == null)
                return false;

            var currentValue = GetPropertyValueToString(property);

            if (currentValue != m_InitialValue)
                return true;

            return false;
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            throw new NotImplementedException();
        }
    }
}
