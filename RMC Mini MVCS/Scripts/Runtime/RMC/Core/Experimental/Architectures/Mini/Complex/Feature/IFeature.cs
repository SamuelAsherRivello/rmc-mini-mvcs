using System;

namespace RMC.Core.Experimental.Architectures.Mini.Complex
{
    public interface IFeature : IInitializableWithMiniComplex, IDisposable
    {
        void Build();
    }
}