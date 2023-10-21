using UnityEngine;
using System;

namespace SerializableCallback
{
    /// <summary> Add to fields of your class extending SerializableCallbackBase&lt;T,...&gt; to limit which types can be assigned to it. </summary>
    public class TargetConstraintAttribute : PropertyAttribute
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Type targetType;

        /// <summary> Add to fields of your class extending SerializableCallbackBase&lt;T,...&gt; to limit which types can be assigned to it. </summary>
        /// <param name="targetType"></param>
        public TargetConstraintAttribute(Type targetType)
        {
            this.targetType = targetType;
        }
    }
}
