using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Controls masking and highlighting styles, and style sheets for the tutorials.
    /// </summary>
    public class TutorialStyles : ScriptableObject
    {
        /// <summary>
        /// Color of the masking overlay.
        /// </summary>
        public Color MaskingColor => m_MaskingColor;
        [Header("Masking and Highlighting")]
        [SerializeField]
        Color m_MaskingColor = new Color32(0, 40, 53, 204);

        /// <summary>
        /// Color of the highlight border.
        /// </summary>
        public Color HighlightColor => m_HighlightColor;
        [SerializeField]
        Color m_HighlightColor = new Color32(0, 198, 223, 255);

        /// <summary>
        /// Color of the blocked interaction overlay.
        /// </summary>
        public Color BlockedInteractionColor => m_BlockedInteractionColor;
        [SerializeField]
        Color m_BlockedInteractionColor = new Color(1, 1, 1, 0.5f);

        /// <summary>
        /// Thickness of the highlight border in pixels.
        /// </summary>
        public float HighlightThickness => m_HighlightThickness;
        [SerializeField, Range(0f, 10f)]
        float m_HighlightThickness = 3f;

        [SerializeField, Range(0f, 10f)]
        float m_HighlightAnimationSpeed = 1.5f;

        [SerializeField, Range(0f, 10f)]
        float m_HighlightAnimationDelay = 5f;

        /// <summary>
        /// Used when the Personal Editor Theme is chosen.
        /// </summary>
        [Header("Style Sheets")]
        [Tooltip("Used when the Personal Editor Theme is chosen.")]
        public StyleSheet LightThemeStyleSheet;

        /// <summary>
        /// Used when the Professional Editor Theme is chosen.
        /// </summary>
        [Tooltip("Used when the Professional Editor Theme is chosen.")]
        public StyleSheet DarkThemeStyleSheet;

        StyleSheet m_LastCommonStyleSheet;

        /// <summary>
        /// The default style sheet file used when the Personal Editor Theme is chosen. Deprecated.
        /// </summary>
        public static readonly string DefaultLightStyleFile = $"{UIElementsUtils.s_UIResourcesPath}/Styles_Light.uss";

        /// <summary>
        /// The default style sheet file used when the Professional Editor Theme is chosen. Deprecated.
        /// </summary>
        public static readonly string DefaultDarkStyleFile = $"{UIElementsUtils.s_UIResourcesPath}/Styles_Dark.uss";

        void OnEnable()
        {
            Apply();
        }

        void OnValidate()
        {
            Apply();
        }

        void Apply()
        {
            MaskingManager.HighlightAnimationSpeed = m_HighlightAnimationSpeed;
            MaskingManager.HighlightAnimationDelay = m_HighlightAnimationDelay;
        }

        /// <summary>
        /// Applies a Theme-specific style to a VisualElement, removing all other styles except the base one
        /// </summary>
        /// <param name="target">VisualElement to which the style should apply (usually, you want to do this to the root)</param>
        public void ApplyThemeStyleSheetTo(VisualElement target)
        {
            //preserve the base style, remove all styles defined in UXML and apply new skin
            StyleSheet baseStyle = target.styleSheets[0];
            target.styleSheets.Clear();
            target.styleSheets.Add(baseStyle);
            AddThemeStyleTo(target);
        }

        static void RemoveStyleSheet(StyleSheet styleSheet, VisualElement target)
        {
            if (styleSheet && target.styleSheets.Contains(styleSheet))
                target.styleSheets.Remove(styleSheet);
        }

        /// <summary>
        /// Adds a Theme-specific style to a VisualElement
        /// </summary>
        /// <param name="target">VisualElement to which the style should be added (usually, you want to do this to the root)</param>
        void AddThemeStyleTo(VisualElement target)
        {
            RemoveStyleSheet(m_LastCommonStyleSheet, target);

            m_LastCommonStyleSheet = EditorGUIUtility.isProSkin ? DarkThemeStyleSheet : LightThemeStyleSheet;
            if (!m_LastCommonStyleSheet)
            {
                string theme = EditorGUIUtility.isProSkin ? "_Dark" : "_Light";
                m_LastCommonStyleSheet = UIElementsUtils.LoadUIAsset<StyleSheet>(string.Format("Styles{0}.uss", theme));
                if (!m_LastCommonStyleSheet)
                {
                    Debug.LogErrorFormat("Default Stylesheet for theme {0} is null", theme);
                    return;
                }
            }
            target.styleSheets.Add(m_LastCommonStyleSheet);
        }
    }
}
