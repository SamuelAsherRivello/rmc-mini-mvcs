using RMC.Mini.Features;
using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

namespace RMC.Mini.Samples.SOM.Mini
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

                // Service
                InventoryService service = new InventoryService();
                ServiceLocator.AddItem(service);
                
                // Initialize
                service.Initialize(_context);
                
            }
        }
        
        //  Methods  -------------------------------
        
        
        //  Event Handlers --------------------------------
    }
}