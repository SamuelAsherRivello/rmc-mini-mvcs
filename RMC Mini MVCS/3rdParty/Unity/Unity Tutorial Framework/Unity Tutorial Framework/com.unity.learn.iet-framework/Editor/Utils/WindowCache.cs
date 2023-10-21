using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    internal interface IWindowCache
    {
        void Clear();
        string CurrentView { get; set; }
        string PreviousView { get; set; }
        bool DomainReloadOccured { get; set; }
        bool IsOpen { get; set; }
        bool AreTestsRunning { get; set; }
        bool PlayModeChanging { get; set; }
        Tutorial CurrentTutorial { get; set; }
        List<SceneInfo> ActiveScenesBeforeTutorialStarted { get; set; }
        SceneViewState SceneViewStateBeforeTutorialStarted { get; set; }
    }

    [Location("Cache/Window.yml", LocationAttribute.Location.LibraryFolder)]
    internal class WindowCache : ScriptableObjectSingleton<WindowCache>, IWindowCache
    {
        public event Action<IWindowCache> BeforeSerialize;

        public void Serialize()
        {
            BeforeSerialize?.Invoke(this);
            Save();
        }

        public void Clear()
        {
            CurrentView = default;
            PreviousView = default;
            DomainReloadOccured = default;
            IsOpen = default;
            AreTestsRunning = default;
            PlayModeChanging = default;
            CurrentTutorial = default;
            ActiveScenesBeforeTutorialStarted = default;
            SceneViewStateBeforeTutorialStarted = default;
        }

        string IWindowCache.CurrentView
        {
            get => CurrentView;
            set => CurrentView = value;
        }

        string IWindowCache.PreviousView
        {
            get => PreviousView;
            set => PreviousView = value;
        }

        bool IWindowCache.DomainReloadOccured
        {
            get => DomainReloadOccured;
            set => DomainReloadOccured = value;
        }

        bool IWindowCache.IsOpen
        {
            get => IsOpen;
            set => IsOpen = value;
        }

        bool IWindowCache.AreTestsRunning
        {
            get => AreTestsRunning;
            set => AreTestsRunning = value;
        }

        Tutorial IWindowCache.CurrentTutorial
        {
            get => CurrentTutorial;
            set => CurrentTutorial = value;
        }

        bool IWindowCache.PlayModeChanging
        {
            get => PlayModeChanging;
            set => PlayModeChanging = value;
        }

        List<SceneInfo> IWindowCache.ActiveScenesBeforeTutorialStarted
        {
            get => ActiveScenesBeforeTutorialStarted;
            set => ActiveScenesBeforeTutorialStarted = value;
        }

        SceneViewState IWindowCache.SceneViewStateBeforeTutorialStarted
        {
            get => SceneViewStateBeforeTutorialStarted;
            set => SceneViewStateBeforeTutorialStarted = value;
        }

        [SerializeField]
        internal string CurrentView;

        [SerializeField]
        internal string PreviousView;

        [SerializeField]
        internal bool DomainReloadOccured;

        [SerializeField]
        internal bool IsOpen;

        [SerializeField]
        internal bool AreTestsRunning;

        [SerializeField]
        internal Tutorial CurrentTutorial;

        [SerializeField]
        internal bool PlayModeChanging;

        [SerializeField]
        internal List<SceneInfo> ActiveScenesBeforeTutorialStarted;

        [SerializeField]
        internal SceneViewState SceneViewStateBeforeTutorialStarted;
    }

    [Serializable]
    internal class SelectedItemsDictionary : SerializableDictionary<string, bool>
    {
        public SelectedItemsDictionary() { }

        public SelectedItemsDictionary(IDictionary<string, bool> dictionary) : base(dictionary) { }
    }
}


