using System;
using UnityEditor;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Proxy class for accessing UnityEditor.EditorWindow.
    /// </summary>
    [Obsolete("Will be removed in v4. Use EditorWindow instead")] //todo: remove in v4
    public class EditorWindowProxy : EditorWindow
    {
        internal override void OnResized()
        {
            base.OnResized();
            OnResized_Internal();
        }

        /// <summary>
        /// Called upon EditorWindow.OnResized(), after the base class' implementation.
        /// </summary>
        protected virtual void OnResized_Internal()
        {
        }

        /// <summary>
        /// Is the parent of this window null.
        /// </summary>
        /// <returns></returns>
        protected bool IsParentNull()
        {
            return m_Parent == null;
        }
    }
}
