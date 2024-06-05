using System;
using RMC.Core.Architectures.Mini.Context;

namespace RMC.Core.Experimental.Architectures.Mini.Complex
{
    public interface IFeatureBuilder<F> : IInitializableWithMiniComplex where F : IFeature
    {
        Locator<IFeature> FeatureLocator { get; }
    }
}