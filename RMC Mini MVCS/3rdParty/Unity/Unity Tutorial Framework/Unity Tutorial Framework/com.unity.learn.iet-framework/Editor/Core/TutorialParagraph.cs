using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Serialization;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Different paragraph types.
    /// </summary>
    public enum ParagraphType
    {
        /// <summary>
        /// Text.
        /// </summary>
        Narrative,
        /// <summary>
        /// Text instructions with underlying completion criterion logic.
        /// </summary>
        Instruction,
        /// <summary>
        /// A button for switching to another tutorial.
        /// </summary>
        SwitchTutorial,

        // NOTE: backwards-compatibility by leaving holes for deprecated values
        // UnorderedList = 3,
        // OrderedList = 4,
        // Icons = 5,

        /// <summary>
        /// An image.
        /// </summary>
        Image = 6,
        /// <summary>
        /// A video clip.
        /// </summary>
        Video,
    }

    enum CompletionType
    {
        CompletedWhenAllAreTrue,    // TODO Simplify name, "All(True)"
        CompletedWhenAnyIsTrue      // TODO Simplify name, "Any(True)"
    }

    /// <summary>
    /// A section of the TutorialPage.
    /// </summary>
    [Serializable]
    public class TutorialParagraph : ISerializationCallbackReceiver
    {
        /// <summary>
        /// Type of this paragraph.
        /// </summary>
        public ParagraphType Type { get => m_Type; internal set => m_Type = value; }
        [SerializeField]
        internal ParagraphType m_Type;

        /// <summary>
        /// Title for Narrative/Instruction, not applicable for SwitchTutorial currently.
        /// </summary>
        public LocalizableString Title;

        /// <summary>
        /// Text/description for Narrative/Instruction or button text for SwitchTutorial.
        /// </summary>
        [LocalizableTextArea(1, 15)]
        public LocalizableString Text;

        /// <summary>
        /// Used for SwitchTutorial.
        /// </summary>
        [SerializeField]
        internal Tutorial m_Tutorial;

        /// <summary>
        /// The image if this paragraph's type is Image.
        /// </summary>
        public Texture2D Image { get => m_Image; set => m_Image = value; }
        [SerializeField]
        Texture2D m_Image;

        /// <summary>
        /// The video clip if this paragraph's type is Video.
        /// </summary>
        public VideoClip Video { get => m_Video; set => m_Video = value; }
        [SerializeField]
        VideoClip m_Video;

        [SerializeField]
        [Tooltip("The state in which the criteria of the page are be considered as completed")]
        internal CompletionType m_CriteriaCompletion = CompletionType.CompletedWhenAllAreTrue;

        [SerializeField] internal TypedCriterionCollection m_Criteria = new TypedCriterionCollection();
        readonly List<TypedCriterion> m_CriteriaBuffer = new List<TypedCriterion>();

        internal IList<TypedCriterion> CriteriaList
        {
            get
            {
                m_Criteria.GetItems(m_CriteriaBuffer);
                return m_CriteriaBuffer.ToArray();
            }
        }

        /// <summary>
        /// The completion criteria if this paragraph's type is Instruction.
        /// </summary>
        public IEnumerable<TypedCriterion> Criteria => CriteriaList;

        /// <summary>
        /// The masking settings for this paragraph.
        /// </summary>
        internal MaskingSettings MaskingSettings => m_MaskingSettings;
        [SerializeField]
        MaskingSettings m_MaskingSettings = new MaskingSettings();

        /// <summary>
        /// Is this paragraph completed? Applicable if this paragraph's type is Instruction.
        /// </summary>
        public bool Completed
        {
            get
            {
                if (Type != ParagraphType.Instruction)
                {
                    return true;
                }
                bool allMandatory = m_CriteriaCompletion == CompletionType.CompletedWhenAllAreTrue;
                bool result = allMandatory;

                foreach (var typedCriterion in m_Criteria)
                {
                    var criterion = typedCriterion.Criterion;
                    if (criterion != null)
                    {
                        if (!allMandatory && criterion.IsCompleted)
                        {
                            result = true;
                            break;
                        }

                        if (allMandatory && !criterion.IsCompleted)
                        {
                            result = false;
                            break;
                        }
                    }
                }

                return result;
            }
        }

        internal bool ShouldRefreshMaskingOnHierarchyChange
        {
            get
            {
                if (!m_MaskingSettings.Enabled)
                {
                    return false;
                }

                foreach (var unmaskedView in m_MaskingSettings.UnmaskedViews)
                {
                    Type editorWindowType = unmaskedView.m_EditorWindowType.Type;
                    bool isMaskingRelatedToHierarchy = editorWindowType == GUIViewProxy.SceneHierarchyWindowType;
                    if (isMaskingRelatedToHierarchy && unmaskedView.GetUnmaskedControls(new List<GuiControlSelector>()) > 0)
                    {
                        // Related to hierarchy, and the unmasked controls is non-empty
                        return true;
                    }
                }
                /* either none of the masking is related to hierarchy
                or all masking related to hierarchy has an empty unmasked controls list */
                return false;
            }
        }

        // Backwards-compatibility for < 1.2
        [SerializeField, HideInInspector]
        string m_Summary;
        [SerializeField, FormerlySerializedAs("m_description1"), HideInInspector]
        string m_Description;
        [SerializeField, FormerlySerializedAs("m_Text"), HideInInspector]
        string m_InstructionBoxTitle;
        [SerializeField, HideInInspector]
        string m_InstructionText;
        [SerializeField, HideInInspector]
        string m_TutorialButtonText;

        /// <summary>
        /// UnityEngine.ISerializationCallbackReceiver override, do not call.
        /// </summary>
        public void OnBeforeSerialize()
        {
        }

        /// <summary>
        /// UnityEngine.ISerializationCallbackReceiver override, do not call.
        /// </summary>
        public void OnAfterDeserialize()
        {
            // Migrate content from < 1.2.
            switch (Type)
            {
                case ParagraphType.Narrative:
                    MigrateStringToLocalizableString(ref m_Summary, ref Title);
                    MigrateStringToLocalizableString(ref m_Description, ref Text);
                    break;
                case ParagraphType.Instruction:
                    MigrateStringToLocalizableString(ref m_InstructionBoxTitle, ref Title);
                    MigrateStringToLocalizableString(ref m_InstructionText, ref Text);
                    break;
                case ParagraphType.SwitchTutorial:
                    MigrateStringToLocalizableString(ref m_TutorialButtonText, ref Text);
                    break;
            }
        }

        internal static void MigrateStringToLocalizableString(ref string oldField, ref LocalizableString newField)
        {
            if (newField.Untranslated.IsNullOrEmpty() && oldField.IsNotNullOrEmpty())
            {
                newField = oldField;
                oldField = string.Empty;
            }
        }

        internal static TutorialParagraph CreateImageParagraph(Texture2D image = null) =>
            new TutorialParagraph { Type = ParagraphType.Image, Image = image };
        internal static TutorialParagraph CreateVideoParagraph(UnityEngine.Video.VideoClip video = null) =>
            new TutorialParagraph { Type = ParagraphType.Video, Video = video };
        internal static TutorialParagraph CreateNarrativeParagraph(string title, string description) =>
            new TutorialParagraph { Type = ParagraphType.Narrative, Title = title, Text = description };
        internal static TutorialParagraph CreateInstructionParagraph(string title, string description, TypedCriterionCollection criteria = null)
        {
            return new TutorialParagraph
            {
                Type = ParagraphType.Instruction,
                Title = title,
                Text = description,
                m_Criteria = criteria != null ? criteria : new TypedCriterionCollection()
            };
        }

        internal static TutorialParagraph CreateTutorialSwitchParagraph(Tutorial nextTutorial, string nextButtonText) =>
            new TutorialParagraph { Type = ParagraphType.SwitchTutorial, m_Tutorial = nextTutorial, Text = nextButtonText };
    }

    /// <summary> A wrapper class for serialization purposes. </summary>
    [Serializable]
    public class TutorialParagraphCollection : CollectionWrapper<TutorialParagraph>
    {
        /// <summary> Default-constructs an empty collection. </summary>
        public TutorialParagraphCollection() : base() { }
        /// <summary> Constructs a collection from existing items. </summary>
        /// <param name="items"></param>
        public TutorialParagraphCollection(IList<TutorialParagraph> items) : base(items) { }
    }
}
