using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;
using RMC.Core.Experimental.Architectures.Mini.Complex;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model;
using RMC.MiniMvcs.Samples.Configurator.Mini.Service;

namespace RMC.MiniMvcs.Samples.Configurator.Mini
{
    /// <summary>
    /// The ComplexMini is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    /// </summary>
    public class ConfiguratorMini: MiniMvcsComplex
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
                FeatureBuilder = new FeatureBuilder();
                FeatureBuilder.Initialize(this);

                //
                ConfiguratorModel model = new ConfiguratorModel();
                ConfiguratorService service = new ConfiguratorService();
                    
                // ModelLocator is created in superclass
                _viewLocator = new Locator<IView>();
                _controllerLocator = new Locator<IController>();
                _serviceLocator = new Locator<IService>();
                
                // Model item is already added in superclass
                ServiceLocator.AddItem(service);
                
                //
                model.Initialize(_context);
                service.Initialize(_context);
                
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
        
        
        //  Event Handlers --------------------------------
    }
}