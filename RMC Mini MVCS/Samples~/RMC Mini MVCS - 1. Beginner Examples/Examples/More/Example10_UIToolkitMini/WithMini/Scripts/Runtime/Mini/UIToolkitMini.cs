using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The MiniMvcs is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    /// </summary>
    public class UIToolkitMini: MiniMvcs
            <Context.Context, 
            UIToolkitModel, 
            UIToolkitView, 
            UIToolkitController,
            UIToolkitService>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------


        //  Initialization  -------------------------------
        public UIToolkitMini(UIToolkitView view)
        {
            _view = view;
        
        }
        
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                //
                _context = new Context.Context();
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