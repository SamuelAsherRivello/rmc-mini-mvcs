using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.View;
using RMC.Core.Architectures.Mini.Structure.Simple;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// See <see cref="SimpleMiniMvcs{TContext,TModel,TView,TController,TService}"/>"/>
    /// </summary>
    public class ClockSimpleMini: SimpleMiniMvcs
        <Context, 
        ClockModel, 
        ClockView, 
        ClockController,
        ClockService>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------


        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                //
                _context = new Context();
                _model = new ClockModel();
                _view = new ClockView();
                _service = new ClockService();
                _controller = new ClockController(_model, _view, _service);
                
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