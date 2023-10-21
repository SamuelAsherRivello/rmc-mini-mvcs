using JetBrains.Annotations;
using System;
using UnityEditor.SettingsManagement;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Holds all data of the application that the controller needs to access or that should be exposed
    /// </summary>
    [Serializable]
    internal class TutorialFrameworkModel : IModel
    {
        /// <summary>
        /// Should we show the Close Tutorials info dialog for the user for the current project.
        /// By default the dialog is shown once per project and disabled after that.
        /// </summary>
        /// <remarks>
        /// You want to set this typically to false when running unit tests.
        /// </remarks>
        internal static UserSetting<bool> s_ShowTutorialsWindowClosedDialog = new UserSetting<bool>("IET.ShowTutorialsWindowClosedDialog", Localization.Tr(LocalizationKeys.k_SettingsShowTutorialsWindowClosedDialog), true);
        internal static ProjectSetting<bool> s_DisplayWelcomeDialogOnStartup = new ProjectSetting<bool>("IET.DisplayWelcomeDialogOnStartup", Localization.Tr(LocalizationKeys.k_SettingsDisplayWelcomeDialogOnStartup), true);

        internal TableOfContentModel TableOfContent => Application.GetModel(typeof(TableOfContentModel)) as TableOfContentModel;
        internal TutorialModel Tutorial => Application.GetModel(typeof(TutorialModel)) as TutorialModel;

        TutorialWindow Application => TutorialWindow.Instance;
        internal static bool s_AreTestsRunning;
        /// <summary>
        /// Only used for test purposes
        /// </summary>
        internal bool AreTestsRunning;

        internal bool IsOpen;
        internal bool DomainReloadOccured;
        internal string CurrentView;
        internal string PreviousView;

        /// <inheritdoc />
        public event Action StateChanged;

        /// <inheritdoc />
        public void OnStart() { }

        /// <inheritdoc />
        public void OnStop() { }

        /// <inheritdoc />
        public void RestoreState([NotNull] IWindowCache cache)
        {
            CurrentView = cache.CurrentView;
            PreviousView = cache.PreviousView;
            DomainReloadOccured = cache.DomainReloadOccured;

            IsOpen = cache.IsOpen;
            s_AreTestsRunning = cache.AreTestsRunning || s_AreTestsRunning;

            StateChanged?.Invoke();
        }

        /// <inheritdoc />
        public void SaveState([NotNull] IWindowCache cache)
        {
            if (TutorialWindow.s_IsLoadingLayout)
            {
                CurrentView = string.Empty;
                cache.CurrentView = string.Empty;
            }
            else
            {
                if (CurrentView == TutorialView.k_Name) //special case: we don't want to remember the Tutorial view, as it is initialized by the controller to ensure Page-related events are fired in a consistent order
                {
                    cache.CurrentView = string.Empty;
                }
                else
                {
                    cache.CurrentView = CurrentView;
                }
            }
            cache.PreviousView = PreviousView;
            cache.DomainReloadOccured = DomainReloadOccured;
            cache.IsOpen = IsOpen;
            cache.AreTestsRunning = s_AreTestsRunning;
        }
    }
}
