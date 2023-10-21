using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomEditor(typeof(TutorialWelcomePage))]
    class TutorialWelcomePageEditor : UnityEditor.Editor
    {
        readonly string[] k_PropsToIgnore = { "m_Script" };
        TutorialWelcomePage Target => (TutorialWelcomePage)target;
        SerializedProperty m_Buttons;
        SerializedProperty m_CurrentEvent;
        const string k_Buttons = "m_Buttons";
        const string k_OnClickEventPropertyPath = "OnClick";

        void OnEnable()
        {
            InitializeSerializedProperties();
            Undo.postprocessModifications += OnPostprocessModifications;
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        void OnDisable()
        {
            Undo.postprocessModifications -= OnPostprocessModifications;
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        void OnUndoRedoPerformed()
        {
            Target.RaiseModified();
        }

        UndoPropertyModification[] OnPostprocessModifications(UndoPropertyModification[] modifications)
        {
            Target.RaiseModified();
            return modifications;
        }

        void InitializeSerializedProperties()
        {
            m_Buttons = serializedObject.FindProperty(k_Buttons);
        }

        public override void OnInspectorGUI()
        {
            TutorialProjectSettings.DrawDefaultAssetRestoreWarning();

            if (GUILayout.Button(Localization.Tr(LocalizationKeys.k_TutorialWelcomePageButtonShowDialog)))
            {
                TutorialModalWindow.Show(Target);
            }

            GUILayout.Space(10);

            if (SerializedTypeDrawer.UseDefaultEditors)
            {
                base.OnInspectorGUI();
            }
            else
            {
                serializedObject.Update();

                DrawPropertiesExcluding(serializedObject, k_PropsToIgnore);

                bool eventOffOrRuntimeOnlyExists = false;
                for (int i = 0; i < m_Buttons.arraySize; i++)
                {
                    m_CurrentEvent = m_Buttons.GetArrayElementAtIndex(i).FindPropertyRelative(k_OnClickEventPropertyPath);
                    if (!TutorialEditorUtils.EventIsNotInState(m_CurrentEvent, UnityEngine.Events.UnityEventCallState.EditorAndRuntime))
                        continue;

                    eventOffOrRuntimeOnlyExists = true;
                    break;
                }
                if (eventOffOrRuntimeOnlyExists)
                {
                    TutorialEditorUtils.RenderEventStateWarning();
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
