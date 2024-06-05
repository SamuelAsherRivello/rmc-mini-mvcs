using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The MiniMvcs is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    /// </summary>
    public class CountUpMini: MiniMvcs
            <Context.Context, 
            CountUpModel, 
            CountUpView, 
            CountUpController,
            CountUpService>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------


        //  Initialization  -------------------------------
        public CountUpMini(CountUpView view)
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
                _model = new CountUpModel();
                _service = new CountUpService();
                _controller = new CountUpController(_model, _view, _service);

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