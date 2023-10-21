using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomEditor(typeof(TutorialProjectSettings))]
    class TutorialProjectSettingsEditor : UnityEditor.Editor
    {
        readonly string[] k_PropertiesToHide = { "m_Script" };

        TutorialProjectSettings Target => (TutorialProjectSettings)target;

        public override void OnInspectorGUI()
        {
            TutorialProjectSettings.DrawDefaultAssetRestoreWarning();

            if (GUILayout.Button(Localization.Tr(LocalizationKeys.k_ButtonRunStartupCode)))
            {
                UserStartupCode.RunStartupCode(Target);
            }

            EditorGUILayout.Space(10);

            if (SerializedTypeDrawer.UseDefaultEditors)
            {
                base.OnInspectorGUI();
            }
            else
            {
                serializedObject.Update();
                DrawPropertiesExcluding(serializedObject, k_PropertiesToHide);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
