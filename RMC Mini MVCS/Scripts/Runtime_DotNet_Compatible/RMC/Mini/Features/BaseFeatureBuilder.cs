using System;
using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

namespace RMC.Mini.Features
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public abstract class BaseFeatureBuilder<TFeature>: IFeatureBuilder<TFeature> where TFeature : IFeature
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IMiniMvcs<Context, IModel, IView, IController, IService> MiniMvcs { get; private set; }
        public Locator<IFeature> FeatureLocator { get; private set; }
        
        //  Fields ----------------------------------------

        
        //  Initialization  -------------------------------
        public virtual void Initialize(IMiniMvcs<Context, IModel, IView, IController, IService> miniMvcs)
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
        
        public void RemoveFeature<TFeatureMethod>(string key = "", bool willDispose = false) where TFeatureMethod : IFeature
        {
            if (FeatureLocator.HasItem<TFeatureMethod>(key))
            {
                TFeatureMethod feature = FeatureLocator.GetItem<TFeatureMethod>(key);
                feature?.Dispose();
                FeatureLocator.RemoveItem<TFeatureMethod>(key, willDispose);
            }
        }
        
        // Overload for automatically disposing
        public void RemoveAndDisposeFeature<TFeatureMethod>(string key = "") where TFeatureMethod : IFeature
        {
            RemoveFeature<TFeatureMethod>(key, true);
        }
        
        //  Event Handlers --------------------------------
     
    }
}