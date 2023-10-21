using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Holder for SerializedType and Criterion.
    /// </summary>
    [Serializable]
    public class TypedCriterion
    {
        /// <summary>
        /// The Type.
        /// </summary>
        [SerializeField, FormerlySerializedAs("type")]
        [SerializedTypeFilter(typeof(Criterion), true)]
        public SerializedType Type;

        /// <summary>
        /// The Criterion.
        /// </summary>
        [SerializeField, FormerlySerializedAs("criterion")]
        public Criterion Criterion;

        /// <summary>
        /// Constructs with type and criterion.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="criterion"></param>
        public TypedCriterion(SerializedType type, Criterion criterion)
        {
            Type = type;
            Criterion = criterion;
        }
    }

    [Serializable]
    class TypedCriterionCollection : CollectionWrapper<TypedCriterion>
    {
        public TypedCriterionCollection() : base()
        {
        }

        public TypedCriterionCollection(IList<TypedCriterion> items) : base(items)
        {
        }
    }
}
