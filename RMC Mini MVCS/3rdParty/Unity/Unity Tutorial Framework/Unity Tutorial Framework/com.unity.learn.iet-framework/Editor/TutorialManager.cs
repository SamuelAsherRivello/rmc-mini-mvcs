using System;
using System.Linq;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Manages the startup and transitions of tutorials.
    /// </summary>
    [Obsolete("Will be removed in v4. All functionality that was previously managed by this class has been distributed in TutorialWindow and its components")] //todo: remove in v4
    public class TutorialManager : ScriptableObject
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        [Obsolete("Will be removed in v4. To interact with tutorials, use TutorialWindowUtils's APIs instead.")] //todo: remove in v4
        public static TutorialManager Instance
        {
            get
            {
                if (s_TutorialManager == null)
                {
                    s_TutorialManager = Resources.FindObjectsOfTypeAll<TutorialManager>().FirstOrDefault();
                    if (s_TutorialManager == null)
                    {
                        s_TutorialManager = CreateInstance<TutorialManager>();
                        s_TutorialManager.hideFlags = HideFlags.HideAndDontSave;
                    }
                }

                return s_TutorialManager;
            }
        }

        //todo: remove in v4
        static TutorialManager s_TutorialManager;

        /// <summary>
        /// The currently active tutorial, if any.
        /// </summary>
        [Obsolete("Will be removed in v4. Use TutorialWindowUtils.CurrentTutorial instead.")] //todo: remove in v4
        public Tutorial ActiveTutorial => TutorialWindow.Instance.Model.Tutorial.CurrentTutorial;

        /// <summary>
        /// Are we currently (during this frame) transitioning from one tutorial to another.
        /// </summary>
        /// <remarks>
        /// This transition typically happens when using a Switch Tutorial button on a tutorial page.
        /// </remarks>
        [Obsolete("Will be removed in v4. Use TutorialWindowUtils.IsTransitioningBetweenTutorials instead.")] //todo: remove in v4
        public bool IsTransitioningBetweenTutorials => TutorialWindow.Instance.Model.Tutorial.IsTransitioningBetweenTutorials;

        /// <summary>
        /// Are we currently loading a window layout.
        /// </summary>
        /// <remarks>
        /// A window layout load typically happens when the project is started for the first time
        /// and the project's startup settings specify a window layout for the project, or when entering
        /// or exiting a tutorial with a window layout specified.
        /// </remarks>
        [Obsolete("Will be removed in v4. Use TutorialWindowUtils.s_IsLoadingLayout instead.")] //todo: remove in v4
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
        [Obsolete("Will be removed in v4. Use TutorialWindowUtils.StartTutorial() instead")] //todo: remove in v4
        public void StartTutorial(Tutorial tutorial)
        {
            TutorialWindow.StartTutorial(tutorial);
        }
    }
}
