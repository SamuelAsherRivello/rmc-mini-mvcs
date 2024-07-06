

//Keep As:RMC.Mini

using System;

namespace RMC.Mini
{
    /// <summary>
    /// Enforces API for types which are one
    /// area of coding concern within the
    /// <see cref="ISimpleMiniMvcs"/> architectural framework.
    /// </summary>
    public interface IConcern : IInitializableWithContext, IDisposable
    {
        //  Properties ------------------------------------
    

        //  Methods ---------------------------------------
    }
}