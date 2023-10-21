using System;
using System.Collections.Generic;
using static Unity.Tutorials.Core.Editor.TutorialContainer;

namespace Unity.Tutorials.Core.Editor
{
    internal class AppEvent { }
    internal class CategoriesRefreshRequestedEvent : AppEvent { }
    internal class CategoryClickedEvent : AppEvent
    {
        public TutorialContainer Category { get; private set; }

        public CategoryClickedEvent(TutorialContainer category)
        {
            Category = category;
        }
    }

    internal class BackButtonClickedEvent : AppEvent { }

    internal class SectionClickedEvent : AppEvent
    {
        public Section Section { get; private set; }

        public SectionClickedEvent(Section section)
        {
            Section = section;
        }
    }

    internal class TutorialStartRequestedEvent : AppEvent
    {
        public Tutorial Tutorial { get; private set; }
        public Tutorial PreviousTutorial { get; private set; }

        public TutorialStartRequestedEvent(Tutorial tutorial, Tutorial previousTutorial)
        {
            Tutorial = tutorial;
            PreviousTutorial = previousTutorial;
        }
    }

    internal class TutorialQuitEvent : AppEvent { }
    internal class TutorialNavigationEvent : AppEvent
    {
        public bool MoveToNextPage { get; private set; }

        public TutorialNavigationEvent(bool moveToNextPage)
        {
            MoveToNextPage = moveToNextPage;
        }
    }
    internal class DomainReloadOccurredEvent : AppEvent { }
    internal class TutorialsCompletionStatusUpdatedEvent : AppEvent { }

    /// <summary>
    /// A simple Event System that can be used for remote systems communication
    /// </summary>
    internal class EventManager
    {
        readonly Dictionary<Type, Action<AppEvent>> s_Events = new Dictionary<Type, Action<AppEvent>>();
        readonly Dictionary<Delegate, Action<AppEvent>> s_EventLookups = new Dictionary<Delegate, Action<AppEvent>>();

        public void AddListener<T>(Action<T> evt) where T : AppEvent
        {
            if (s_EventLookups.ContainsKey(evt)) { return; }

            Action<AppEvent> newAction = (e) => evt((T)e);
            s_EventLookups[evt] = newAction;

            if (s_Events.TryGetValue(typeof(T), out Action<AppEvent> internalAction))
            {
                s_Events[typeof(T)] = internalAction += newAction;
            }
            else
            {
                s_Events[typeof(T)] = newAction;
            }
        }

        public void RemoveListener<T>(Action<T> evt) where T : AppEvent
        {
            if (!s_EventLookups.TryGetValue(evt, out var action)) { return; }

            if (s_Events.TryGetValue(typeof(T), out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                    s_Events.Remove(typeof(T));
                else
                    s_Events[typeof(T)] = tempAction;
            }

            s_EventLookups.Remove(evt);
        }

        public void Broadcast(AppEvent evt)
        {
            if (s_Events.TryGetValue(evt.GetType(), out var action))
                action.Invoke(evt);
        }

        public void Clear()
        {
            s_Events.Clear();
            s_EventLookups.Clear();
        }
    }
}
