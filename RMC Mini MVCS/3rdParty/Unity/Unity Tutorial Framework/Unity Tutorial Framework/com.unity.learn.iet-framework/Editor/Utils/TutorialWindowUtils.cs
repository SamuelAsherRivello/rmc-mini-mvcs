namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Wrapper class for TutorialWindow public APIs
    /// </summary>
    public static class TutorialWindowUtils
    {
        /// <summary>
        /// The currently active tutorial, if any.
        /// </summary>
        public static Tutorial CurrentTutorial => TutorialWindow.Instance.CurrentTutorial;

        /// <summary>
        /// Are we currently (during this frame) transitioning from one tutorial to another?
        /// </summary>
        /// <remarks>
        /// This transition typically happens when using a Switch Tutorial button on a tutorial page.
        /// </remarks>
        public static bool IsTransitioningBetweenTutorials => TutorialWindow.Instance.IsTransitioningBetweenTutorials;

        /// <summary>
        /// Are we currently loading a window layout.
        /// </summary>
        /// <remarks>
        /// A window layout load typically happens when the project is started for the first time
        /// and the project's startup settings specify a window layout for the project, or when entering
        /// or exiting a tutorial with a window layout specified.
        /// </remarks>
        public static bool IsLoadingLayout => TutorialWindow.s_IsLoadingLayout;

        /// <summary>
        /// Starts a tutorial.
        /// </summary>
        /// <param name="tutorial">The tutorial to be started.</param>
        /// <remarks>
        /// The caller of the funtion is responsible for positioning the TutorialWindow for the tutorials.
        /// If no TutorialWindow is visible, it is created and shown as a free-floating window.
        /// If the currently active scene has unsaved changes, the user is asked to save them.
        /// If we are in Play Mode, it is exited.
        /// Note that currently there is no explicit way to quit a tutorial. Instead, a tutorial should be quit either
        /// by user interaction or by closing the TutorialWindow programmatically.
        /// </remarks>
        public static void StartTutorial(Tutorial tutorial)
        {
            TutorialWindow.StartTutorial(tutorial);
        }

        /// <summary>
        /// Clear localization table cache
        /// </summary>
        public static void ClearLocalizationCache()
        {
            TutorialWindow.ClearLocalizationCache();
        }
    }
}
