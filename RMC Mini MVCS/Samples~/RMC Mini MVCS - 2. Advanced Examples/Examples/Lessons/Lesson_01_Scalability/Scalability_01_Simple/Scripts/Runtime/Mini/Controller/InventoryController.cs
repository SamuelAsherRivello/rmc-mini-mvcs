using RMC.Mini.Controller;

namespace RMC.Mini.Lessons.Scalability.Simple.Mini
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class InventoryController: BaseController // Extending 'base' is optional
        <InventoryModel, InventoryView, InventoryService> 
    {
        public InventoryController(
            InventoryModel model, InventoryView view, InventoryService service) 
            : base(model, view, service)
        {
        }

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                //
                _view.OnAction.AddListener(View_OnAction);
                
                // Load the data as needed
                _service.OnLoadCompleted.AddListener(Service_OnLoadCompleted);
                _service.Load();
            }
        }




        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
        private void Service_OnLoadCompleted(int inventoryCount)
        {
            RequireIsInitialized();
            _model.InventoryCount.Value = inventoryCount; 
        }
        
        private void View_OnAction()
        {
            _model.InventoryCount.Value++;
        }
    }
}