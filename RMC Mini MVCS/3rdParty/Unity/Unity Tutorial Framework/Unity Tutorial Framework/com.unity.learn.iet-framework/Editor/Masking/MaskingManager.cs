using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif

namespace Unity.Tutorials.Core.Editor
{
    using static Localization;

    /// <summary>
    /// Manages masking and highlighting.
    /// </summary>
    internal static class MaskingManager
    {
        /// <summary>
        /// Master control for masking and highlighting.
        /// </summary>
        public static UserSetting<bool> MaskingEnabled = new UserSetting<bool>("IET.MaskingEnabled", Tr(LocalizationKeys.k_SettingsMaskingEnabled), true, Tr(LocalizationKeys.k_SettingsMaskingEnabledTooltip));

        /// <summary>
        /// Delay, in seconds, before the highlight starts pulsating.
        /// </summary>
        public static float HighlightAnimationDelay { get; set; }

        /// <summary>
        /// Speed of the highligh pulsation.
        /// </summary>
        public static float HighlightAnimationSpeed { get; set; }

        static GUIViewProxyComparer s_GUIViewProxyComparer = new GUIViewProxyComparer();

        static readonly Dictionary<GUIViewProxy, MaskViewData> s_UnmaskedViews = new Dictionary<GUIViewProxy, MaskViewData>(s_GUIViewProxyComparer);
        static readonly Dictionary<GUIViewProxy, MaskViewData> s_HighlightedViews = new Dictionary<GUIViewProxy, MaskViewData>(s_GUIViewProxyComparer);

        static readonly List<VisualElement> s_Masks = new List<VisualElement>();
        static readonly List<VisualElement> s_Highlighters = new List<VisualElement>();

        static double s_LastHighlightTime;

        internal static bool IsMasked(GUIViewProxy view, List<Rect> rects)
        {
            rects.Clear();

            MaskViewData maskViewData;
            if (s_UnmaskedViews.TryGetValue(view, out maskViewData))
            {
                rects.AddRange(maskViewData.rects);
                return false;
            }
            return true;
        }

        internal static bool IsHighlighted(GUIViewProxy view, List<Rect> rects)
        {
            rects.Clear();
            if (!s_HighlightedViews.TryGetValue(view, out MaskViewData maskViewData))
            {
                return false;
            }
            rects.AddRange(maskViewData.rects);
            return true;
        }

        internal static void OnEditorUpdate()
        {
            // do not animate unless enough time has passed since masking was last applied
            var t = EditorApplication.timeSinceStartup - s_LastHighlightTime - HighlightAnimationDelay;
            if (t < 0d)
            {
                return;
            }

            const float baseBorderWidth = 4.2f;
            const float borderWidthAmplitude = 2.1f;
            float animatedBorderWidth = Mathf.Cos((float)t * HighlightAnimationSpeed) * borderWidthAmplitude + baseBorderWidth;

            foreach (var highlighter in s_Highlighters)
            {
                if (highlighter == null) { continue; }

                highlighter.style.borderLeftWidth = animatedBorderWidth;
                highlighter.style.borderRightWidth = animatedBorderWidth;
                highlighter.style.borderTopWidth = animatedBorderWidth;
                highlighter.style.borderBottomWidth = animatedBorderWidth;
            }

            foreach (var view in s_HighlightedViews)
            {
                if (view.Key.IsValid)
                {
                    view.Key.Repaint();
                }
            }
        }

        /// <summary>
        /// Unmasks all views.
        /// </summary>
        public static void Unmask()
        {
            foreach (var mask in s_Masks)
            {
                if (mask != null && mask.parent != null)
                {
                    mask.parent.Remove(mask);
                }
            }
            s_Masks.Clear();
            foreach (var highlighter in s_Highlighters)
            {
                if (highlighter != null && highlighter.parent != null)
                {
                    highlighter.parent.Remove(highlighter);
                }
            }
            s_Highlighters.Clear();
        }

        static void CopyMaskData(UnmaskedView.MaskData maskData, Dictionary<GUIViewProxy, MaskViewData> viewsAndResources)
        {
            viewsAndResources.Clear();
            foreach (var unmaskedView in maskData.m_MaskData)
            {
                if (unmaskedView.Key == null) { continue; }

                var maskViewData = unmaskedView.Value;
                var unmaskedRegions = maskViewData.rects == null ? new List<Rect>(1) : maskViewData.rects.ToList();
                if (unmaskedRegions.Count == 0)
                {
                    unmaskedRegions.Add(new Rect(0f, 0f, unmaskedView.Key.Position.width, unmaskedView.Key.Position.height));
                }
                viewsAndResources[unmaskedView.Key] = new MaskViewData
                {
                    maskType = maskViewData.maskType,
                    rects = unmaskedRegions,
                    EditorWindowType = maskViewData.EditorWindowType
                };
            }
        }

