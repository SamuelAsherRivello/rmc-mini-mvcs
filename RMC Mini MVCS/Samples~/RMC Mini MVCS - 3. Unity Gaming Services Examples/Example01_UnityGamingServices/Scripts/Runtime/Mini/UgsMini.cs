using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Features;
using RMC.Core.Architectures.Mini.Locators;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini
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
                
                _context = new Context();
                
                // Locators
                // ...ModelLocator is created in superclass
                _viewLocator = new Locator<IView>();
                _controllerLocator = new Locator<IController>();
                _serviceLocator = new Locator<IService>();
       
                // Model
                UgsModel model = new UgsModel();
                model.Initialize(_context); //Added to locator inside
                
                // Service
                AuthenticationService authenticationService = new AuthenticationService();
                ServiceLocator.AddItem(authenticationService);
                authenticationService.Initialize(_context);
                
                AnalyticsService analyticsService = new AnalyticsService();
                ServiceLocator.AddItem(analyticsService);
                analyticsService.Initialize(_context);
                
                //Feature
                FeatureBuilder = new FeatureBuilder();
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
        
        public void RemoveFeature<TFeature>(string key = "") where TFeature : IFeature
        {
            FeatureBuilder.RemoveFeature<TFeature>(key);
        }
        
    }
}