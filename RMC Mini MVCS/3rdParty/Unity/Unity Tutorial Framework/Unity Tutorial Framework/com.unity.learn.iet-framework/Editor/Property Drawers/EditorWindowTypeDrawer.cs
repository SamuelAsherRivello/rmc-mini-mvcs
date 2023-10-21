using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(EditorWindowType))]
    class EditorWindowTypeDrawer : FlushChildrenDrawer
    {
    }
}
