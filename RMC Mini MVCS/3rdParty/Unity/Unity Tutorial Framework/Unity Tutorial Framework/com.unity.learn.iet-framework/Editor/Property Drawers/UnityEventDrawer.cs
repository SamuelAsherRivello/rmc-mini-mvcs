using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Used to work around the fact that the default UnityEventDrawer doesn't support tooltips for the header content.
    /// </summary>
    [CustomPropertyDrawer(typeof(UnityEventBase), true)]
    class UnityEventDrawer : UnityEditorInternal.UnityEventDrawer
    {
        GUIContent m_Label;

        protected override void DrawEventHeader(Rect headerRect)
        {
            headerRect.height = EditorGUIUtility.singleLineHeight;
            GUI.Label(headerRect, m_Label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            m_Label = label;
            base.OnGUI(position, property, label);
        }
    }
}
