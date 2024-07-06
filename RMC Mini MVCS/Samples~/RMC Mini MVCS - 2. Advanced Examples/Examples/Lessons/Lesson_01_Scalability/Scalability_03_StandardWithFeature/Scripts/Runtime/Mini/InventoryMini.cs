using RMC.Mini.Features;
using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

namespace RMC.Mini.Lessons.Scalability.StandardWithFeature.Mini
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
                
                // Context
                _context = new Context();
                
                // Feature
                FeatureBuilder = new FeatureBuilder();
                FeatureBuilder.Initialize(this);

                // Model
                InventoryModel model = new InventoryModel();
                
                // Service
                InventoryService service = new InventoryService();
                ServiceLocator.AddItem(service);
                
                // Initialize
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
        
        
        public void RemoveFeature<TFeature>(string key = "", bool willDispose = false) where TFeature : IFeature
        {
            FeatureBuilder.RemoveFeature<TFeature>(key, willDispose);
        }
        
        public void RemoveAndDisposeFeature<TFeature>() where TFeature : IFeature
        {
            FeatureBuilder.RemoveFeature<TFeature>("", true);
        }
        
        //  Event Handlers --------------------------------
    }
}