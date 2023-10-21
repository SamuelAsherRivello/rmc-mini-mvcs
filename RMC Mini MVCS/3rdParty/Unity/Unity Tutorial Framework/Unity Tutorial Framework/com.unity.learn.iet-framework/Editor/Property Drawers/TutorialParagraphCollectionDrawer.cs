using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(TutorialParagraphCollection))]
    class TutorialParagraphCollectionDrawer : CollectionWrapperDrawer
    {
        const string k_TypePath = "m_Type";
        const string k_TextPath = "Text.m_Untranslated";
        const string k_SummaryPath = "m_Summary";
        const string k_CriteriaPath = "m_Criteria.m_Items";


        protected override void OnReorderableListCreated(ReorderableList list)
        {
            base.OnReorderableListCreated(list);
            list.onAddCallback = delegate(ReorderableList lst) {
                ++lst.serializedProperty.arraySize;
                lst.serializedProperty.serializedObject.ApplyModifiedProperties();
                var lastElement = lst.serializedProperty.GetArrayElementAtIndex(lst.serializedProperty.arraySize - 1);
                lastElement.FindPropertyRelative(k_TypePath).intValue = 0;
                lastElement.FindPropertyRelative(k_TextPath).stringValue = "";
                lastElement.FindPropertyRelative(k_SummaryPath).stringValue = "";
                lastElement.FindPropertyRelative(k_CriteriaPath).arraySize = 0;
                list.serializedProperty.serializedObject.ApplyModifiedProperties();
            };
        }
    }
}
