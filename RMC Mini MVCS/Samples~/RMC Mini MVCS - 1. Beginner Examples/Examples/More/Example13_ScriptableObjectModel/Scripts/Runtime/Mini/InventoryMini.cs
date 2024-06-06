using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Features;
using RMC.Core.Architectures.Mini.Locators;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.SOM.Mini
{
    /// <summary>
    /// See <see cref="MiniMvcs{TContext,TModel,TView,TController,TService}"/>
    ///
    /// For this demo, this mini is set up with nothing much inside.
    ///
    /// From outside this class the MVCS are added.
    ///
    /// 
    /// </summary>
    public class InventoryMini: MiniMvcs
            <Context, 
                IModel, 
                IView, 
                IController,
                IService>
    {
        
        //  Fields ----------------------------------------

        
        //  Properties ------------------------------------
        /// <summary>
        /// This is an optional add-on that gives this <see cref="InventoryMini"/>
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
                InventoryService service = new InventoryService();
                    
                // ModelLocator is created in superclass
                _viewLocator = new Locator<IView>();
                _controllerLocator = new Locator<IController>();
                _serviceLocator = new Locator<IService>();
                
                // Model item is already added in superclass
                ServiceLocator.AddItem(service);
                
                //
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