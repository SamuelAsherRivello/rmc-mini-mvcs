using System;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Used to refer Unity Objects in different Criterion implementations.
    /// </summary>
    [Serializable]
    public class ObjectReference
    {
        [SerializeField]
        SceneObjectReference m_SceneObjectReference;

        [SerializeField]
        FutureObjectReference m_FutureObjectReference = default;

        /// <summary>
        /// Is this ObjectReference a FutureObjectReference instead of a SceneObjectReference;.
        /// </summary>
        public bool IsFutureReference => m_FutureObjectReference != null;

        /// <summary>
        /// The SceneObjectReference.
        /// </summary>
        public SceneObjectReference SceneObjectReference
        {
            get
            {
                if (IsFutureReference)
                    return m_FutureObjectReference.SceneObjectReference;
                return m_SceneObjectReference ?? (m_SceneObjectReference = new SceneObjectReference());
            }
            set
            {
                if (!IsFutureReference)
                    m_SceneObjectReference = value;
            }
        }
    }
}
