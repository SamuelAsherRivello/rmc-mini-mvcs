using System;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Base class for different SerializedTypeFilter attribute implementations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class SerializedTypeFilterAttributeBase : Attribute
    {
        /// <summary>
        /// Base type.
        /// </summary>
        public Type BaseType { get; protected set; }

        /// <summary>
        /// Should abstract types be hidden by the filter?
        /// </summary>
        public bool HideAbstractTypes { get; protected set; }
    }

    /// <summary>
    /// Use to create type filter for any type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SerializedTypeFilterAttribute : SerializedTypeFilterAttributeBase
    {
        /// <summary>
        /// Constructs with a type.
        /// </summary>
        /// <param name="baseType">Base type.</param>
        /// <param name="hideAbstractTypes">Should abstract types be hidden by the filter?</param>
        public SerializedTypeFilterAttribute(Type baseType, bool hideAbstractTypes)
        {
            BaseType = baseType;
            HideAbstractTypes = hideAbstractTypes;
        }
    }

    /// <summary>
    /// Specialization for typeof(GUIView).
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SerializedTypeGuiViewFilterAttribute : SerializedTypeFilterAttributeBase
    {
        /// <summary>
        /// Default-construcs with typeof(GUIView).
        /// </summary>
        public SerializedTypeGuiViewFilterAttribute()
        {
            BaseType = GUIViewProxy.GuiViewType;
            HideAbstractTypes = false;
        }
    }
}
