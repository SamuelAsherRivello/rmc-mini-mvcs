using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Unity.Tutorials.Core.Editor
{
    internal enum MaskType
    {
        FullyUnmasked = 0,
        BlockInteractions
    }

    internal enum MaskSizeModifier
    {
        NoModifications = 0,
        ExpandWidthToWholeWindow
    }

    internal struct MaskViewData
    {
        internal MaskType maskType;
        internal List<Rect> rects;
        internal MaskSizeModifier maskSizeModifier;
        public Type EditorWindowType;

        internal static MaskViewData CreateEmpty(MaskType type)
        {
            return new MaskViewData()
            {
                maskType = type,
                rects = null,
            };
        }
    }

    [Serializable]
    class UnmaskedView
    {
        public class MaskData : ICloneable
        {
            internal Dictionary<GUIViewProxy, MaskViewData> m_MaskData;

            public MaskData() : this(null) { }

            public int Count { get { return m_MaskData.Count; } }

            internal MaskData(Dictionary<GUIViewProxy, MaskViewData> maskData)
            {
                m_MaskData = maskData ?? new Dictionary<GUIViewProxy, MaskViewData>();
            }

            public void AddParentFullyUnmasked(EditorWindow window)
            {
                if (!window.IsParentNull()) //this is true when the tutorial window is undocked during a tutorial
                {
                    m_MaskData[window.GetParent()] = MaskViewData.CreateEmpty(MaskType.FullyUnmasked);
                }
            }

            public void RemoveParent(EditorWindow window)
            {
                m_MaskData.Remove(window.GetParent());
            }

            public void AddTooltipViews()
            {
                var allViews = new List<GUIViewProxy>();
                GUIViewDebuggerHelperProxy.GetViews(allViews);

                foreach (var tooltipView in allViews.Where(v => v.IsGUIViewAssignableTo(GUIViewProxy.TooltipViewType)))
                    m_MaskData[tooltipView] = MaskViewData.CreateEmpty(MaskType.FullyUnmasked);
            }

            public void RemoveTooltipViews()
            {
                foreach (var view in m_MaskData.Keys.ToArray())
                {
                    if (view.IsGUIViewAssignableTo(GUIViewProxy.TooltipViewType))
                        m_MaskData.Remove(view);
                }
            }

            public object Clone()
            {
                return new MaskData(m_MaskData.ToDictionary(kv => kv.Key, kv => kv.Value));
            }
        }

        public static MaskData GetViewsAndRects(IEnumerable<UnmaskedView> unmaskedViews)
        {
            bool foundAncestorProperty;
            return GetViewsAndRects(unmaskedViews, out foundAncestorProperty);
        }

        public static MaskData GetViewsAndRects(IEnumerable<UnmaskedView> unmaskedViews, out bool foundAncestorProperty)
        {
            foundAncestorProperty = false;

            var allViews = new List<GUIViewProxy>();
            GUIViewDebuggerHelperProxy.GetViews(allViews);

            // initialize result
            var result = new Dictionary<GUIViewProxy, MaskViewData>();
            var unmaskedControls = new Dictionary<GUIViewProxy, List<GuiControlSelector>>();
            var viewsWithWindows = new Dictionary<GUIViewProxy, HashSet<EditorWindow>>();
            foreach (var unmaskedView in unmaskedViews)
            {
                foreach (var view in GetMatchingViews(unmaskedView, allViews, viewsWithWindows))
                {
                    MaskViewData maskViewData;
                    if (!result.TryGetValue(view, out maskViewData))
                    {
                        result[view] = new MaskViewData
                        {
                            rects = new List<Rect>(8),
                            maskType = unmaskedView.m_MaskType,
                            maskSizeModifier = unmaskedView.m_MaskSizeModifier,
                            EditorWindowType = unmaskedView.ResolvedEditorWindowType
                        };
                    }

                    List<GuiControlSelector> controls;
                    if (!unmaskedControls.TryGetValue(view, out controls))
                        unmaskedControls[view] = controls = new List<GuiControlSelector>();

                    controls.AddRange(unmaskedView.m_UnmaskedControls);
                }
            }

            // validate input
            foreach (var viewWithWindow in viewsWithWindows)
            {
                if (viewWithWindow.Value.Count > 1)
                {
                    throw new ArgumentException(
                        string.Format(
                            "Tried to get controls from multiple EditorWindows docked in the same location: {0}",
                            string.Join(", ", viewWithWindow.Value.Select(w => w.GetType().Name).ToArray())
                            ),
                        "unmaskedViews"
                    );
                }
            }

            // populate result
            var drawInstructions = new List<IMGUIDrawInstructionProxy>(32);
            var namedControlInstructions = new List<IMGUINamedControlInstructionProxy>(32);
            var propertyInstructions = new List<IMGUIPropertyInstructionProxy>(32);

            foreach (var viewRects in result)
            {
                // prevents null exception when repainting in case e.g., user has accidentally maximized view
                if (!viewRects.Key.IsWindowAndRootViewValid)
                    continue;

                var unmaskedControlSelectors = unmaskedControls[viewRects.Key];
                if (unmaskedControlSelectors.Count == 0)
                    continue;

                // if the view refers to an InspectorWindow, flush the optimized GUI blocks so that Editor control rects will be updated
                HashSet<EditorWindow> windows;
                if (viewsWithWindows.TryGetValue(viewRects.Key, out windows) && windows.Count > 0)
                    InspectorWindowProxy.DirtyAllEditors(windows.First());

                // TODO: use actual selectors when API is in place
                GUIViewDebuggerHelperProxy.DebugWindow(viewRects.Key);

                viewRects.Key.RepaintImmediately();

                GUIViewDebuggerHelperProxy.GetDrawInstructions(drawInstructions);
                GUIViewDebuggerHelperProxy.GetNamedControlInstructions(namedControlInstructions);
                GUIViewDebuggerHelperProxy.GetPropertyInstructions(propertyInstructions);

                foreach (var controlSelector in unmaskedControls[viewRects.Key])
                {
                    bool reverse = controlSelector.SelectorMatchType == GuiControlSelector.MatchType.Last;
                    bool selectAll = controlSelector.SelectorMatchType == GuiControlSelector.MatchType.All;

                    var regionRects = new List<Rect>();
                    switch (controlSelector.SelectorMode)
                    {
                        case GuiControlSelector.Mode.GuiContent:
                            bool IsGuiContentMatch(IMGUIDrawInstructionProxy instruction, GUIContent content) =>
                                AreEquivalent(instruction.usedGUIContent, content);

                            if (reverse)
                                drawInstructions.Reverse();

                            foreach (var instruction in drawInstructions)
                            {
                                if (IsGuiContentMatch(instruction, controlSelector.GuiContent))
                                {
                                    regionRects.Add(instruction.rect);
                                    if (!selectAll)
                                        break;
                                }
                            }
                            break;

                        case GuiControlSelector.Mode.GuiStyleName:
                            bool IsGuiStyleNameMatch(IMGUIDrawInstructionProxy instruction, string styleName) =>
                                instruction.usedGUIStyleName == styleName;

                            if (reverse)
                                drawInstructions.Reverse();

                            foreach (var instruction in drawInstructions)
                            {
                                if (IsGuiStyleNameMatch(instruction, controlSelector.GuiStyleName))
                                {
                                    regionRects.Add(instruction.rect);
                                    if (!selectAll)
                                        break;
                                }
                            }
                            break;

                        case GuiControlSelector.Mode.NamedControl:
                            bool IsControlNameMatch(IMGUINamedControlInstructionProxy instruction, string controlName) =>
                                instruction.name == controlName;

                            if (reverse)
                                namedControlInstructions.Reverse();

                            foreach (var instruction in namedControlInstructions)
                            {
                                if (IsControlNameMatch(instruction, controlSelector.ControlName))
                                {
                                    regionRects.Add(instruction.rect);
                                    if (!selectAll)
                                        break;
                                }
                            }
                            break;

                        case GuiControlSelector.Mode.Property:
                            bool IsPropertyMatch(IMGUIPropertyInstructionProxy instruction, string typeName, string propertyPath) =>
                                (instruction.targetTypeName == typeName && instruction.path == controlSelector.PropertyPath);

                            if (controlSelector.TargetType == null)
                                continue;

                            if (reverse)
                                propertyInstructions.Reverse();

                            var targetTypeName = controlSelector.TargetType.AssemblyQualifiedName;
                            foreach (var instruction in propertyInstructions)
                            {
                                if (IsPropertyMatch(instruction, targetTypeName, controlSelector.PropertyPath))
                                {
                                    regionRects.Add(instruction.rect);
                                    if (!selectAll)
                                        break;
                                }
                            }

                            if (!regionRects.Any())
                            {
                                // Property instruction not found
                                // Let's see if we can find any of the ancestor instructions to allow the user to unfold
                                Rect regionRect = Rect.zero;
                                foundAncestorProperty = FindAncestorPropertyRegion(
                                    controlSelector.PropertyPath, targetTypeName, drawInstructions, propertyInstructions, ref regionRect
                                );
                                if (foundAncestorProperty)
                                    regionRects.Add(regionRect);
                            }
                            break;

                        case GuiControlSelector.Mode.ObjectReference:
                            bool IsObjectNameMatch(IMGUIDrawInstructionProxy instruction, string objectName) =>
                                instruction.usedGUIContent.text == objectName;

                            if (controlSelector.ObjectReference == null)
                                continue;

                            var referencedObject = controlSelector.ObjectReference.SceneObjectReference.ReferencedObject;
                            if (referencedObject == null)
                                continue;

                            if (reverse)
                                drawInstructions.Reverse();

                            foreach (var instruction in drawInstructions)
                            {
                                if (IsObjectNameMatch(instruction, referencedObject.name))
                                {
                                    regionRects.Add(instruction.rect);
                                    if (!selectAll)
                                        break;
                                }
                            }
                            break;

                        case GuiControlSelector.Mode.VisualElement:
                            // At least one of the three properties must be specified in order to make a sensible query.
                            if (controlSelector.VisualElementTypeName.IsNotNullOrWhiteSpace() ||
                                controlSelector.VisualElementClassName.IsNotNullOrWhiteSpace() ||
                                controlSelector.VisualElementName.IsNotNullOrWhiteSpace())
                            {
                                var visualTree = UIElementsHelper.GetVisualTree(viewRects.Key);
                                // Passing null as name or class will make the query to consider it as an optional argument.
                                var queryBuilder = visualTree.Query(
                                    controlSelector.VisualElementName.AsNullIfWhiteSpace(),
                                    controlSelector.VisualElementClassName.AsNullIfWhiteSpace()
                                );
                                // Apply type, if valid type specified.
                                if (controlSelector.VisualElementTypeName.IsNotNullOrWhiteSpace())
                                {
                                    queryBuilder = queryBuilder.Where(elem => elem.GetType().ToString() == controlSelector.VisualElementTypeName);
                                }

                                var elements = queryBuilder.Build().ToList();
                                if (reverse)
                                    elements.Reverse();

                                foreach (var element in elements)
                                {
                                    regionRects.Add(element.worldBound);
                                    if (!selectAll)
                                        break;
                                }
                            }
                            break;

                        default:
                            Debug.LogErrorFormat(
                                "No method currently implemented for selecting using specified mode: {0}",
                                controlSelector.SelectorMode
                            );
                            break;
                    }

                    if (regionRects.Any())
                    {
                        if (viewRects.Value.maskSizeModifier == MaskSizeModifier.ExpandWidthToWholeWindow)
                        {
                            const int padding = 5;
                            regionRects.ForEach(regionRect =>
                            {
                                regionRect.x = padding;
                                regionRect.width = viewRects.Key.Position.width - padding * 2;
                            });
                        }
                        viewRects.Value.rects.AddRange(regionRects);
                    }
                }

                GUIViewDebuggerHelperProxy.StopDebugging();
            }

            return new MaskData(result);
        }

        static bool FindAncestorPropertyRegion(string propertyPath, string targetTypeName,
            List<IMGUIDrawInstructionProxy> drawInstructions, List<IMGUIPropertyInstructionProxy> propertyInstructions,
            ref Rect regionRect)
        {
            while (true)
            {
                // Remove last component of property path
                var lastIndexOfDelimiter = propertyPath.LastIndexOf(".");
                if (lastIndexOfDelimiter < 1)
                {
                    // No components left, give up
                    return false;
                }
                propertyPath = propertyPath.Substring(0, lastIndexOfDelimiter);

                foreach (var instruction in propertyInstructions)
                {
                    if (instruction.targetTypeName == targetTypeName &&
                        instruction.path == propertyPath)
                    {
                        regionRect = instruction.rect;

                        // The property rect itself does not contain the foldout arrow
                        // Expand region to include all draw instructions for this property
                        var unifiedInstructions = new List<IMGUIInstructionProxy>(128);
                        GUIViewDebuggerHelperProxy.GetUnifiedInstructions(unifiedInstructions);
                        var collectDrawInstructions = false;
                        var propertyBeginLevel = 0;
                        foreach (var unifiedInstruction in unifiedInstructions)
                        {
                            if (collectDrawInstructions)
                            {
                                if (unifiedInstruction.level <= propertyBeginLevel)
                                    break;

                                if (unifiedInstruction.type == InstructionTypeProxy.StyleDraw)
                                {
                                    var drawRect = drawInstructions[unifiedInstruction.typeInstructionIndex].rect;
                                    if (drawRect.xMin < regionRect.xMin)
                                        regionRect.xMin = drawRect.xMin;
                                    if (drawRect.yMin < regionRect.yMin)
                                        regionRect.yMin = drawRect.yMin;
                                    if (drawRect.xMax > regionRect.xMax)
                                        regionRect.xMax = drawRect.xMax;
                                    if (drawRect.yMax > regionRect.yMax)
                                        regionRect.yMax = drawRect.yMax;
                                }
                            }
                            else
                            {
                                if (unifiedInstruction.type == InstructionTypeProxy.PropertyBegin)
                                {
                                    var propertyInstruction = propertyInstructions[unifiedInstruction.typeInstructionIndex];
                                    if (propertyInstruction.targetTypeName == targetTypeName
                                        && propertyInstruction.path == propertyPath)
                                    {
                                        collectDrawInstructions = true;
                                        propertyBeginLevel = unifiedInstruction.level;
                                    }
                                }
                            }
                        }

                        return true;
                    }
                }
            }
        }

        static bool AreEquivalent(GUIContent gc1, GUIContent gc2)
        {
            return
                gc1.image == gc2.image &&
                (string.IsNullOrEmpty(gc1.text) ? string.IsNullOrEmpty(gc2.text) : gc1.text == gc2.text) &&
                (string.IsNullOrEmpty(gc1.tooltip) ? string.IsNullOrEmpty(gc2.tooltip) : gc1.tooltip == gc2.tooltip);
        }

        static IEnumerable<GUIViewProxy> GetMatchingViews(
            UnmaskedView unmaskedView,
            List<GUIViewProxy> allViews,
            Dictionary<GUIViewProxy, HashSet<EditorWindow>> viewsWithWindows)
        {
            var matchingViews = new HashSet<GUIViewProxy>(new GUIViewProxyComparer());

            switch (unmaskedView.m_SelectorType)
            {
                case SelectorType.EditorWindow:
                    var targetEditorWindowType = unmaskedView.ResolvedEditorWindowType;
                    if (unmaskedView.m_EditorWindowType.IsSpecified && targetEditorWindowType == null)
                    {
                        throw new ArgumentException(
                            $"Specified unmasked view does not refer to a known EditorWindow type:\n{JsonUtility.ToJson(unmaskedView, true)}",
                            "unmaskedView"
                        );
                    }
                    if (targetEditorWindowType != null)
                    {
                        // make sure desired window is in current layout
                        /*
                         * todo: discuss this feature proposal:
                         * uncommenting this would basically implement a "show if not already on screen" feature. 
                         * Maybe we could add an option in the masking settings to do so?
                         * 
                         * var window = EditorWindow.GetWindow(targetEditorWindowType);
                         */
                        var window = Resources.FindObjectsOfTypeAll(targetEditorWindowType).Cast<EditorWindow>().FirstOrDefault();
                        if (window == null || window.GetParent() == null)
                        {
                            return matchingViews;
                        }

                        if (!allViews.Contains(window.GetParent()))
                        {
                            allViews.Add(window.GetParent());
                        }

                        foreach (var view in allViews)
                        {
                            if (!view.IsActualViewAssignableTo(targetEditorWindowType))
                            {
                                continue;
                            }

                            if (!viewsWithWindows.TryGetValue(view, out HashSet<EditorWindow> windows))
                            {
                                viewsWithWindows[view] = windows = new HashSet<EditorWindow>();
                            }

                            windows.Add(window);
                            matchingViews.Add(view);
                        }
                    }
                    break;
                case SelectorType.GUIView:
                    var targetViewType = unmaskedView.m_ViewType.Type;
                    if (unmaskedView.m_ViewType.IsSpecified && targetViewType == null)
                    {
                        throw new ArgumentException(
                            $"Specified unmasked view does not refer to a known GUIView type:\n{JsonUtility.ToJson(unmaskedView, true)}",
                            "unmaskedView"
                        );
                    }
                    if (targetViewType != null)
                    {
                        foreach (var view in allViews)
                        {
                            if (view.IsGUIViewAssignableTo(targetViewType))
                            {
                                matchingViews.Add(view);
                            }
                        }
                    }
                    break;
            }

            return matchingViews;
        }

        public enum SelectorType
        {
            GUIView,
            EditorWindow,
        }

        [SerializeField]
        internal SelectorType m_SelectorType;

        /// <summary>
        /// Applicable when SelectorType == GUIView.
        /// </summary>
        [SerializedTypeGuiViewFilter]
        [SerializeField]
        internal SerializedType m_ViewType = new SerializedType(null);

        /// <summary>
        /// Applicable when SelectorType == EditorWindow.
        /// </summary>
        [SerializedTypeFilter(typeof(EditorWindow), false)]
        [SerializeField]
        internal SerializedType m_EditorWindowType = new SerializedType(null);
        Type ResolvedEditorWindowType
        {
            get
            {
                // Use main EditorWindow type if it can be resolved
                var type = m_EditorWindowType.Type;
                if (type != null)
                    return type;

                // Otherwise use first alternate type that resolves
                foreach (var editorWindowTypeWrapper in m_AlternateEditorWindowTypes)
                {
                    type = editorWindowTypeWrapper.Type.Type;
                    if (type != null)
                        return type;
                }

                return null;
            }
        }

        /// <summary>
        /// Applicable when SelectorType == EditorWindow. Used as the back-up type if primary EditorWindowType cannot be resolved.
        /// </summary>
        [SerializeField]
        internal EditorWindowTypeCollection m_AlternateEditorWindowTypes = new EditorWindowTypeCollection();

        [SerializeField]
        internal MaskType m_MaskType = MaskType.FullyUnmasked;

        [SerializeField]
        internal MaskSizeModifier m_MaskSizeModifier = MaskSizeModifier.NoModifications;

        [SerializeField]
        internal List<GuiControlSelector> m_UnmaskedControls = new List<GuiControlSelector>();

        public int GetUnmaskedControls(List<GuiControlSelector> unmaskedControls)
        {
            unmaskedControls.Clear();
            unmaskedControls.AddRange(m_UnmaskedControls);
            return unmaskedControls.Count;
        }

        protected UnmaskedView() { }

        internal static UnmaskedView CreateInstanceForGUIView(Type type, IList<GuiControlSelector> unmaskedControls = null)
        {
            if (!GUIViewProxy.IsAssignableFrom(type))
                throw new InvalidOperationException("Type must be assignable to GUIView");

            UnmaskedView result = new UnmaskedView();
            result.m_SelectorType = SelectorType.GUIView;
            result.m_ViewType.Type = type;
            if (unmaskedControls != null)
                result.m_UnmaskedControls.AddRange(unmaskedControls);
            return result;
        }

        public static UnmaskedView CreateInstanceForEditorWindow(Type type, IList<GuiControlSelector> unmaskedControls = null)
        {
            if (!typeof(EditorWindow).IsAssignableFrom(type))
                throw new InvalidOperationException("Type must be assignable to EditorWindow");

            UnmaskedView result = new UnmaskedView();
            result.m_SelectorType = SelectorType.EditorWindow;
            result.m_EditorWindowType.Type = type;
            if (unmaskedControls != null)
                result.m_UnmaskedControls.AddRange(unmaskedControls);
            return result;
        }
    }

    [Serializable]
    class EditorWindowType
    {
        [SerializeField, FormerlySerializedAs("editorWindowType")]
        [SerializedTypeFilter(typeof(EditorWindow), false)]
        public SerializedType Type;

        public EditorWindowType(SerializedType editorWindowType)
        {
            Type = editorWindowType;
        }
    }

    [Serializable]
    class EditorWindowTypeCollection : CollectionWrapper<EditorWindowType>
    {
        public EditorWindowTypeCollection() : base()
        {
        }

        public EditorWindowTypeCollection(IList<EditorWindowType> items) : base(items)
        {
        }
    }
}
