using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Tutorials.Core.Editor
{
    [Serializable]
    internal class MaskingSettings
    {
        public bool Enabled { get => m_MaskingEnabled; set => m_MaskingEnabled = value; }
        [SerializeField, FormerlySerializedAs("m_Enabled")]
        bool m_MaskingEnabled;

        public IEnumerable<UnmaskedView> UnmaskedViews => m_UnmaskedViews;
        [SerializeField]
        List<UnmaskedView> m_UnmaskedViews = new List<UnmaskedView>();

        public void SetUnmaskedViews(IEnumerable<UnmaskedView> unmaskedViews)
        {
            m_UnmaskedViews.Clear();
            if (unmaskedViews != null)
                m_UnmaskedViews.AddRange(unmaskedViews);
        }
    }
}
