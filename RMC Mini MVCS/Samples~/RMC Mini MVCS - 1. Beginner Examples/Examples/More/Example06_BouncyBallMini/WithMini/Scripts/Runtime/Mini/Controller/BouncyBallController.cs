using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class BouncyBallController: BaseController // Extending 'base' is optional
            <BouncyBallModel, 
            BouncyBallView, 
            BouncyBallService> 
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------

        public BouncyBallController(BouncyBallModel model, BouncyBallView view, BouncyBallService service) 
            : base(model, view, service)
        {
            
        }

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
 
                // Demo - Controller may update view DIRECTLY...
                _view.TitleText.text = "Bouncy Ball, Mini Example";
                _view.StatusText.text = "Loading...";
                
                //
                _service.OnLoaded.AddListener(Service_OnLoaded);
                _service.Load();
            }
        }
        
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
        private void Service_OnLoaded()
        {
            RequireIsInitialized();
            
            _model.BounceCountMax.Value = _service.BounceCountMax;
            _view.OnBounced.AddListener(View_OnBounced);
            _model.BounceCount.OnValueChanged.AddListener(Model_OnCounterChanged);
            
            // Demo - Controller may update view DIRECTLY...
            _view.StatusText.text = "Waiting...";
        }
        
        private void View_OnBounced()
        {
            RequireIsInitialized();
            
            // Demo - Controller may update view INDIRECTLY...
            Context.CommandManager.InvokeCommand(
                new PlayAudioClipCommand("AudioClips/Bounce01"));
            
            // Demo - Controller may update model DIRECTLY...
            if (_model.BounceCount.Value + 1 > _model.BounceCountMax.Value)
            {
                //Wrap the value
                _model.BounceCount.Value = 1;
            }
            else
            {
                //Increment the value
                _model.BounceCount.Value++;
            }
        }
        
        private void Model_OnCounterChanged(int previousValue, int currentValue)
        {
            RequireIsInitialized();
            
            // Demo - Controller may update view INDIRECTLY...
            Context.CommandManager.InvokeCommand(
                new BounceCountChangedCommand(previousValue, currentValue));
        }
    }
}