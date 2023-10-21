using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Used the select which GUI controls are unmasked.
    /// </summary>
    [Serializable]
    public class GuiControlSelector
    {
        /// <summary>
        /// Supported selector modes.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Select by GUIContent (IMGUI).
            /// </summary>
            GuiContent,
            /// <summary>
            /// Select by Named Control's name, the name used for GUI.SetNextControlName() (IMGUI).
            /// </summary>
            NamedControl,
            /// <summary>
            /// Select by property's path name (IMGUI).
            /// </summary>
            Property,
            /// <summary>
            /// Select by GUIStyle's name (IMGUI).
            /// </summary>
            GuiStyleName,
            /// <summary>
            /// Match by the referenced Unity Object (IMGUI).
            /// </summary>
            ObjectReference,
            /// <summary>
            /// Select by VisualElement's name and class name (UI Toolkit).
            /// </summary>
            VisualElement,
        }

        /// <summary>
         /// Supported selector match types.
        /// </summary>
        public enum MatchType
        {
            /// <summary>
            /// Select the last matching control.
            /// </summary>
            Last,
            /// <summary>
            /// Select the first matching control.
            /// </summary>
            First,
            /// <summary>
            /// Select all matching controls.
            /// </summary>
            All,
        }

        /// <summary>
        /// The mode by which the UI element is chosen.
        /// </summary>
        public Mode SelectorMode { get => m_SelectorMode; set => m_SelectorMode = value; }
        [SerializeField]
        Mode m_SelectorMode;

        /// <summary>
        /// The used match type in case of the selector matches multiple UI elements.
        /// </summary>
        public MatchType SelectorMatchType { get => m_SelectorMatchType; set => m_SelectorMatchType = value; }
        [SerializeField]
        internal MatchType m_SelectorMatchType;

        /// <summary>
        /// Applicable if Mode.GuiContent used.
        /// </summary>
        public GUIContent GuiContent { get => new GUIContent(m_GUIContent); set => m_GUIContent = new GUIContent(value); }
        [SerializeField]
        GUIContent m_GUIContent = new GUIContent();

        /// <summary>
        /// Applicable if Mode.NamedControl used.
        /// </summary>
        public string ControlName { get => m_ControlName; set => m_ControlName = value ?? ""; }
        [SerializeField]
        string m_ControlName = "";

        /// <summary>
        /// Applicable if Mode.Property used.
        /// </summary>
        public string PropertyPath { get => m_PropertyPath; set => m_PropertyPath = value ?? ""; }
        [SerializeField]
        string m_PropertyPath = "";

        /// <summary>
        /// Applicable if Mode.Property used.
        /// </summary>
        public Type TargetType { get => m_TargetType.Type; set => m_TargetType.Type = value; }
        [SerializeField, SerializedTypeFilter(typeof(UnityObject), false)]
        SerializedType m_TargetType = new SerializedType(null);

        /// <summary>
        /// Applicable if Mode.GuiStyleName used.
        /// </summary>
        public string GuiStyleName { get => m_GUIStyleName; set => m_GUIStyleName = value; }
        [SerializeField]
        string m_GUIStyleName;

        /// <summary>
        /// A reference to a Unity Object of which name will be matched against the text in UI elements.
        /// Applicable if Mode.ObjectReference used.
        /// </summary>
        /// <remarks>
        /// In order for this to work for assets, the asset must have a short name, i.e.,
        /// the name cannot be visible in the UI in shortened form, e.g. "A longer...".
        /// </remarks>
        public ObjectReference ObjectReference { get => m_ObjectReference; set => m_ObjectReference = value; }
        [SerializeField]
        ObjectReference m_ObjectReference;

        /// <summary>
        /// Unity style sheet class name. Applicable if Mode.VisualElement used.
        /// </summary>
        public string VisualElementClassName { get => m_VisualElementClassName; set => m_VisualElementClassName = value; }
        [Tooltip("Unity style sheet class name."), SerializeField]
        internal string m_VisualElementClassName;

        /// <summary>
        /// The name of the element. Applicable if Mode.VisualElement used.
        /// </summary>
        public string VisualElementName { get => m_VisualElementName; set => m_VisualElementName = value; }
        [Tooltip("The name of the element."), SerializeField]
        internal string m_VisualElementName;

        /// <summary>
        /// The fully qualified C# class/type name of the element. Applicable if Mode.VisualElement used.
        /// </summary>
        public string VisualElementTypeName { get => m_VisualElementTypeName; set => m_VisualElementTypeName = value; }
        [Tooltip("The fully qualified C# class/type name of the element."), SerializeField]
        internal string m_VisualElementTypeName;
    }
}
