using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;
using RMC.Core.Architectures.Mini.Structure.Simple;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// See <see cref="SimpleMiniMvcs{TContext,TModel,TView,TController,TService}"/>"/>
    /// </summary>
    public class UIToolkitSimpleMini: SimpleMiniMvcs
            <Context, 
            UIToolkitModel, 
            UIToolkitView, 
            UIToolkitController,
            UIToolkitService>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------


        //  Initialization  -------------------------------
        public UIToolkitSimpleMini(UIToolkitView view)
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
                _model = new UIToolkitModel();
                _service = new UIToolkitService();
                _controller = new UIToolkitController(_model, _view, _service);
                
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