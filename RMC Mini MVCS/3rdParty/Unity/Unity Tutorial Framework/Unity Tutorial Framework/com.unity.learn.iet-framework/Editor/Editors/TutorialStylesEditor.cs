using UnityEditor;

namespace Unity.Tutorials.Core.Editor
{
    [CustomEditor(typeof(TutorialStyles))]
    class TutorialStylesEditor : UnityEditor.Editor
    {
        readonly string[] k_PropertiesToHide = { "m_Script" };

        public override void OnInspectorGUI()
        {
            TutorialProjectSettings.DrawDefaultAssetRestoreWarning();

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
