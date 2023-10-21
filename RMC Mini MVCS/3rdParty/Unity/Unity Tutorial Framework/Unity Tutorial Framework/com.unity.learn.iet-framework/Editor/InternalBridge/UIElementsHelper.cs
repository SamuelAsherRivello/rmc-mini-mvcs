using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

using UnityEngine.UIElements;

namespace Unity.Tutorials.Core.Editor
{
    // Handle difference in UIElements/UIToolkit APIs between different Unity versions.
    // Initialize on load to surface potential reflection issues immediately.
    // TODO unit tests for these would be good to have
    [InitializeOnLoad]
    internal static class UIElementsHelper
    {
        private static Assembly GetAssemblyByName(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.GetName().Name.Equals(name))
                    return assembly;
            }

            return null;
        }

        static PropertyInfo s_VisualTreeProperty;

        static UIElementsHelper()
        {
#if UNITY_2021_2_OR_NEWER
            // Somewhere between 2021.2.0a and 2021.2.0b the internals changed.
            // Make sure to get the type from UnityEditor assemblies, not from UnityEngine.
            var uiElementsModule = GetAssemblyByName("UnityEditor.UIElementsModule");
            // DefaultWindowBackend is a private class so cannot access the type directly.
            var defaultWindowBackendType = uiElementsModule.GetType("UnityEditor.UIElements.DefaultWindowBackend");
            s_VisualTreeProperty = GetProperty(defaultWindowBackendType, "visualTree", typeof(object));
#endif
#if UNITY_2020_1_OR_NEWER
            // We might end up here on 2021.2.0a
            s_VisualTreeProperty = s_VisualTreeProperty ?? GetProperty<IWindowBackend>("visualTree", typeof(object));
#else
            s_VisualTreeProperty = GetProperty<GUIView>("visualTree", typeof(VisualElement))
                ?? GetProperty<GUIView>("visualTree", typeof(VisualElement));
#endif
            if (s_VisualTreeProperty == null)
                Debug.LogError("Cannot find property GUIView/IWindowBackend.visualTree");
        }

        static PropertyInfo GetProperty(Type type, string name, Type returnType)
        {
            return type.GetProperty(
                name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                returnType,
                new Type[0],
                null
            );
        }

        static PropertyInfo GetProperty<T>(string name, Type returnType) =>
            GetProperty(typeof(T), name, returnType);

        public static void Add(GUIViewProxy view, VisualElement child)
        {
            if (view.IsDockedToEditor())
            {
                GetVisualTree(view).Add(child);
            }
            else
            {
                foreach (var visualElement in GetVisualTree(view).Children())
                {
                    visualElement.Add(child);
                }
            }
        }

        public static VisualElement GetVisualTree(GUIViewProxy guiViewProxy) => GetVisualTree(guiViewProxy.GuiView);

        public static VisualElement GetVisualTree(GUIView guiView)
        {
            return (VisualElement)s_VisualTreeProperty.GetValue(
#if UNITY_2020_1_OR_NEWER
                guiView.windowBackend,
#else
                guiView,
#endif
                new object[0]
            );
        }

        public static void SetLayout(this VisualElement element, Rect rect)
        {
            var style = element.style;
            style.position = Position.Absolute;
            style.marginLeft = 0.0f;
            style.marginRight = 0.0f;
            style.marginBottom = 0.0f;
            style.marginTop = 0.0f;
            style.left = rect.x;
            style.top = rect.y;
            style.right = float.NaN;
            style.bottom = float.NaN;
            style.width = rect.width;
            style.height = rect.height;
        }
    }
}
