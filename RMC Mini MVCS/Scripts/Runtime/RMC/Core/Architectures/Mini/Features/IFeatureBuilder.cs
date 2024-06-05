using RMC.Core.Architectures.Mini.Locators;

namespace RMC.Core.Architectures.Mini.Features
{
    public interface IFeatureBuilder<F> : IInitializableWithMini where F : IFeature
    {
        Locator<IFeature> FeatureLocator { get; }
    }
}