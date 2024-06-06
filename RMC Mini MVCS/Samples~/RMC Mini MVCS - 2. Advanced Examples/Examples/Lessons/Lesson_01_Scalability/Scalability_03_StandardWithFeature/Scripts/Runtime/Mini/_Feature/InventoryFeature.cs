using RMC.Core.Architectures.Mini.Features;

namespace RMC.Mini.Lessons.Scalability.StandardWithFeature.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class InventoryFeature: BaseFeature // Extending 'base' is optional
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        
        //  Initialization  -------------------------------

        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            // Get from mini
            InventoryModel model = MiniMvcs.ModelLocator.GetItem<InventoryModel>();
            InventoryService service = MiniMvcs.ServiceLocator.GetItem<InventoryService>();
            
            // Create new controller
            InventoryController controller = 
                new InventoryController(model, View as InventoryView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
            
        }

        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<InventoryController>())
            {
                MiniMvcs.ControllerLocator.RemoveItem<InventoryController>();
                MiniMvcs.ViewLocator.RemoveItem<InventoryView>();
            }
        }

        //  Event Handlers --------------------------------

    }
}