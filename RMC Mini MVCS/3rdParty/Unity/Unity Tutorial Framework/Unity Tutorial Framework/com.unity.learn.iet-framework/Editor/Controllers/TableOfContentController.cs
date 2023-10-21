using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    internal class TableOfContentController : Controller
    {
        TableOfContentModel m_Model => Application.Model.TableOfContent;
        TableOfContentView m_View => Application.TableOfContentView;

        internal TableOfContentController()
        {
            SetupCategories();

            AddListener<CategoriesRefreshRequestedEvent>(OnCategoriesRefreshRequested);
            AddListener<CategoryClickedEvent>(OnCategoryClicked);
            AddListener<SectionClickedEvent>(OnSectionClicked);
            AddListener<BackButtonClickedEvent>(OnBackButtonClicked);
            AddListener<TutorialsCompletionStatusUpdatedEvent>(OnTutorialsCompletionStatusUpdated);
        }

        internal override void RemoveListeners()
        {
            RemoveListener<CategoriesRefreshRequestedEvent>(OnCategoriesRefreshRequested);
            RemoveListener<CategoryClickedEvent>(OnCategoryClicked);
            RemoveListener<SectionClickedEvent>(OnSectionClicked);
            RemoveListener<BackButtonClickedEvent>(OnBackButtonClicked);
            RemoveListener<TutorialsCompletionStatusUpdatedEvent>(OnTutorialsCompletionStatusUpdated);
        }

        void OnTutorialsCompletionStatusUpdated(TutorialsCompletionStatusUpdatedEvent evt)
        {
            if (Application.CurrentView != m_View.Name)
            {
                return;
            }
            m_View.Refresh();
        }

        void SetupCategories()
        {
            var allCategories = TutorialFrameworkModel.s_AreTestsRunning ? TableOfContentModel.CategoriesOfProjectDuringTests
                                                                         : TutorialEditorUtils.FindAssets<TutorialContainer>();
            var rootCategories = allCategories.Where(category => category.ParentContainer is null);

            TutorialContainer defaultCategory = rootCategories.FirstOrDefault();

            /* If we have more than one root container, we show a selection view. Exactly one (or zero) container
            is set active immediately without possibility to return to the the selection view. */
            m_Model.RootCategoriesOfProject = rootCategories;
            if (rootCategories.Count() < 2)
            {
                m_Model.CurrentCategory = defaultCategory;
            }

            foreach (var category in allCategories)
            {
                category.Modified.RemoveListener(OnTutorialCategoryModified);
                category.Modified.AddListener(OnTutorialCategoryModified);
            }

            m_Model.FetchAllTutorialStates();
        }

        void OnTutorialCategoryModified(TutorialContainer category)
        {
            if (Application == null
            || Application.CurrentView != m_View.Name)
            {
                return;
            }

            if (m_Model.CurrentCategory == category
            || m_Model.CurrentCategory == category.ParentContainer)
            {
                m_View.Refresh();
            }
        }

        void OnCategoriesRefreshRequested(CategoriesRefreshRequestedEvent evt)
        {
            SetupCategories();
        }

        void OnCategoryClicked(CategoryClickedEvent evt)
        {
            EnterCategory(evt.Category);
        }

        void EnterCategory(TutorialContainer category)
        {
            if (m_Model.CurrentCategory == category) { return; }
            m_Model.CurrentCategory = category;
            m_View.Refresh();
        }

        void OnBackButtonClicked(BackButtonClickedEvent evt)
        {
            ExitCategory();
        }

        void ExitCategory()
        {
            if (m_Model.CurrentCategory && m_Model.CurrentCategory.ParentContainer)
            {
                m_Model.CurrentCategory = m_Model.CurrentCategory.ParentContainer;
            }
            else
            {
                m_Model.CurrentCategory = null;
            }
            m_View.Refresh();
        }

        void OnSectionClicked(SectionClickedEvent evt)
        {
            if (evt.Section.IsTutorial)
            {
                StartTutorial(evt.Section.Tutorial);
                return;
            }
            evt.Section.OpenUrl();
        }

        void StartTutorial(Tutorial tutorial)
        {
            Application.Broadcast(new TutorialStartRequestedEvent(tutorial, null));
        }
    }
}
