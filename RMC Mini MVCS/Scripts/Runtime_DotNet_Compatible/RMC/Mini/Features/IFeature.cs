using System;

namespace RMC.Mini.Features
{
    /// <summary>
    /// A <see cref="IFeature"/> is a collection of one or more <see cref="IConcern"/>
    /// You can turn on and off something in the game (like an inventory system)
    /// by adding or removing your custom inventory-related <see cref="IFeature"/>
    /// </summary>
    public interface IFeature : IInitializableWithMini, IDisposable
    {
        void Build();
    }
}