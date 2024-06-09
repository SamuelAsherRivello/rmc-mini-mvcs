using RMC.Core.Architectures.Mini.Controller;

namespace RMC.Core.Architectures.Mini.Samples.SOM.Mini
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class InventoryController: BaseController // Extending 'base' is optional
        <InventoryScriptableObjectModel, InventoryView, InventoryService> 
    {
        public InventoryController(
            InventoryScriptableObjectModel model, InventoryView view, InventoryService service) 
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
        private void Service_OnLoadCompleted(string message)
        {
            RequireIsInitialized();
        }
        
        private void View_OnAction()
        {
            _model.InventoryCount.Value++;
        }
    }
}