        /// <summary>
        /// Adds a mask for a view.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="child"></param>
        static void AddMaskToView(GUIViewProxy view, VisualElement child)
        {
            // Since 2019.3(?), we must suppress input to the elements behind masks.
            // TODO Doesn't suppress everything, e.g. tooltips are shown still.
            child.RegisterCallback<MouseDownEvent>((e) => e.StopPropagation());
            child.RegisterCallback<MouseUpEvent>((e) => e.StopPropagation());
            child.RegisterCallback<MouseMoveEvent>((e) => e.StopPropagation());
            child.RegisterCallback<WheelEvent>((e) => e.StopPropagation());
            child.RegisterCallback<PointerDownEvent>((e) => e.StopPropagation());
            child.RegisterCallback<PointerUpEvent>((e) => e.StopPropagation());
            child.RegisterCallback<PointerMoveEvent>((e) => e.StopPropagation());
            child.RegisterCallback<KeyDownEvent>((e) => e.StopPropagation());
            child.RegisterCallback<KeyUpEvent>((e) => e.StopPropagation());

            if (view.IsDockedToEditor())
            {
                UIElementsHelper.GetVisualTree(view).Add(child);
                return;
            }
            var viewVisualElement = UIElementsHelper.GetVisualTree(view);

            Debug.Assert(
                viewVisualElement.Children().Count() == 2
                && viewVisualElement.Children().Count(viewChild => viewChild is IMGUIContainer) == 1,
                "Could not find the expected VisualElement structure"
            );

            foreach (var visualElement in viewVisualElement.Children())
            {
                if (!(visualElement is IMGUIContainer))
                {
                    visualElement.Add(child);
                    break;
                }
            }
        }

        /// <summary>
        /// Applies masking.
        /// </summary>
        /// <param name="unmaskedViewsAndRegionsMaskData"></param>
        /// <param name="maskColor"></param>
        /// <param name="highlightedRegionsMaskData"></param>
        /// <param name="highlightColor"></param>
        /// <param name="blockedInteractionsColor"></param>
        /// <param name="highlightThickness"></param>
        public static void Mask(
            UnmaskedView.MaskData unmaskedViewsAndRegionsMaskData, Color maskColor,
            UnmaskedView.MaskData highlightedRegionsMaskData, Color highlightColor, Color blockedInteractionsColor, float highlightThickness
        )
        {
            Unmask();

            CopyMaskData(unmaskedViewsAndRegionsMaskData, s_UnmaskedViews);
            CopyMaskData(highlightedRegionsMaskData, s_HighlightedViews);

            List<GUIViewProxy> views = new List<GUIViewProxy>();
            GUIViewDebuggerHelperProxy.GetViews(views);

            foreach (var view in views)
            {
                if (!view.IsValid) { continue; }

                MaskViewData maskViewData;

                var viewRect = new Rect(0, 0, view.Position.width, view.Position.height);

                // mask everything except the unmasked view rects
                if (s_UnmaskedViews.TryGetValue(view, out maskViewData))
                {
                    // Beginning from 2021.2 the layout of floating/undocked EditorWindows has changed a bit and now contains
                    // an offset caused by the tab area which we need to take into account.
                    EditorWindow parentWindow = null;
                    if (maskViewData.EditorWindowType != null)
                    {
                        parentWindow = FindOpenEditorWindowInstance(maskViewData.EditorWindowType);
                    }

                    List<Rect> rects = maskViewData.rects;
                    var maskedRects = GetNegativeSpaceRects(viewRect, rects);
                    for (var i = 0; i < maskedRects.Count; ++i)
                    {
                        var rect = maskedRects[i];
                        if (parentWindow != null && !parentWindow.IsDocked())
                        {
                            // In theory we could have an X offset also but it seems highgly unlikely.
                            rect.y -= parentWindow.rootVisualElement.layout.y;
                        }
                        var mask = new VisualElement();
                        mask.style.backgroundColor = maskColor;
                        mask.SetLayout(rect);
                        AddMaskToView(view, mask);
                        s_Masks.Add(mask);
                    }

                    if (maskViewData.maskType == MaskType.BlockInteractions)
                    {
                        foreach (var rect in rects)
                        {
                            var mask = new VisualElement();
                            mask.style.backgroundColor = blockedInteractionsColor;
                            mask.SetLayout(rect);
                            AddMaskToView(view, mask);
                            s_Masks.Add(mask);
                        }
                    }
                }
                else // mask the whole view
                {
                    var mask = new VisualElement();
                    mask.style.backgroundColor = maskColor;
                    mask.SetLayout(viewRect);
                    AddMaskToView(view, mask);
                    s_Masks.Add(mask);
                }

                if (s_HighlightedViews.TryGetValue(view, out maskViewData))
                {
                    var rects = maskViewData.rects;
                    // unclip highlight to apply as "outer stroke" if it is being applied to some control(s) in the view
                    var unclip = rects.Count > 1 || rects[0] != viewRect;
                    var borderRadius = 5.0f;
                    foreach (var rect in rects)
                    {
                        var highlighter = new VisualElement();
#if UNITY_2019_3_OR_NEWER
                        highlighter.style.borderLeftColor = highlightColor;
                        highlighter.style.borderRightColor = highlightColor;
                        highlighter.style.borderTopColor = highlightColor;
                        highlighter.style.borderBottomColor = highlightColor;
#else
                        highlighter.style.borderColor = highlightColor;
#endif
                        highlighter.style.borderLeftWidth = highlightThickness;
                        highlighter.style.borderRightWidth = highlightThickness;
                        highlighter.style.borderTopWidth = highlightThickness;
                        highlighter.style.borderBottomWidth = highlightThickness;

                        highlighter.style.borderBottomLeftRadius = borderRadius;
                        highlighter.style.borderTopLeftRadius = borderRadius;
                        highlighter.style.borderBottomRightRadius = borderRadius;
                        highlighter.style.borderTopRightRadius = borderRadius;

                        highlighter.pickingMode = PickingMode.Ignore;
                        var layout = rect;
                        if (unclip)
                        {
                            layout.xMin -= highlightThickness;
                            layout.xMax += highlightThickness;
                            layout.yMin -= highlightThickness;
                            layout.yMax += highlightThickness;
                        }
                        highlighter.SetLayout(layout);
                        UIElementsHelper.GetVisualTree(view).Add(highlighter);
                        s_Highlighters.Add(highlighter);
                    }
                }
            }

            s_LastHighlightTime = EditorApplication.timeSinceStartup;
        }

