using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(TutorialParagraph))]
    class TutorialParagraphDrawer : FlushChildrenDrawer
    {
        const string k_TypePath = "m_Type";
        const string k_TextPath = "m_Text";
        const string k_CriteriaPath = "m_Criteria";
        const string k_SummaryPath = "m_Summary";
        const string k_CompletionPath = "m_CriteriaCompletion";
        const string k_TutorialPath = "m_Tutorial";
        const string k_TutorialButtonTextPath = "m_TutorialButtonText";
        const string k_ImagePath = "m_Image";
        const string k_VideoPath = "m_Video";

        protected override void DisplayChildProperty(
            Rect position, SerializedProperty parentProperty, SerializedProperty childProperty, GUIContent label
        )
        {
            ParagraphType type = (ParagraphType)parentProperty.FindPropertyRelative(k_TypePath).intValue;
            switch (childProperty.name)
            {
                case k_TextPath:
                    if (type == ParagraphType.SwitchTutorial || type == ParagraphType.Image || type == ParagraphType.Video)
                        return;
                    break;
                case k_TutorialButtonTextPath:
                case k_TutorialPath:
                    if (type != ParagraphType.SwitchTutorial)
                        return;
                    break;
                case k_CriteriaPath:
                case k_CompletionPath:
                    if (type != ParagraphType.Instruction)
                        return;
                    break;
                case k_SummaryPath:
                    if ((type != ParagraphType.Instruction) && (type != ParagraphType.Narrative))
                        return;
                    break;
                case k_ImagePath:
                    if (type != ParagraphType.Image)
                        return;
                    break;
                case k_VideoPath:
                    if (type != ParagraphType.Video)
                        return;
                    break;
            }
            base.DisplayChildProperty(position, parentProperty, childProperty, label);
        }

        protected override float GetChildPropertyHeight(SerializedProperty parentProperty, SerializedProperty childProperty)
        {
            ParagraphType type = (ParagraphType)parentProperty.FindPropertyRelative(k_TypePath).intValue;
            switch (childProperty.name)
            {
                case k_TextPath:
                    if (type == ParagraphType.Image || type == ParagraphType.Video)
                        return -EditorGUIUtility.standardVerticalSpacing;
                    break;
                case k_CriteriaPath:
                    if (type != ParagraphType.Instruction)
                        return -EditorGUIUtility.standardVerticalSpacing;
                    break;
                case k_SummaryPath:
                    if ((type != ParagraphType.Instruction) && (type != ParagraphType.Narrative))
                        return -EditorGUIUtility.standardVerticalSpacing;
                    break;
                case k_ImagePath:
                    if (type != ParagraphType.Image)
                        return -EditorGUIUtility.standardVerticalSpacing;
                    break;
                case k_VideoPath:
                    if (type != ParagraphType.Video)
                        return -EditorGUIUtility.standardVerticalSpacing;
                    break;
            }
            return base.GetChildPropertyHeight(parentProperty, childProperty);
        }
    }
}
