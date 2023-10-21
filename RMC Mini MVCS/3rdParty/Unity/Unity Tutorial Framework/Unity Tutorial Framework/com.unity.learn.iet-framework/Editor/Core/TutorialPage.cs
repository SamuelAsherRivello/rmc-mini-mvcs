using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// A generic event for signaling changes in a tutorial page.
    /// Parameters: sender.
    /// </summary>
    [Serializable]
    public class TutorialPageEvent : UnityEvent<TutorialPage>
    {
    }

    /// <summary>
    /// A TutorialPage consists of TutorialParagraphs which define the content of the page.
    /// </summary>
    public class TutorialPage : ScriptableObject, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Raised when any page's criteria are tested for completion.
        /// </summary>
        public static TutorialPageEvent CriteriaCompletionStateTested = new TutorialPageEvent();

        /// <summary>
        /// Raised when any page's masking settings are changed.
        /// </summary>
        public static TutorialPageEvent TutorialPageMaskingSettingsChanged = new TutorialPageEvent();

        /// <summary>
        /// Raised when any page's non-masking settings are changed.
        /// </summary>
        public static TutorialPageEvent TutorialPageNonMaskingSettingsChanged = new TutorialPageEvent();

        internal event Action<TutorialPage> m_PlayedCompletionSound;

        /// <summary>
        /// Title of the page
        /// </summary>
        public LocalizableString Title = new LocalizableString();

        /// <summary>
        /// Are we moving to the next page?
        /// </summary>
        public bool HasMovedToNextPage { get; private set; }

        /// <summary>
        /// Are all criteria satisfied?
        /// </summary>
        public bool AreAllCriteriaSatisfied => Paragraphs.All(p => p.Completed);

        internal bool ShouldRefreshMaskingOnHierarchyChange => m_Paragraphs.Any(p => p.ShouldRefreshMaskingOnHierarchyChange);

        /// <summary>
        /// Paragraphs of this page.
        /// </summary>
        public TutorialParagraphCollection Paragraphs => m_Paragraphs;
        [SerializeField]
        internal TutorialParagraphCollection m_Paragraphs = new TutorialParagraphCollection();

        /// <summary>
        /// Has the current page any completion criteria?
        /// </summary>
        /// <returns></returns>
        public bool HasCriteria()
        {
            foreach (TutorialParagraph para in Paragraphs)
            {
                foreach (TypedCriterion crit in para.Criteria)
                {
                    if (crit.Criterion != null) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Currently active masking settings.
        /// </summary>
        internal MaskingSettings CurrentMaskingSettings
        {
            get
            {
                MaskingSettings result = null;
                for (int i = 0, count = m_Paragraphs.Count; i < count; ++i)
                {
                    if (!m_Paragraphs[i].MaskingSettings.Enabled)
                        continue;

                    result = m_Paragraphs[i].MaskingSettings;
                    if (!m_Paragraphs[i].Completed)
                        break;
                }
                return result;
            }
        }

        [Header("Initial Camera Settings")]
        [SerializeField]
        SceneViewCameraSettings m_CameraSettings = new SceneViewCameraSettings();

        /// <summary>
        /// The text shown on the Next button on all pages except the last page.
        /// </summary>
        [Header("Button Labels")]
        [Tooltip("The text shown on the next button on all pages except the last page.")]
        public LocalizableString NextButton = "Next";

        /// <summary>
        /// The text shown on the next button on the last page.
        /// </summary>
        [Tooltip("The text shown on the Next button on the last page.")]
        public LocalizableString DoneButton = "Done";

        [Header("Sounds")]
        [SerializeField]
        AudioClip m_CompletedSound = null;

        /// <summary>
        /// Should we auto-advance upon completion.
        /// </summary>
        public bool AutoAdvanceOnComplete { get => m_AutoAdvance; set => m_AutoAdvance = value; }
        [SerializeField, FormerlySerializedAs("m_autoAdvance"), Tooltip("Should we auto-advance upon completion.")]
        internal bool m_AutoAdvance;


        /// <summary>
        /// Raised before this page is displayed (even when going back).
        /// </summary>
        [Header("Events")]
        [Tooltip("Raised before this page is displayed (even when going back).")]
        public TutorialPageEvent Showing = new TutorialPageEvent();

        /// <summary>
        /// Raised after this page is displayed (even when going back).
        /// </summary>
        [Tooltip("Raised after this page is displayed (even when going back).")]
        public TutorialPageEvent Shown = new TutorialPageEvent();

        /// <summary>
        /// Raised while the user is staying on this tutorial page, every Editor frame.
        /// </summary>
        [Tooltip("Raised while the user is staying on this tutorial page, every Editor frame.")]
        public TutorialPageEvent Staying = new TutorialPageEvent();

        /// <summary>
        /// Raised when this page's criteria are tested for completion.
        /// </summary>
        [Tooltip("Raised when this page's criteria are tested for completion.")]
        public TutorialPageEvent CriteriaValidated = new TutorialPageEvent();

        /// <summary>
        /// Raised when this page's masking settings are changed.
        /// </summary>
        [Tooltip("Raised when this page's masking settings are changed.")]
        public TutorialPageEvent MaskingSettingsChanged = new TutorialPageEvent();

        /// <summary>
        /// Raised when this page's non-masking settings are changed.
        /// </summary>
        [Tooltip("Raised when this page's non-masking settings are changed.")]
        public TutorialPageEvent NonMaskingSettingsChanged = new TutorialPageEvent();

        static Queue<WeakReference<TutorialPage>> s_DeferedValidationQueue = new Queue<WeakReference<TutorialPage>>();

        // Backwards-compatibility for < 2.0.0-pre.6
        [SerializeField, HideInInspector] internal UnityEvent m_OnBeforePageShown = default;
        [SerializeField, HideInInspector] internal UnityEvent m_OnAfterPageShown = default;
        [SerializeField, HideInInspector] internal UnityEvent m_OnTutorialPageStay = default;
        [SerializeField, Tooltip("This event will be deprecated, please migrate to use Tutorial's Quit event instead.")]
        internal UnityEvent m_OnBeforeTutorialQuit = default;
        // Backwards-compatibility for < 1.2
        [SerializeField, HideInInspector] string m_NextButton = "Next";
        [SerializeField, HideInInspector] string m_DoneButton = "Done";

        /// <summary>
        /// Raises TutorialPageMaskingSettingsChanged event.
        /// </summary>
        public void RaiseMaskingSettingsChanged()
        {
            MaskingSettingsChanged?.Invoke(this);
            TutorialPageMaskingSettingsChanged?.Invoke(this);
        }

        /// <summary>
        /// Raises TutorialPageNonMaskingSettingsChanged event.
        /// </summary>
        public void RaiseNonMaskingSettingsChanged()
        {
            NonMaskingSettingsChanged?.Invoke(this);
            TutorialPageNonMaskingSettingsChanged?.Invoke(this);
        }

        static TutorialPage()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        static void OnEditorUpdate()
        {
            while (s_DeferedValidationQueue.Count != 0)
            {
                var weakPageReference = s_DeferedValidationQueue.Dequeue();
                TutorialPage page;
                if (weakPageReference.TryGetTarget(out page))
                {
                    if (page != null) //Taking into account "unity null"
                    {
                        page.SyncCriteriaAndFutureReferences();
                    }
                }
            }
        }

        void OnEnable()
        {
            // Migrate content from < 2.0.0-pre.6
            // NOTE events are migrated in OnEnable() instead of OnAfterDeserialize() due to the use of SerializedObject:
            // "UnityException: InternalCreate is not allowed to be called during serialization,
            // call it from OnEnable instead. Called from ScriptableObject 'TutorialPage'."
            if (m_OnBeforePageShown != null && m_OnBeforePageShown.GetPersistentEventCount() > 0)
            {
                TransferPersistentCalls(this, nameof(m_OnBeforePageShown), nameof(Showing));
                Debug.Log($"{AssetDatabase.GetAssetPath(this)}: OnBeforePageShown event is deprecated, migrated to use Showing automatically.");
            }

            if (m_OnAfterPageShown != null && m_OnAfterPageShown.GetPersistentEventCount() > 0)
            {
                TransferPersistentCalls(this, nameof(m_OnBeforePageShown), nameof(Shown));
                Debug.Log($"{AssetDatabase.GetAssetPath(this)}: OnAfterPageShown event is be deprecated, migrated to use Shown automatically.");
            }

            if (m_OnBeforeTutorialQuit != null && m_OnBeforeTutorialQuit.GetPersistentEventCount() > 0)
            {
                // A page doesn't have an explicit parent tutorial, and page can be in multiple tutorials; the users must migrate this event on their own.
                Debug.LogWarning($"{AssetDatabase.GetAssetPath(this)}: OnBeforeTutorialQuit event is deprecated, please migrate to use Tutorial's Quit event instead.");
            }

            if (m_OnTutorialPageStay != null && m_OnTutorialPageStay.GetPersistentEventCount() > 0)
            {
                TransferPersistentCalls(this, nameof(m_OnTutorialPageStay), nameof(Staying));
                Debug.Log($"{AssetDatabase.GetAssetPath(this)}: OnTutorialPageStay event is deprecated, asset migrated to use Staying automatically.");
            }
        }

        void OnValidate()
        {
            foreach (var paragraph in Paragraphs)
            {
                paragraph.Text = POFileUtils.SanitizeString(paragraph.Text);
                paragraph.Title = POFileUtils.SanitizeString(paragraph.Title);
            }

            // Defer synchronization of sub-assets to next editor update due to AssetDatabase interactions

            // Retaining a reference to this instance in OnValidate/OnEnable can cause issues on project load
            // The same object might be imported more than once and if it's referenced it won't be unloaded correctly
            // Use WeakReference instead of subscribing directly to EditorApplication.update to avoid strong reference

            s_DeferedValidationQueue.Enqueue(new WeakReference<TutorialPage>(this));
        }

        void SyncCriteriaAndFutureReferences()
        {
            // Find instanceIDs of referenced criteria
            var referencedCriteriaInstanceIDs = new HashSet<int>();
            foreach (var paragraph in Paragraphs)
            {
                foreach (var typedCriterion in paragraph.Criteria)
                {
                    if (typedCriterion.Criterion != null)
                        referencedCriteriaInstanceIDs.Add(typedCriterion.Criterion.GetInstanceID());
                }
            }

            // Destroy unreferenced criteria
            var assetPath = AssetDatabase.GetAssetPath(this);
            var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            var criteria = assets.Where(o => o is Criterion).Cast<Criterion>();
            foreach (var criterion in criteria)
            {
                if (!referencedCriteriaInstanceIDs.Contains(criterion.GetInstanceID()))
                    DestroyImmediate(criterion, true);
            }

            // Update future reference names
            var futureReferences = assets.Where(o => o is FutureObjectReference).Cast<FutureObjectReference>();
            foreach (var futureReference in futureReferences)
            {
                if (futureReference.Criterion == null
                    || !referencedCriteriaInstanceIDs.Contains(futureReference.Criterion.GetInstanceID()))
                {
                    // Destroy future reference from unrefereced criteria
                    DestroyImmediate(futureReference, true);
                }
                else
                    UpdateFutureObjectReferenceName(futureReference);
            }
        }

        internal void UpdateFutureObjectReferenceName(FutureObjectReference futureReference)
        {
            if (GetIndicesForCriterion(futureReference.Criterion, out int paragraphIndex, out int criterionIndex))
            {
                futureReference.name = string.Format("Paragraph {0}, Criterion {1}, {2}",
                    paragraphIndex + 1, criterionIndex + 1, futureReference.ReferenceName);
            }
        }

        bool GetIndicesForCriterion(Criterion criterion, out int paragraphIndex, out int criterionIndex)
        {
            paragraphIndex = 0;
            criterionIndex = 0;

            foreach (var paragraph in Paragraphs)
            {
                foreach (var typedCriterion in paragraph.Criteria)
                {
                    if (typedCriterion.Criterion == criterion)
                        return true;

                    criterionIndex++;
                }

                paragraphIndex++;
            }

            return false;
        }

        internal void ApplyCameraSettings()
        {
            if (m_CameraSettings != null && m_CameraSettings.Enabled)
            {
                m_CameraSettings.Apply();
            }
        }

        internal void PlayCompletionSound()
        {
            Undo.ClearAll(); //todo: investigate why this is needed
            if (m_CompletedSound != null)
            {
                AudioUtilProxy.PlayClip(m_CompletedSound);
            }
            m_PlayedCompletionSound?.Invoke(this);
        }

        internal void Initiate()
        {
            ApplyCameraSettings();
        }

        internal void SetupCompletionCriteria(UnityAction<Criterion, TutorialParagraph> onCriterionCompleted, UnityAction<Criterion, TutorialParagraph> onCriterionInvalidated, UnityAction<TutorialPage> onPageCompletionStatusChangedOrSet = null)
        {
            foreach (var paragraph in Paragraphs)
            {
                if (paragraph.Criteria == null) { continue; }
                foreach (var criterion in paragraph.Criteria)
                {
                    if (criterion.Criterion)
                    {
                        criterion.Criterion.Completed.AddListener((crit) => { OnCriterionCompleted(crit); onCriterionCompleted.Invoke(crit, paragraph); });
                        criterion.Criterion.Invalidated.AddListener((crit) => { OnCriterionInvalidated(crit); onCriterionInvalidated.Invoke(crit, paragraph); });
                        criterion.Criterion.StartTesting();
                    }
                }
            }

            CriteriaCompletionStateTested.RemoveAllListeners();
            if (onPageCompletionStatusChangedOrSet != null)
            {
                CriteriaCompletionStateTested.AddListener(onPageCompletionStatusChangedOrSet);
            }
            OnCompletionCriteriaStatusChangedOrSet();
        }

        internal void ResetUserProgressAndCompletionCriteria()
        {
            foreach (var paragraph in Paragraphs)
            {
                if (paragraph.Type != ParagraphType.Instruction) { continue; }
                foreach (var criterion in paragraph.Criteria)
                {
                    if (criterion != null && criterion.Criterion != null)
                    {
                        criterion.Criterion.Completed.RemoveAllListeners();
                        criterion.Criterion.Invalidated.RemoveAllListeners();
                        criterion.Criterion.StopTesting();
                        criterion.Criterion.ResetCompletionState();
                    }
                }
            }
            HasMovedToNextPage = false;
        }

        void OnCriterionCompleted(Criterion sender)
        {
            OnCompletionCriteriaStatusChangedOrSet();
        }

        void OnCriterionInvalidated(Criterion sender)
        {
            OnCompletionCriteriaStatusChangedOrSet();
        }

        void OnCompletionCriteriaStatusChangedOrSet()
        {
            CriteriaValidated?.Invoke(this);
            CriteriaCompletionStateTested?.Invoke(this);
        }

        internal void MarkAsCompleted()
        {
            ResetUserProgressAndCompletionCriteria();
            HasMovedToNextPage = true;
            //todo: add page-specific analytics here?
        }

        internal void RaiseShowing()
        {
            Showing?.Invoke(this);
            m_OnBeforePageShown?.Invoke();
        }

        internal void RaiseShown()
        {
            Shown?.Invoke(this);
            m_OnAfterPageShown?.Invoke();
        }

        internal void RaiseOnBeforeTutorialQuit()
        {
            m_OnBeforeTutorialQuit?.Invoke();
        }

        internal void RaiseStaying()
        {
            Staying?.Invoke(this);
            m_OnTutorialPageStay?.Invoke();
        }

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
            MigrateContentFromV1ToV2();
            MigrateContentFromV2ToV3();
        }

        /// <summary>
        /// Migrate content from < 1.2.
        /// </summary>
        void MigrateContentFromV1ToV2()
        {
            TutorialParagraph.MigrateStringToLocalizableString(ref m_NextButton, ref NextButton);
            TutorialParagraph.MigrateStringToLocalizableString(ref m_DoneButton, ref DoneButton);
        }

        /// <summary>
        /// Migrate content from < 3.0
        /// </summary>
        void MigrateContentFromV2ToV3()
        {
            if (Title.Untranslated.IsNullOrEmpty())
            {
                if (Paragraphs.Count > 1)
                {
                    if (Paragraphs[1].Title != null //in previous version, the title of the 2nd paragraph, which was always narrative, was the title ofthe page
                    && Paragraphs[1].Title.Untranslated.IsNotNullOrEmpty())
                    {
                        Title = Paragraphs[1].Title.Untranslated;
                        Paragraphs[1].Title = string.Empty;
                    }
                }
            }
        }

        internal static TutorialPage Create(params TutorialParagraph[] paragraphs)
        {
            var page = CreateInstance<TutorialPage>();
            page.Paragraphs.SetItems(paragraphs);
            return page;
        }


        // Based on https://gist.github.com/wesleywh/1c56d880c0289371ea2dc47661a0cdaf
        static void TransferPersistentCalls(UnityEngine.Object obj, in string srcEventName, in string dstEventName)
        {
            var so = new SerializedObject(obj);
            const string CallsPropertyPathFormat = "{0}.m_PersistentCalls.m_Calls";
            var srcCalls = so.FindProperty(string.Format(CallsPropertyPathFormat, GetValidFieldName(srcEventName.Trim())));
            var dstCalls = so.FindProperty(string.Format(CallsPropertyPathFormat, GetValidFieldName(dstEventName.Trim())));
            var dstOffset = dstCalls.arraySize;

            for (var srcIndex = 0; srcIndex < srcCalls.arraySize; srcIndex++)
            {
                var srcCall = srcCalls.GetArrayElementAtIndex(srcIndex);
                var srcTarget = GetCallTarget(srcCall);
                var srcMethodName = GetCallMethodName(srcCall);
                var srcMode = GetCallMode(srcCall);
                var srcCallState = GetCallState(srcCall);
                var srcArgs = GetCallArgs(srcCall);
                var srcObjectArg = GetCallObjectArg(srcArgs);
                var srcObjectArgType = GetCallObjectArgType(srcArgs);
                var srcIntArg = GetCallIntArg(srcArgs);
                var srcFloatArg = GetCallFloatArg(srcArgs);
                var srcStringArg = GetCallStringArg(srcArgs);
                var srcBoolArg = GetCallBoolArg(srcArgs);

                SerializedProperty dstCall;
                if (dstOffset > 0)
                {
                    dstCall = dstCalls.GetArrayElementAtIndex(srcIndex);
                    // If we are satisfied that the call is exactly the same, skip ahead.
                    if (SerializedProperty.DataEquals(srcCall, dstCall))
                        continue;
                }

                // Only unique properties beyond this point. Append with care.
                // Copy Properties from Source to Destination
                dstCalls.InsertArrayElementAtIndex(dstOffset + srcIndex);
                dstCall = dstCalls.GetArrayElementAtIndex(dstOffset + srcIndex);

                SerializedProperty dstTarget = GetCallTarget(dstCall);
                SerializedProperty dstMethodName = GetCallMethodName(dstCall);
                SerializedProperty dstMode = GetCallMode(dstCall);
                SerializedProperty dstCallState = GetCallState(dstCall);
                SerializedProperty dstArgs = GetCallArgs(dstCall);
                SerializedProperty dstObjectArg = GetCallObjectArg(dstArgs);
                SerializedProperty dstObjectArgType = GetCallObjectArgType(dstArgs);
                SerializedProperty dstIntArg = GetCallIntArg(dstArgs);
                SerializedProperty dstFloatArg = GetCallFloatArg(dstArgs);
                SerializedProperty dstStringArg = GetCallStringArg(dstArgs);
                SerializedProperty dstBoolArg = GetCallBoolArg(dstArgs);

                dstTarget.objectReferenceValue = srcTarget.objectReferenceValue;
                dstMethodName.stringValue = srcMethodName.stringValue;
                dstMode.enumValueIndex = srcMode.enumValueIndex;
                dstCallState.enumValueIndex = srcCallState.enumValueIndex;

                dstObjectArg.objectReferenceValue = srcObjectArg.objectReferenceValue;
                dstObjectArgType.stringValue = srcObjectArgType.stringValue;
                dstIntArg.intValue = srcIntArg.intValue;
                dstFloatArg.floatValue = srcFloatArg.floatValue;
                dstStringArg.stringValue = srcStringArg.stringValue;
                dstBoolArg.boolValue = srcBoolArg.boolValue;
            }

            srcCalls.ClearArray();

            so.ApplyModifiedProperties();

            string GetValidFieldName(in string name)
            {
                const BindingFlags bindedTypes = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                var field = obj.GetType().GetField(name, bindedTypes);
                var value = field?.GetValue(obj);
                if (value is UnityEventBase)
                    return name;

                throw new FieldAccessException("Incorrect event name.");
            }

            SerializedProperty GetCallTarget(in SerializedProperty sp) => sp?.FindPropertyRelative("m_Target");
            SerializedProperty GetCallMethodName(in SerializedProperty sp) => sp?.FindPropertyRelative("m_MethodName");
            SerializedProperty GetCallMode(in SerializedProperty sp) => sp?.FindPropertyRelative("m_Mode");
            SerializedProperty GetCallState(in SerializedProperty sp) => sp?.FindPropertyRelative("m_CallState");

            SerializedProperty GetCallArgs(in SerializedProperty sp) => sp?.FindPropertyRelative("m_Arguments");
            SerializedProperty GetCallObjectArg(in SerializedProperty sp) => sp?.FindPropertyRelative("m_ObjectArgument");
            SerializedProperty GetCallObjectArgType(in SerializedProperty sp) => sp?.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName");
            SerializedProperty GetCallIntArg(in SerializedProperty sp) => sp?.FindPropertyRelative("m_IntArgument");
            SerializedProperty GetCallFloatArg(in SerializedProperty sp) => sp?.FindPropertyRelative("m_FloatArgument");
            SerializedProperty GetCallStringArg(in SerializedProperty sp) => sp?.FindPropertyRelative("m_StringArgument");
            SerializedProperty GetCallBoolArg(in SerializedProperty sp) => sp?.FindPropertyRelative("m_BoolArgument");
        }
    }
}
