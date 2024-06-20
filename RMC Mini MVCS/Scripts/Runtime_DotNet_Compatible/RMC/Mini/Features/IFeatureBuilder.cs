namespace RMC.Mini.Features
{
    public interface IFeatureBuilder<F> : IInitializableWithMini where F : IFeature
    {
        Locator<IFeature> FeatureLocator { get; }
    }
}