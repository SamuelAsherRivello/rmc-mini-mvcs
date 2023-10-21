using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomEditor(typeof(TutorialContainer))]
    class TutorialContainerEditor : UnityEditor.Editor
    {
        readonly string[] k_PropertiesToHide =
        {
            "m_Script",
            nameof(TutorialContainer.Modified)  // this is not not something tutorial authors should subscribe to typically
        };

        TutorialContainer Target => (TutorialContainer)target;

        void OnEnable()
        {
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
            /* If this category is parented, we consider modifications to 'this'
            category also to be modifications of the parent. */
            if (Target.ParentContainer != null)
            {
                Target.ParentContainer.RaiseModified();
            }
        }

        UndoPropertyModification[] OnPostprocessModifications(UndoPropertyModification[] modifications)
        {
            var previousCategoryModification = modifications.Where(m => m.previousValue.propertyPath == nameof(Target.ParentContainer))
                                                            .FirstOrDefault();

            bool parentCategoryWasEdited = previousCategoryModification.previousValue != null;
            if (parentCategoryWasEdited)
            {
                /* If this category was parented, we consider modifications to 'this'
                category also to be modifications of the parent. */
                var previousCategory = previousCategoryModification.previousValue.objectReference as TutorialContainer;
                if (previousCategory != null)
                {
                    previousCategory.RaiseModified();
                }
            }

            Target.RaiseModified();
            /* If this category is parented, we consider modifications to 'this'
            category also to be modifications of the parent. */
            if (Target.ParentContainer != null)
            {
                Target.ParentContainer.RaiseModified();
            }
            return modifications;
        }

        public override void OnInspectorGUI()
        {
            TutorialProjectSettings.DrawDefaultAssetRestoreWarning();

            if (GUILayout.Button(Localization.Tr(MenuItems.ShowTutorials)))
            {
                // Make sure we will display 'this' container in the window.
                var window = Target.ProjectLayout != null ? TutorialWindow.GetOrCreateWindowAndLoadLayout(Target, true)
                                                          : TutorialWindow.GetOrCreateWindowNextToInspector();

                window.Broadcast(new CategoryClickedEvent(Target));
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
