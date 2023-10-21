using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    using static Localization;

    [CustomEditor(typeof(Tutorial))]
    class TutorialEditor : UnityEditor.Editor
    {
        static class Contents
        {
            public static GUIContent s_AutoCompletion = new GUIContent(Tr(LocalizationKeys.k_TutorialLabelAutoCompletion));
            public static GUIContent s_StartAutoCompletion = new GUIContent(Tr(LocalizationKeys.k_TutorialButtonStartAutoCompletion));
            public static GUIContent s_StopAutoCompletion = new GUIContent(Tr(LocalizationKeys.k_TutorialButtonStopAutoCompletion));
        }

        static readonly string[] k_PropsToIgnore = { "m_Script", nameof(Tutorial.m_LessonId) };
        static readonly string[] k_PropsToIgnoreNoScene = k_PropsToIgnore
            .Concat(new[] { nameof(Tutorial.m_SceneManagementBehavior) })
            .ToArray();

        static readonly string s_PagesPropertyPath = $"{nameof(Tutorial.m_Pages)}.m_Items";

        static readonly Regex s_MatchPagesPropertyPath =
            new Regex(
                string.Format("^({0}\\.Array\\.size)|(^({0}\\.Array\\.data\\[\\d+\\]))", Regex.Escape(s_PagesPropertyPath))
            );

        Tutorial Target => (Tutorial)target;

        [NonSerialized]
        string m_WarningMessage;

        protected virtual void OnEnable()
        {
            if (serializedObject.FindProperty(s_PagesPropertyPath) == null)
            {
                m_WarningMessage = string.Format(Tr(LocalizationKeys.k_MissingPropertyPathWarning), s_PagesPropertyPath);
            }

            Undo.postprocessModifications += OnPostprocessModifications;
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        protected virtual void OnDisable()
        {
            Undo.postprocessModifications -= OnPostprocessModifications;
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        void OnUndoRedoPerformed()
        {
            if (Target != null)
            {
                Target.RaiseModified();
            }
        }

        UndoPropertyModification[] OnPostprocessModifications(UndoPropertyModification[] modifications)
        {
            Target.RaiseModified();

            bool pagesChanged = false;

            foreach (var modification in modifications)
            {
                if (modification.currentValue.target != target)
                {
                    continue;
                }

                string propertyPath = modification.currentValue.propertyPath;
                if (s_MatchPagesPropertyPath.IsMatch(propertyPath))
                {
                    pagesChanged = true;
                    break;
                }
            }

            if (pagesChanged)
            {
                Target.RaiseModified();
            }

            return modifications;
        }

        public override void OnInspectorGUI()
        {
            TutorialProjectSettings.DrawDefaultAssetRestoreWarning();

            if (!string.IsNullOrEmpty(m_WarningMessage))
            {
                EditorGUILayout.HelpBox(m_WarningMessage, MessageType.Warning);
            }

            if (SerializedTypeDrawer.UseDefaultEditors)
            {
                base.OnInspectorGUI();
            }
            else
            {
                serializedObject.Update();

                // Scene management options are visible only when we have no scenes specified.
                DrawPropertiesExcluding(serializedObject, Target.HasScenes() ? k_PropsToIgnoreNoScene : k_PropsToIgnore);

                serializedObject.ApplyModifiedProperties();
            }

            // Auto completion
            GUILayout.Label(Contents.s_AutoCompletion, EditorStyles.boldLabel);
            using (new EditorGUI.DisabledScope(Target.IsCompleted))
            {
                if (GUILayout.Button(Target.IsAutoCompleting ? Contents.s_StopAutoCompletion : Contents.s_StartAutoCompletion))
                {
                    if (Target.IsAutoCompleting)
                    {
                        Target.StopAutoCompletion();
                    }
                    else
                    {
                        Target.StartAutoCompletion();
                    }
                }
            }
        }
    }
}
