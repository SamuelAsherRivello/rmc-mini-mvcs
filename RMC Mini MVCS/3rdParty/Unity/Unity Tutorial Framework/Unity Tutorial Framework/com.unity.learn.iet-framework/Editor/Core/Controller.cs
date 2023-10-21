using System;

namespace Unity.Tutorials.Core.Editor
{
    internal abstract class Controller
    {
        protected TutorialWindow Application => TutorialWindow.Instance;

        /// <summary>
        /// Subscribes to an AppEvent
        /// </summary>
        /// <param name="evt">Callback for an AppEvent</param>
        internal void AddListener<T>(Action<T> evt) where T : AppEvent
        {
            Application.EventManager.AddListener(evt);
        }

        internal void RemoveListener<T>(Action<T> evt) where T : AppEvent
        {
            Application.EventManager.RemoveListener(evt);
        }

        internal abstract void RemoveListeners();
    }
}
