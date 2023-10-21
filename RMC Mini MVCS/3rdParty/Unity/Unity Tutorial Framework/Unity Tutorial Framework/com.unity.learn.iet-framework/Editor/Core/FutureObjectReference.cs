using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Future Object Reference is a reference to a Unity Object that might not exist yet (prefab instance).
    /// </summary>
    public class FutureObjectReference : ScriptableObject
    {
        [SerializeField]
        SceneObjectReferenceHolder m_ReferenceHolder;

        [SerializeField]
        Criterion m_Criterion;

        [SerializeField]
        string m_ReferenceName;

        /// <summary>
        /// Returns the SceneObjectReferenceHolder for this FutureObjectReference.
        /// Creates the SceneObjectReferenceHolder instance if it does not exist.
        /// </summary>
        SceneObjectReferenceHolder ReferenceHolder
        {
            get
            {
                if (m_ReferenceHolder == null)
                {
                    m_ReferenceHolder = CreateInstance<SceneObjectReferenceHolder>();
                    m_ReferenceHolder.hideFlags = HideFlags.HideAndDontSave;
                }

                return m_ReferenceHolder;
            }
        }

        /// <summary>
        /// The SceneObjectReference of this FutureObjectReference.
        /// </summary>
        public SceneObjectReference SceneObjectReference
        {
            get
            {
                if (ReferenceHolder.SceneObjectReference == null)
                    ReferenceHolder.SceneObjectReference = new SceneObjectReference();

                return ReferenceHolder.SceneObjectReference;
            }
            set
            {
                ReferenceHolder.SceneObjectReference = value;
            }
        }

        /// <summary>
        /// The Criterion this FutureObjectReference belongs to.
        /// </summary>
        public Criterion Criterion { get => m_Criterion; set => m_Criterion = value; }

        /// <summary>
        /// The name used to refer the Unity Object.
        /// </summary>
        public string ReferenceName { get => m_ReferenceName; set => m_ReferenceName = value; }

        void OnDestroy()
        {
            if (m_ReferenceHolder != null)
                DestroyImmediate(m_ReferenceHolder);
        }
    }

    /// <summary>
    /// SceneObjectReference holder.
    /// </summary>
    public class SceneObjectReferenceHolder : ScriptableObject
    {
        /// <summary>
        /// The ScenObjectReference.
        /// </summary>
        public SceneObjectReference SceneObjectReference { get => m_SceneObjectReference; set => m_SceneObjectReference = value; }
        [SerializeField]
        SceneObjectReference m_SceneObjectReference;
    }
}
