using System;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Locators;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.Features
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public abstract class BaseFeatureBuilder<TFeature>: IFeatureBuilder<TFeature> where TFeature : IFeature
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IMiniMvcs<Mini.Context, IModel, IView, IController, IService> MiniMvcs { get; private set; }
        public Locator<IFeature> FeatureLocator { get; private set; }
        
        //  Fields ----------------------------------------

        
        //  Initialization  -------------------------------
        public virtual void Initialize(IMiniMvcs<Mini.Context, IModel, IView, IController, IService> miniMvcs)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                MiniMvcs = miniMvcs;
                FeatureLocator = new Locator<IFeature>();
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }

        //  Methods ---------------------------------------
        public bool HasFeature<TFeatureMethod>(string key = "") where TFeatureMethod : IFeature
        {
            return FeatureLocator.HasItem<TFeatureMethod>(key);
        }
        
        public void AddFeature<TFeatureMethod>(TFeatureMethod feature, string key = "") where TFeatureMethod : IFeature
        {
            FeatureLocator.AddItem(feature, key);
            feature.Initialize(MiniMvcs);
            feature.Build();
        }
        
        public void RemoveFeature<TFeatureMethod>(string key = "") where TFeatureMethod : IFeature
        {
            if (FeatureLocator.HasItem<TFeatureMethod>(key))
            {
                TFeatureMethod feature = FeatureLocator.GetItem<TFeatureMethod>(key);
                feature?.Dispose();
                FeatureLocator.RemoveItem<TFeatureMethod>(key);
            }
        }
        
        //  Event Handlers --------------------------------
     
    }
}