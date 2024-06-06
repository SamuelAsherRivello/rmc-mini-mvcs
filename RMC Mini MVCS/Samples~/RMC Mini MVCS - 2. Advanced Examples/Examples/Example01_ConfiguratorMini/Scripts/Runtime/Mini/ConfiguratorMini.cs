using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Features;
using RMC.Core.Architectures.Mini.Locators;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Service;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini
{
    /// <summary>
    /// See <see cref="MiniMvcs{TContext,TModel,TView,TController,TService}"/>
    /// </summary>
    public class ConfiguratorMini: MiniMvcs
            <Context, 
                IModel, 
                IView, 
                IController,
                IService>
    {
        
        //  Fields ----------------------------------------

        
        //  Properties ------------------------------------
        /// <summary>
        /// This is an optional addon that gives this <see cref="ConfiguratorMini"/>
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
                ConfiguratorModel model = new ConfiguratorModel();
                model.Initialize(_context); //Added to locator inside
                
                // Service
                ConfiguratorService service = new ConfiguratorService();
                ServiceLocator.AddItem(service);
                service.Initialize(_context);
                
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