using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// A generic event for signaling changes in a tutorial container.
    /// Parameters: sender.
    /// </summary>
    [Serializable]
    public class TutorialContainerEvent : UnityEvent<TutorialContainer>
    {
    }

    /// <summary>
    /// A tutorial container is a collection of tutorial content, and is used to access the actual tutorials in the project.
    /// </summary>
    /// <remarks>
    /// A tutorial container can be two things:
    /// 1. Tutorial project (null Parent): a root container which is the entry point for tutorial content in the project.
    /// 2. Tutorial category (non-null Parent): a set of tutorials that are a part of some other container
    /// </remarks>
    public class TutorialContainer : ScriptableObject
    {
        /// <summary>
        /// Raised when any TutorialContainer is modified.
        /// </summary>
        /// <remarks>
        /// Raised before Modified event.
        /// </remarks>
        public static TutorialContainerEvent TutorialContainerModified = new TutorialContainerEvent();

        /// <summary>
        /// Raised when any field of this container is modified.
        /// </summary>
        /// <remarks>
        /// If 'this' container is parented, we consider modifications to 'this' container also to be modifications of the parent.
        /// </remarks>
        public TutorialContainerEvent Modified;

        /// <summary>
        /// By setting another container as a parent, this container becomes a tutorial category in the parent container.
        /// </summary>
        [Tooltip("By setting another container as a parent, this container becomes a tutorial category in the parent container.")]
        public TutorialContainer ParentContainer;

        /// <summary>
        /// This value determines the position of a container / container card within a container, if this container is shown as a card.
        /// </summary>
        [Tooltip("This value determines the position of a container / container card within a container, if this container is shown as a card.")]
        public int OrderInView;

        /// <summary>
        /// Background texture for the card/header.
        /// </summary>
        [FormerlySerializedAs("HeaderBackground")]
        public Texture2D BackgroundImage;

        /// <summary>
        /// Title shown in the card/header.
        /// </summary>
        [Tooltip("Title shown in the card/header.")]
        public LocalizableString Title = new LocalizableString();

        /// <summary>
        /// Subtitle shown in the container card and header area.
        /// </summary>
        [Tooltip("Subtitle shown in the card/header.")]
        public LocalizableString Subtitle = new LocalizableString();

        /// <summary>
        /// Used as the tooltip for the container card.
        /// </summary>
        [Tooltip("Used as the tooltip for the card.")]
        public LocalizableString Description = new LocalizableString();

        /// <summary>
        /// Can be used to override or disable (the default behavior) the default project layout specified by the Tutorial Framework.
        /// </summary>
        [Tooltip("Can be used to override or disable (the default behavior) the default project layout specified by the Tutorial Framework.")]
        public UnityEngine.Object ProjectLayout;

        /// <summary>
        /// Sections (tutorial or link card) of this container.
        /// </summary>
        [NonReorderable]
        public Section[] Sections = { };

        /// <summary>
        /// Returns the path for the ProjectLayout, relative to the project folder,
        /// or a default tutorial layout path if ProjectLayout not specified.
        /// </summary>
        public string ProjectLayoutPath =>
            ProjectLayout != null ? AssetDatabase.GetAssetPath(ProjectLayout) : k_DefaultLayoutPath;

        // The default layout used when a project is started for the first time, if project layout is used.
        internal static readonly string k_DefaultLayoutPath =
#if UNITY_2020
            "Packages/com.unity.learn.iet-framework/Editor/DefaultAssets/DefaultLayout2020.wlt";
#else
            "Packages/com.unity.learn.iet-framework/Editor/DefaultAssets/DefaultLayout2021.wlt";
#endif

        internal IEnumerable<TutorialContainer> FindSubCategories() =>
            TutorialEditorUtils.FindAssets<TutorialContainer>().Where(c => c.ParentContainer == this);

        /// <summary>
        /// A section/card for starting a tutorial or opening a web page.
        /// </summary>
        [Serializable]
        public class Section
        {
            /// <summary>
            /// This value determines the position of a section / section card within a container.
            /// </summary>
            [Tooltip("This value determines the position of a section / section card within a container.")]
            public int OrderInView;

            /// <summary>
            /// Title of the card.
            /// </summary>
            public LocalizableString Heading;

            /// <summary>
            /// Description of the card.
            /// </summary>
            public LocalizableString Text;

            /// <summary>
            /// Used as content type metadata for external references/URLs
            /// </summary>
            [Tooltip("Used as content type metadata for external references/URLs"), FormerlySerializedAs("LinkText")]
            public string Metadata;

            /// <summary>
            /// The URL of this section.
            /// Setting the URL will take precedence and make the card act as a link card instead of a tutorial card
            /// </summary>
            [Tooltip("Setting the URL will take precedence and make the card act as a link card instead of a tutorial card")]
            public string Url;

            /// <summary>
            /// Image for the card.
            /// </summary>
            public Texture2D Image;

            /// <summary>
            /// The tutorial this container contains
            /// </summary>
            public Tutorial Tutorial;

            /// <summary>
            /// Does this represent a tutorial?
            /// </summary>
            public bool IsTutorial => Url.IsNullOrEmpty();

            /// <summary>
            /// Is this section set up properly? Does it have all the data needed to fulfill its purpose?
            /// </summary>
            public bool IsConfiguredCorrectly
            {
                get
                {
                    if (IsTutorial)
                    {
                        return Tutorial != null;
                    }
                    return !Url.IsNullOrEmpty();
                }
            }
            /// <summary>
            /// The ID of the represented tutorial, if any
            /// </summary>
            public string TutorialId => Tutorial?.LessonId.AsEmptyIfNull();

            /// <summary>
            /// Starts the tutorial of the section
            /// </summary>
            [Obsolete("Will be removed in v4. Use TutorialWindow.StartTutorial() instead")] //todo: remove in v4
            public void StartTutorial()
            {
                TutorialWindow.StartTutorial(Tutorial);
            }

            /// <summary>
            /// Opens the URL Of the section, if any
            /// </summary>
            public void OpenUrl()
            {
                // TODO by making a static OpenUrl(string url) utility function we can easily track rich text hyperlink clicks also
                TutorialEditorUtils.OpenUrl(Url);
                AnalyticsHelper.SendExternalReferenceEvent(Url, Heading.Untranslated, Metadata, Tutorial?.LessonId);
            }

            /// <summary>
            /// Loads the state of the section from SessionState.
            /// </summary>
            /// <returns>returns true if the state was found from EditorPrefs</returns>
            internal bool LoadState()
            {
                return IsTutorial
                    && (IsConfiguredCorrectly && Tutorial.LoadLocalCompletionState());
            }
        }

        void OnValidate()
        {
            Title = POFileUtils.SanitizeString(Title);
            Subtitle = POFileUtils.SanitizeString(Subtitle);
            Description = POFileUtils.SanitizeString(Description);
            foreach (var section in Sections)
            {
                section.Heading = POFileUtils.SanitizeString(section.Heading);
                section.Text = POFileUtils.SanitizeString(section.Text);
            }
            Array.Sort(Sections, (x, y) => x.OrderInView.CompareTo(y.OrderInView));
        }

        /// <summary>
        /// Loads the tutorial project layout
        /// </summary>
        public void LoadTutorialProjectLayout()
        {
            TutorialModel.LoadWindowLayoutWorkingCopy(ProjectLayoutPath);
        }

        /// <summary>
        /// Raises the Modified events for this asset.
        /// </summary>
        public void RaiseModified()
        {
            TutorialContainerModified?.Invoke(this);
            Modified?.Invoke(this);
        }
    }
}
