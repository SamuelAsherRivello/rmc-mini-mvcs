using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// An utility class for common UIElements setup method
    /// </summary>
    internal static class UIElementsUtils
    {
        internal static readonly string s_UIResourcesPath = $"Packages/{FrameworkSettings.k_PackageName}/Editor/UI/";

        /// <summary>
        /// Loads an asset from the common UI resource folder.
        /// </summary>
        /// <typeparam name="T">type fo the file to load</typeparam>
        /// <param name="filename">name of the file</param>
        /// <returns>A reference to the loaded file</returns>
        internal static T LoadUIAsset<T>(string filename) where T : UnityObject => AssetDatabase.LoadAssetAtPath<T>($"{s_UIResourcesPath}/{filename}");

        internal static Button SetupButton(string buttonName, Action onClickAction, bool isEnabled, VisualElement parent, string text = "", string tooltip = "", bool showIfEnabled = true, bool localize = false)
        {
            Button button = parent.Query<Button>(buttonName);
            button.SetEnabled(isEnabled);
            button.clickable = new Clickable(() => onClickAction?.Invoke());
            button.text = localize ? Localization.Tr(text) : text;
            button.tooltip = string.IsNullOrEmpty(tooltip) ? button.text : tooltip;

            if (showIfEnabled && isEnabled)
            {
                Show(button);
            }

            return button;
        }

        internal static Label SetupLabel(string labelName, string text, VisualElement parent, bool localize, Manipulator manipulator = null)
        {
            Label label = parent.Query<Label>(labelName);
            label.text = localize ? Localization.Tr(text) : text;
            if (manipulator != null)
            {
                label.AddManipulator(manipulator);
            }

            return label;
        }

        internal static Foldout SetupFoldout(string name, string text, VisualElement parent, bool localize, bool open)
        {
            Foldout foldout = parent.Query<Foldout>(name);
            foldout.text = localize ? Localization.Tr(text) : text;
            foldout.value = open;
            return foldout;
        }

        internal static EnumField SetupEnumField<T>(string enumName, string text, EventCallback<ChangeEvent<Enum>> onValueChanged, VisualElement parent, T defaultValue, bool localize) where T : Enum
        {
            EnumField uxmlField = parent.Q<EnumField>(enumName);
            uxmlField.label = localize ? Localization.Tr(text) : text;
            uxmlField.Init(defaultValue);
            uxmlField.value = defaultValue;
            uxmlField.RegisterCallback(onValueChanged);
            return uxmlField;
        }

        internal static EnumFlagsField SetupEnumFlagField<T>(string enumName, string text, EventCallback<ChangeEvent<Enum>> onValueChanged, VisualElement parent, T defaultValue, bool localize) where T : Enum
        {
            var uxmlField = parent.Q<EnumFlagsField>(enumName);
            uxmlField.label = localize ? Localization.Tr(text) : text;
            uxmlField.Init(defaultValue);
            uxmlField.value = defaultValue;
            uxmlField.RegisterCallback(onValueChanged);
            return uxmlField;
        }

        internal static void SetupObjectField<T>(string fieldName, EventCallback<ChangeEvent<UnityEngine.Object>> onValueChanged, VisualElement parent, T defaultValue) where T : UnityEngine.Object
        {
            ObjectField spriteField = parent.Q<ObjectField>(fieldName);
            spriteField.objectType = typeof(T);
            spriteField.value = defaultValue;
            spriteField.RegisterCallback(onValueChanged);
        }

        internal static Toggle SetupToggle(string name, string label, string text, bool defaultValue, EventCallback<ChangeEvent<bool>> onValueChanged, VisualElement parent, bool localize)
        {
            Toggle uxmlField = parent.Q<Toggle>(name);
            uxmlField.label = localize ? Localization.Tr(label) : label;
            uxmlField.text = localize ? Localization.Tr(text) : text;
            uxmlField.value = defaultValue;
            uxmlField.SetEnabled(true);
            uxmlField.RegisterCallback(onValueChanged);
            return uxmlField;
        }

        internal static ToolbarSearchField SetupToolbarSearchField(string name, EventCallback<ChangeEvent<string>> onValueChanged, VisualElement parent)
        {
            ToolbarSearchField uxmlField = parent.Q<ToolbarSearchField>(name);
            uxmlField.value = string.Empty;
            uxmlField.SetEnabled(true);
            uxmlField.RegisterCallback(onValueChanged);
            return uxmlField;
        }

        internal static IntegerField SetupIntegerField(string name, int value, EventCallback<ChangeEvent<int>> onValueChanged, VisualElement parent)
        {
            IntegerField uxmlField = parent.Q<IntegerField>(name);
            uxmlField.value = value;
            uxmlField.SetEnabled(true);
            uxmlField.RegisterCallback(onValueChanged);
            return uxmlField;
        }

        internal static TextField SetupStringField(string name, string localizationKey, string value, EventCallback<ChangeEvent<string>> onValueChanged, VisualElement parent, bool localize)
        {
            TextField uxmlField = parent.Q<TextField>(name);
            uxmlField.label = localize ? Localization.Tr(localizationKey) : localizationKey;
            uxmlField.value = value;
            uxmlField.SetEnabled(true);
            uxmlField.RegisterCallback(onValueChanged);
            return uxmlField;
        }

        internal static void ShowOrHide(string elementName, VisualElement parent, bool show)
        {
            if (show)
            {
                Show(elementName, parent);
                return;
            }
            Hide(elementName, parent);
        }

        internal static void Hide(string elementName, VisualElement parent)
        {
            Hide(parent.Query<VisualElement>(elementName));
        }

        internal static void Show(string elementName, VisualElement parent)
        {
            Show(parent.Query<VisualElement>(elementName));
        }

        internal static void Hide(VisualElement element)
        {
            element.style.display = DisplayStyle.None;
        }

        internal static void Show(VisualElement element)
        {
            element.style.display = DisplayStyle.Flex;
        }

        internal static VisualTreeAsset LoadUXML(string fileName)
        {
            string path = $"{s_UIResourcesPath}{fileName}.uxml";
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        }

        internal static void LoadSkinStyleSheet(out StyleSheet styleSheet, VisualElement target)
        {
            string theme = EditorGUIUtility.isProSkin ? "_Dark" : "_Light";
            string themedStyleSheet = $"{s_UIResourcesPath}Styles{theme}.uss";
            styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(themedStyleSheet);
            target.styleSheets.Add(styleSheet);
        }

        internal static void LoadCommonStyleSheet(VisualElement target)
        {
            string commonStyleSheetFilePath = $"{s_UIResourcesPath}Styles.uss";
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(commonStyleSheetFilePath);
            target.styleSheets.Add(styleSheet);
        }

        internal static void RemoveStyleSheet(StyleSheet styleSheet, VisualElement target)
        {
            if (!styleSheet) { return; }

            if (!target.styleSheets.Contains(styleSheet)) { return; }

            target.styleSheets.Remove(styleSheet);
        }
    }


    /// <summary>
    /// Represents a MouseManipulator that allows a visual element to react when left clicked
    /// </summary>
    internal class LeftClickManipulator : MouseManipulator
    {
        Action<VisualElement> m_OnClick;
        bool m_Active;

        /// <summary>
        /// Initializes and returns an instance of LeftClickManipulator.
        /// </summary>
        /// <param name="OnClick">The default callback that will be triggered when the element is clicked</param>
        public LeftClickManipulator(Action<VisualElement> OnClick)
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            m_OnClick = OnClick;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        }

        /// <summary>
        /// Called when the mouse is clicked on the target, when the user starts pressing the button
        /// </summary>
        /// <param name="e"></param>
        protected void OnMouseDown(MouseDownEvent e)
        {
            if (m_Active)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(e))
            {
                m_Active = true;
                target.CaptureMouse();
                e.StopPropagation();
            }
        }

        /// <summary>
        /// Called when the mouse is clicked on the target, when the user stops pressing the button
        /// </summary>
        /// <param name="e"></param>
        protected void OnMouseUp(MouseUpEvent e)
        {
            if (!m_Active || !target.HasMouseCapture() || !CanStopManipulation(e)) { return; }

            m_Active = false;
            target.ReleaseMouse();
            e.StopPropagation();

            if (m_OnClick == null) { return; }
            m_OnClick.Invoke(target);
        }
    }
}