        static EditorWindow FindOpenEditorWindowInstance(System.Type type) =>
            Resources.FindObjectsOfTypeAll(type).FirstOrDefault() as EditorWindow;

        static readonly HashSet<float> s_YCoords = new HashSet<float>();
        static readonly HashSet<float> s_XCoords = new HashSet<float>();

        static readonly List<float> s_SortedYCoords = new List<float>();
        static readonly List<float> s_SortedXCoords = new List<float>();

        internal static List<Rect> GetNegativeSpaceRects(Rect viewRect, List<Rect> positiveSpaceRects)
        {
            //TODO maybe its okay to round to int?

            s_YCoords.Clear();
            s_XCoords.Clear();

            for (int i = 0; i < positiveSpaceRects.Count; i++)
            {
                var hole = positiveSpaceRects[i];
                s_YCoords.Add(hole.y);
                s_YCoords.Add(hole.yMax);
                s_XCoords.Add(hole.x);
                s_XCoords.Add(hole.xMax);
            }

            s_YCoords.Add(0);
            s_YCoords.Add(viewRect.height);

            s_XCoords.Add(0);
            s_XCoords.Add(viewRect.width);

            s_SortedYCoords.Clear();
            s_SortedXCoords.Clear();

            s_SortedYCoords.AddRange(s_YCoords);
            s_SortedXCoords.AddRange(s_XCoords);

            s_SortedYCoords.Sort();
            s_SortedXCoords.Sort();

            var filledRects = new List<Rect>();

            for (var i = 1; i < s_SortedYCoords.Count; ++i)
            {
                var minY = s_SortedYCoords[i - 1];
                var maxY = s_SortedYCoords[i];
                var midY = (maxY + minY) / 2;
                var workingRect = new Rect(s_SortedXCoords[0], minY, 0, (maxY - minY));

                for (var j = 1; j < s_SortedXCoords.Count; ++j)
                {
                    var minX = s_SortedXCoords[j - 1];
                    var maxX = s_SortedXCoords[j];
                    var midX = (maxX + minX) / 2;

                    var potentialHole = positiveSpaceRects.Find((hole) => { return hole.Contains(new Vector2(midX, midY)); });
                    var cellIsHole = potentialHole.width > 0 && potentialHole.height > 0;

                    if (cellIsHole)
                    {
                        if (workingRect.width > 0 && workingRect.height > 0)
                        {
                            filledRects.Add(workingRect);
                        }

                        workingRect.x = maxX;
                        workingRect.xMax = maxX;
                    }
                    else
                    {
                        workingRect.xMax = maxX;
                    }
                }

                if (workingRect.width > 0 && workingRect.height > 0)
                {
                    filledRects.Add(workingRect);
                }
            }

            return filledRects;
        }
    }
}
