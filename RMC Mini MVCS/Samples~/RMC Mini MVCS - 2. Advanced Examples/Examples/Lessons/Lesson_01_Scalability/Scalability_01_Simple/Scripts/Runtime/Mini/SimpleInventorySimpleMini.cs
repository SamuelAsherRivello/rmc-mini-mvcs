using RMC.Core.Architectures.Mini;
using RMC.Core.Architectures.Mini.Structure.Simple;

namespace RMC.Mini.Lessons.Scalability.Simple.Mini
{
    /// <summary>
    /// See <see cref="SimpleMiniMvcs{TContext,TModel,TView,TController,TService}"/>"/>
    /// </summary>
    public class SimpleInventorySimpleMini: SimpleMiniMvcs
            <Context, 
                InventoryModel, 
                InventoryView, 
                InventoryController,
                InventoryService>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------


        //  Initialization  -------------------------------
        public SimpleInventorySimpleMini(InventoryView view)
        {
            _view = view;
        
        }
        
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                //
                _context = new Context();
                _model = new InventoryModel();
                _service = new InventoryService();
                _controller = new InventoryController(_model, _view, _service);
                
                //
                _model.Initialize(_context);
                _view.Initialize(_context);
                _service.Initialize(_context);
                _controller.Initialize(_context);
            }
        }

        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}