using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    internal static class WindowLayoutProxy
    {
        public static void SaveWindowLayout(string path)
        {
            WindowLayout.SaveWindowLayout(path);
        }

        public static object GetParentOf(EditorWindow window)
        {
            return window.m_Parent;
        }

        // TODO PerformDrop should be ideally in a new SplitViewProxy class.
        public static void PerformDrop(object window, EditorWindow child, Vector2 screenPoint)
        {
            PerformDrop(GetWindowOf(window), child, screenPoint);
        }

        static void PerformDrop(ContainerWindow window, EditorWindow child, Vector2 screenPoint)
        {
            SplitView splitView = window.rootSplitView;
            DropInfo dropInfo = splitView.DragOver(child, screenPoint);

            if (dropInfo == null || dropInfo.dropArea == null) //this 2nd case happens when the inspector is a child of another view (I.E: Scene View)
            {
                //this undocks the window in this particular edge case
                child.RemoveFromDockArea();
                EditorWindow.CreateNewWindowForEditorWindow(child, true, true, true);
                return;
            }
            splitView.PerformDrop(child, dropInfo, screenPoint); //seems to alway return true so no point in relaying this value, for now at least
        }

        static ContainerWindow GetWindowOf(object window)
        {
            return (window as HostView).window;
        }
    }
}
