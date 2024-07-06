using RMC.Mini.Features;
using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;
using RMC.Mini.Samples.UGS.Mini.Model;
using RMC.Mini.Samples.UGS.Mini.Service;

namespace RMC.Mini.Samples.UGS.Mini
{
    /// <summary>
    /// See <see cref="MiniMvcs{TContext,TModel,TView,TController,TService}"/>
    /// </summary>
    public class UgsMini: MiniMvcs
            <Context, 
                IModel, 
                IView, 
                IController,
                IService>
    {
        
        //  Fields ----------------------------------------

        
        //  Properties ------------------------------------
        /// <summary>
        /// This is an optional addon that gives this <see cref="UgsMini"/>
        /// the support of a <see cref="IFeatureBuilder{F}"/> to easily 
        /// add and remove <see cref="IFeature"/>. 
        /// </summary>
        private FeatureBuilder FeatureBuilder { get; set; }
        
        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                // Context
                _context = new Context();
                
                // Model
                UgsModel model = new UgsModel();
                
                // Service
                AuthenticationService authenticationService = new AuthenticationService();
                ServiceLocator.AddItem(authenticationService);
                
                AnalyticsService analyticsService = new AnalyticsService();
                ServiceLocator.AddItem(analyticsService);

                //Feature
                FeatureBuilder = new FeatureBuilder();
                
                // Initialize
                model.Initialize(_context); //Added to locator inside
                authenticationService.Initialize(_context);
                analyticsService.Initialize(_context);
                FeatureBuilder.Initialize(this);
                
            }
        }

        
        //  Methods  -------------------------------
        public bool HasFeature<TFeature>(string key = "") where TFeature : IFeature
        {
            return FeatureBuilder.HasFeature<TFeature>(key);
        }
        
        public void AddFeature<TFeature>(TFeature feature, string key = "") where TFeature : IFeature
        {
            FeatureBuilder.AddFeature<TFeature>(feature, key);
        }
        
        public void RemoveFeature<TFeature>(string key = "", bool willDispose = false) where TFeature : IFeature
        {
            FeatureBuilder.RemoveFeature<TFeature>(key, willDispose);
        }
        
        // Overload for automatically disposing
        public void RemoveAndDisposeFeature<TFeature>() where TFeature : IFeature
        {
            FeatureBuilder.RemoveFeature<TFeature>("", true);
        }
    }
}