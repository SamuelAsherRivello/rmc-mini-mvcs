using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(LocalizableString), true)]
    class LocalizableStringDrawer : PropertyDrawer
    {
        static GUIContent s_IconContent;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var origIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var value = property.FindPropertyRelative(LocalizableString.PropertyPath);
            EditorGUI.PropertyField(position, value, GUIContent.none);
            position.x -= 25;

            EditorGUI.LabelField(position, IconContent());
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = origIndentLevel;
        }

        internal static GUIContent IconContent()
        {
            if (s_IconContent == null)
            {
                s_IconContent = EditorGUIUtility.IconContent("console.infoicon.sml"); // TODO create and use a proper localization/translation icon
            }
            s_IconContent.tooltip = Localization.Tr(LocalizationKeys.k_LocalizableStringIconTooltip);
            return s_IconContent;
        }
    }

    // Copy-paste-modified from TextAreaDrawer
    [CustomPropertyDrawer(typeof(LocalizableTextAreaAttribute))]
    class LocalizableTextAreaDrawer : PropertyDrawer
    {
        const int k_SingleLineHeight = 13;
        const int k_IconSize = 18;

        Vector2 m_ScrollPosition;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);

                Rect labelPosition = EditorGUI.IndentedRect(position);
                labelPosition.height = k_IconSize;
                position.yMin += labelPosition.height;
                EditorGUI.HandlePrefixLabel(position, labelPosition, label);

                var iconPos = labelPosition;
                iconPos.height = k_IconSize;
                iconPos.x += labelPosition.width - k_IconSize;
                EditorGUI.LabelField(iconPos, LocalizableStringDrawer.IconContent());

                var value = property.FindPropertyRelative(LocalizableString.PropertyPath);
                EditorGUI.BeginChangeCheck();
                string newValue = EditorGUIProxy.ScrollableTextAreaInternal(position, value.stringValue, ref m_ScrollPosition, EditorStyles.textArea);
                if (EditorGUI.EndChangeCheck())
                    value.stringValue = newValue;

                EditorGUI.EndProperty();
            }
            //else
            //    EditorGUI.LabelField(position, label.text, "Use TextAreaDrawer with string.");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var textAreaAttribute = attribute as LocalizableTextAreaAttribute;
            var value = property.FindPropertyRelative(LocalizableString.PropertyPath);
            string text = value.stringValue;

            float fullTextHeight = EditorStyles.textArea.CalcHeight(GUIContent_Temp(text), EditorGUIProxy.contextWidth);
            int lines = Mathf.CeilToInt(fullTextHeight / k_SingleLineHeight);

            lines = Mathf.Clamp(lines, textAreaAttribute.MinLines, textAreaAttribute.MaxLines);

            return k_SingleLineHeight // header
                + k_SingleLineHeight // first line
                + (lines - 1) * k_SingleLineHeight; // remaining lines
        }

        internal static GUIContent GUIContent_Temp(string t)
        {
            s_Text.text = t;
            s_Text.tooltip = string.Empty;
            return s_Text;
        }

        static readonly GUIContent s_Text = new GUIContent();
    }
}
