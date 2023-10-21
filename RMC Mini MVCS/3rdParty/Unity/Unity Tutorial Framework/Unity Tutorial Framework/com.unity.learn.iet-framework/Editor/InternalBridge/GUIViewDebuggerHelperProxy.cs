using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    internal static class GUIViewDebuggerHelperProxy
    {
        public static void GetViews(List<GUIViewProxy> views)
        {
            var guiViews = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(guiViews);
            views.AddRange(guiViews.Select(view => new GUIViewProxy(view)));
        }

        public static void DebugWindow(GUIViewProxy guiViewProxy)
        {
            GUIViewDebuggerHelper.DebugWindow(guiViewProxy.GuiView);
        }

        public static void GetDrawInstructions(List<IMGUIDrawInstructionProxy> drawInstructions)
        {
            var instructions = new List<IMGUIDrawInstruction>();
            GUIViewDebuggerHelper.GetDrawInstructions(instructions);
            drawInstructions.AddRange(instructions.Select(i => new IMGUIDrawInstructionProxy(i)));
        }

        public static void GetNamedControlInstructions(List<IMGUINamedControlInstructionProxy> namedControlInstructions)
        {
            var instructions = new List<IMGUINamedControlInstruction>();
            GUIViewDebuggerHelper.GetNamedControlInstructions(instructions);
            namedControlInstructions.AddRange(instructions.Select(i => new IMGUINamedControlInstructionProxy(i)));
        }

        public static void GetPropertyInstructions(List<IMGUIPropertyInstructionProxy> propertyInstructions)
        {
            var instructions = new List<IMGUIPropertyInstruction>();
            GUIViewDebuggerHelper.GetPropertyInstructions(instructions);
            propertyInstructions.AddRange(instructions.Select(i => new IMGUIPropertyInstructionProxy(i)));
        }

        public static void GetUnifiedInstructions(List<IMGUIInstructionProxy> unifiedInstructions)
        {
            var instructions = new List<IMGUIInstruction>();
            GUIViewDebuggerHelper.GetUnifiedInstructions(instructions);
            unifiedInstructions.AddRange(instructions.Select(i => new IMGUIInstructionProxy(i)));
        }

        public static void StopDebugging()
        {
            GUIViewDebuggerHelper.StopDebugging();
        }
    }

    internal class IMGUIDrawInstructionProxy
    {
        IMGUIDrawInstruction m_IMGUIDrawInstruction;

        internal IMGUIDrawInstructionProxy(IMGUIDrawInstruction imguiDrawInstruction)
        {
            m_IMGUIDrawInstruction = imguiDrawInstruction;
        }

        public GUIContent usedGUIContent => m_IMGUIDrawInstruction.usedGUIContent;
        public Rect rect => m_IMGUIDrawInstruction.rect;
        public string usedGUIStyleName => m_IMGUIDrawInstruction.usedGUIStyle.name;
    }

    internal class IMGUINamedControlInstructionProxy
    {
        IMGUINamedControlInstruction m_IMGUINamedControlInstructionProxy;

        internal IMGUINamedControlInstructionProxy(IMGUINamedControlInstruction imguiNamedControlInstruction)
        {
            m_IMGUINamedControlInstructionProxy = imguiNamedControlInstruction;
        }

        public string name => m_IMGUINamedControlInstructionProxy.name;
        public Rect rect => m_IMGUINamedControlInstructionProxy.rect;
    }

    internal class IMGUIPropertyInstructionProxy
    {
        IMGUIPropertyInstruction m_IMGUIPropertyInstruction;

        internal IMGUIPropertyInstructionProxy(IMGUIPropertyInstruction imguiPropertyInstruction)
        {
            m_IMGUIPropertyInstruction = imguiPropertyInstruction;
        }

        public string targetTypeName => m_IMGUIPropertyInstruction.targetTypeName;
        public string path => m_IMGUIPropertyInstruction.path;
        public Rect rect => m_IMGUIPropertyInstruction.rect;
    }

    internal enum InstructionTypeProxy
    {
        StyleDraw = 1,
        ClipPush = 2,
        ClipPop = 3,
        LayoutBeginGroup = 4,
        LayoutEndGroup = 5,
        LayoutEntry = 6,
        PropertyBegin = 7,
        PropertyEnd = 8,
        LayoutNamedControl = 9,
    }

    internal class IMGUIInstructionProxy
    {
        IMGUIInstruction m_IMGUIInstruction;

        internal IMGUIInstructionProxy(IMGUIInstruction imguiInstruction)
        {
            m_IMGUIInstruction = imguiInstruction;
        }

        public InstructionTypeProxy type => (InstructionTypeProxy)m_IMGUIInstruction.type;
        public int level => m_IMGUIInstruction.level;
        public int typeInstructionIndex => m_IMGUIInstruction.typeInstructionIndex;
    }
}
