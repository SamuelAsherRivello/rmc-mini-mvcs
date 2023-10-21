using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class UnmaskedViewTests
    {
        [Test]
        [Ignore("Annoyingly closes Test Runner window and clears test results of other tests")]
        public void TestGetViewsAndRects_ThrowsArgumentException_WhenTryingToGetRectsFromTwoEditorWindowsInTheSameDockArea()
        {
            Assert.True(
                EditorUtility.LoadWindowLayout("Packages/com.unity.learn.iet-framework/Tests/Editor/UnmaskedViewTestLayout.dwlt"),
                "UnmaskedViewTestLayout.dwlt missing."
            );

            // these two windows are docked together in the test layout
            var unmaskedViews = new[]
            {
                UnmaskedView.CreateInstanceForEditorWindow(typeof(SceneView)),
                UnmaskedView.CreateInstanceForEditorWindow(GUIViewProxy.GameViewType),
            };

            Assert.Throws<ArgumentException>(
                () => UnmaskedView.GetViewsAndRects(unmaskedViews),
                "Did not throw ArgumentException when getting rects for two EditorWindows in the same DockArea"
            );
        }

        static GuiControlSelector CreateGuiControlSelector(string namedControl, string visualElementName) =>
#if UNITY_2021_1_OR_NEWER
            new GuiControlSelector() { SelectorMode = GuiControlSelector.Mode.VisualElement, VisualElementName = visualElementName };
#else
            new GuiControlSelector() { SelectorMode = GuiControlSelector.Mode.NamedControl, ControlName = namedControl };
#endif

        // TODO Could split this into two tests, one for Toolbar and one for SceneView
        [Ignore("TODO Runs fine locally, fails on Yamato.")]
        [Test]
        public void TestGetViewsAndRects_ForNamedControlsInToolbar()
        {
            // 0) Setup
            var toolbarType = GUIViewProxy.ToolbarType;
            var sceneViewType = typeof(SceneView);
            var umaskedControls = new Dictionary<Type, List<GuiControlSelector>>
            {
                { toolbarType, new List<GuiControlSelector>() },
                { sceneViewType, new List<GuiControlSelector>() },
            };

            var playModeControls = new[]
            {
                CreateGuiControlSelector("ToolbarPlayModePlayButton", "Play"),
                CreateGuiControlSelector("ToolbarPlayModePauseButton", "Pause"),
                CreateGuiControlSelector("ToolbarPlayModeStepButton", "Step")
            };

            // These controls are still in toolbar regardless of Unity version
            umaskedControls[toolbarType].AddRange(playModeControls);

            var sceneToolControls = new[]
            {
                CreateGuiControlSelector("ToolbarPersistentToolsPan", "ViewTool"),
                CreateGuiControlSelector("ToolbarPersistentToolsTranslate", "MoveTool"),
                CreateGuiControlSelector("ToolbarPersistentToolsRotate", "RotateTool"),
                CreateGuiControlSelector("ToolbarPersistentToolsScale", "ScaleTool"),
                CreateGuiControlSelector("ToolbarPersistentToolsRect", "RectTool"),
                CreateGuiControlSelector("ToolbarToolPivotPositionButton", "Pivot Mode"),
                CreateGuiControlSelector("ToolbarToolPivotOrientationButton", "Pivot Rotation"),
            };

            // These controls are now in the Scene view instead of Toolbar beginning from 2021.1
#if UNITY_2021_1_OR_NEWER
            umaskedControls[sceneViewType].AddRange(sceneToolControls);
#else
            umaskedControls[toolbarType].AddRange(sceneToolControls);
#endif

            // 1) Toolbar tests
            var unmaskedToolbarViews = new List<UnmaskedView>();
            var unmaskedToolbarControls = umaskedControls[toolbarType];
            if (unmaskedToolbarControls.Any())
                unmaskedToolbarViews.Add(UnmaskedView.CreateInstanceForGUIView(toolbarType, unmaskedToolbarControls));

            var viewsAndRects = UnmaskedView.GetViewsAndRects(unmaskedToolbarViews).m_MaskData;
            Assert.AreEqual(unmaskedToolbarViews.Count, viewsAndRects.Count, "Did not find expected amount of views for the Toolbar views");
            if (viewsAndRects.Count > 0)
            {
                var rects = viewsAndRects.Values.FirstOrDefault().rects;
                Assert.AreEqual(unmaskedToolbarControls.Count(), rects.Count, "Did not find all of the expected controls in the Toolbar");
            }

            // 2) SceneView tests
            var unmaskedSceneViewViews = new List<UnmaskedView>();
            var unmaskedSceneViewControls = umaskedControls[sceneViewType];
            if (unmaskedSceneViewControls.Any())
                unmaskedSceneViewViews.Add(UnmaskedView.CreateInstanceForEditorWindow(sceneViewType, unmaskedSceneViewControls));

            viewsAndRects = UnmaskedView.GetViewsAndRects(unmaskedSceneViewViews).m_MaskData;
            Assert.AreEqual(unmaskedSceneViewViews.Count, viewsAndRects.Count, "Did not find expected amount of views for the SceneView views");
            if (viewsAndRects.Count > 0)
            {
                var rects = viewsAndRects.Values.FirstOrDefault().rects;
                Assert.AreEqual(unmaskedSceneViewControls.Count(), rects.Count, "Did not find all of the expected controls in the SceneView");
            }
        }
#if UNITY_EDITOR_LINUX
        [Ignore("Unstable on Linux, fails non-deterministically")]
#endif
        [UnityTest]
        public IEnumerator TestGetViewsAndRects_ForSerializedPropertyInInspector()
        { 
            var testObject = new GameObject("TestGetViewsAndRects_ForSerializedPropertiesInInspector");
            Selection.activeObject = testObject;
            try
            {
                EditorWindow.GetWindow(GUIViewProxy.InspectorWindowType);
                yield return null;

                var unmaskedViews = new[]
                {
                    UnmaskedView.CreateInstanceForEditorWindow(
                        GUIViewProxy.InspectorWindowType,
                        new[]
                        {
                            new GuiControlSelector() { SelectorMode = GuiControlSelector.Mode.Property, TargetType = typeof(Transform), PropertyPath = "m_LocalPosition" }
                        }
                    )
                };
                var viewsAndRects = UnmaskedView.GetViewsAndRects(unmaskedViews).m_MaskData;
                Assert.AreEqual(1, viewsAndRects.Count, "Did not find one view for the Inspector");
                var rects = viewsAndRects.Values.First().rects;
                Assert.AreEqual(1, rects.Count, "Did not find exactly one control for the SerializedProperty m_LocalPosition for a Transform");
            }
            finally
            {
                GameObject.DestroyImmediate(testObject);
            }
        }

#if UNITY_EDITOR_LINUX
        [Ignore("Unstable on Linux, fails non-deterministically")]
#endif
        [UnityTest]
        public IEnumerator TestGetViewsAndRects_ForSerializedPropertyInInspector_WhenSamePathExistsOnMultipleComponents()
        {
            var testObject = new GameObject("TestGetViewsAndRects_ForSerializedPropertiesInInspector", typeof(Light), typeof(SpriteRenderer));

            Selection.activeObject = testObject;
            try
            {
                Assert.IsNotNull(new SerializedObject(testObject.GetComponent<Light>()).FindProperty("m_Color"));
                Assert.IsNotNull(new SerializedObject(testObject.GetComponent<SpriteRenderer>()).FindProperty("m_Color"));

                EditorWindow.GetWindow(GUIViewProxy.InspectorWindowType);
                yield return null;

                var unmaskedViews = new[]
                {
                    UnmaskedView.CreateInstanceForEditorWindow(
                        GUIViewProxy.InspectorWindowType,
                        new[]
                        {
                            new GuiControlSelector() { SelectorMode = GuiControlSelector.Mode.Property, TargetType = typeof(SpriteRenderer), PropertyPath = "m_Color" }
                        }
                    )
                };
                var viewsAndRects = UnmaskedView.GetViewsAndRects(unmaskedViews).m_MaskData;
                Assert.AreEqual(1, viewsAndRects.Count, "Did not find one view for the Inspector");
                var rects = viewsAndRects.Values.First().rects;
                Assert.AreEqual(1, rects.Count, "Did not find exactly one control for the SerializedProperty m_Color for a SpriteRenderer");
            }
            finally
            {
                GameObject.DestroyImmediate(testObject);
            }
        }

#if TODO_UIElements_implementation
        [UnityTest]
        [TestCase(true, false, ExpectedResult = null)]
#if UNITY_2019_3_OR_NEWER
        [Ignore("TODO Fails on 2019.3 due two-pixel difference on the rect.")]
#endif
        [TestCase(false, true, ExpectedResult = null)]
        public IEnumerator TestGetViewsAndRects_ForSerializedPropertyInInspector_WhenParentPropertyIsCollapsed(
            bool parentPropertyExpanded, bool expectedFoundAncestorProperty)
        {
            var testObject = new GameObject("TestGetViewsAndRects_ForContractedPropertyInInspector",
                typeof(TestComponents.ComponentWithNestedValues));

            Selection.activeObject = testObject;
            try
            {
                var serializedObject = new SerializedObject(testObject.GetComponent<TestComponents.ComponentWithNestedValues>());
                var parentProperty = serializedObject.FindProperty("componentWithNestedValuesFieldA");
                var childProperty = serializedObject.FindProperty(
                    "componentWithNestedValuesFieldA.componentWithNestedValuesFieldB");

                Assert.That(parentProperty, Is.Not.Null);
                Assert.That(childProperty, Is.Not.Null);

                parentProperty.isExpanded = parentPropertyExpanded;
                serializedObject.ApplyModifiedProperties();

                var inspectorWindow = EditorWindow.GetWindow<InspectorWindow>();

                Rect labelRectOfExpectedFoundProperty;
                using (var automatedWindow = new AutomatedWindow<InspectorWindow>(inspectorWindow))
                {
                    inspectorWindow.Repaint();
                    yield return null;

                    if (expectedFoundAncestorProperty)
                    {
                        var parentElements = automatedWindow.FindElementsByGUIContent(
                            new GUIContent("Component With Nested Values Field A"));
                        Assert.That(parentElements.Count(), Is.EqualTo(1));
                        labelRectOfExpectedFoundProperty = parentElements.Single().rect;
                    }
                    else
                    {
                        var childElements = automatedWindow.FindElementsByGUIContent(
                            new GUIContent("Component With Nested Values Field B"));
                        Assert.That(childElements.Count(), Is.EqualTo(1));
                        labelRectOfExpectedFoundProperty = childElements.Single().rect;
                    }
                }

                var unmaskedViews = new[]
                {
                    UnmaskedView.CreateInstanceForEditorWindow<InspectorWindow>(
                        new[]
                        {
                            new GuiControlSelector()
                            {
                                SelectorMode = GuiControlSelector.Mode.Property,
                                TargetType = typeof(TestComponents.ComponentWithNestedValues),
                                PropertyPath = "componentWithNestedValuesFieldA.componentWithNestedValuesFieldB"
                            }
                        }
                    )
                };

                bool foundAncestorProperty;
                var viewsAndRects = UnmaskedView.GetViewsAndRects(unmaskedViews, out foundAncestorProperty)
                    .m_MaskData;

                Assert.That(foundAncestorProperty, Is.EqualTo(expectedFoundAncestorProperty));
                Assert.That(viewsAndRects.Count, Is.EqualTo(1), "Did not find one view for the Inspector");

                var rects = viewsAndRects.Values.First().rects;
                Assert.That(rects.Count, Is.EqualTo(1),
                    "Did not find exactly one control for the SerializedPropert " +
                    "componentWithNestedValuesFieldA.componentWithNestedValuesFieldB for ComponentWithNestedValues");

                var rect = rects.Single();
                Assert.That(rect.yMin, Is.GreaterThanOrEqualTo(labelRectOfExpectedFoundProperty.yMin),
                    "Found property rect does not contain of label rect of expected found property");
                Assert.That(rect.yMax, Is.LessThanOrEqualTo(labelRectOfExpectedFoundProperty.yMax),
                    "Found property rect does not contain of label rect of expected found property");
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(testObject);
            }
        }

#endif
    }
}
