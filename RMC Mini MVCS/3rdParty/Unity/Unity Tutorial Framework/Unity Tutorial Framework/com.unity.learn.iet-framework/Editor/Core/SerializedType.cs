using System;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Used to serialize System.Type using Type.AssemblyQualifiedName.
    /// </summary>
    [Serializable]
    public class SerializedType : ISerializationCallbackReceiver
    {
        static readonly bool k_IsAuthoringMode = ProjectMode.IsAuthoringMode();

        /// <summary>
        /// Is the type specified, meaning, the assembly-qualified type name is stored.
        /// </summary>
        internal bool IsSpecified => m_TypeName.IsNotNullOrEmpty();

        [SerializeField]
        internal string m_TypeName;

        /// <summary>
        /// The Type is stored using its assembly-qualified type name.
        /// </summary>
        public Type Type
        {
            get { return string.IsNullOrEmpty(m_TypeName) ? null : Type.GetType(m_TypeName); }
            set { m_TypeName = value == null ? "" : value.AssemblyQualifiedName; }
        }

        /// <summary>
        /// Constructs with a type.
        /// </summary>
        /// <param name="type"></param>
        public SerializedType(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// UnityEngine.ISerializationCallbackReceiver override, do not call.
        /// </summary>
        public void OnBeforeSerialize()
        {
        }

        /// <summary>
        /// UnityEngine.ISerializationCallbackReceiver override, do not call.
        /// </summary>
        public void OnAfterDeserialize()
        {
            // TODO what's this? Is this needed?
            // Remove "-testable" suffix from assembly names
            if (!k_IsAuthoringMode && IsSpecified)
            {
                m_TypeName = m_TypeName.Replace("Assembly-CSharp-Editor-firstpass-testable", "Assembly-CSharp-Editor-firstpass");
                m_TypeName = m_TypeName.Replace("Assembly-CSharp-Editor-testable", "Assembly-CSharp-Editor");
                m_TypeName = m_TypeName.Replace("Assembly-CSharp-firstpass-testable", "Assembly-CSharp-firstpass");
                m_TypeName = m_TypeName.Replace("Assembly-CSharp-testable", "Assembly-CSharp");
            }

            // Backwards-compatibility for IET < 2.0 when the namespace and assemblies were Unity.InteractiveTutorials.*
            // instead of Unity.Tutorials.Core(.Editor).
            if (IsSpecified)
            {
                m_TypeName = m_TypeName.Replace("Unity.InteractiveTutorials.Core", "Unity.Tutorials.Core.Editor");
                m_TypeName = m_TypeName.Replace("Unity.InteractiveTutorials", "Unity.Tutorials.Core.Editor");
            }

            // TODO figure out how to log these issues: we don't want to log warnings/errors always, and especially
            // without showing which is the asset causing the warnings/errors, as sometimes the author might want to keep
            // missing types when authoring assets for a project which supports multiple Unity versions.
            //if (ProjectMode.IsAuthoringMode() && IsSpecified)
            //{
            //    if (Type == null)
            //        Debug.LogError($"Type name '{m_TypeName}' was specified but it could not been resolved into a run-time Type.");
            //    else if (Type.AssemblyQualifiedName != m_TypeName)
            //        Debug.LogWarning($"Type name '{m_TypeName}' was resolved into a run-time Type '{Type}' but with a different type name '{Type.AssemblyQualifiedName}'");
            //}
        }
    }
}
