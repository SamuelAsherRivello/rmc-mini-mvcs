using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the Concerns and contains the core app logic 
    /// </summary>
    public class CountUpController: BaseController // Extending 'base' is optional
        <CountUpModel, CountUpView, CountUpService> 
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------

        public CountUpController(CountUpModel model, CountUpView view, CountUpService service) 
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
                _view.TitleText.text = "Count Up, Mini Example";
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

            _model.Counter.Value = _service.CounterInitialValue;
            _view.OnScreenClicked.AddListener(View_OnScreenClicked);
            _model.Counter.OnValueChanged.AddListener(Model_OnCounterChanged);
            
            // Demo - Controller may update view DIRECTLY...
            _view.StatusText.text = "Click anywhere to continue.";
        }
        
        private void View_OnScreenClicked()
        {
            RequireIsInitialized();

            _model.Counter.Value ++;
        }
        
        private void Model_OnCounterChanged(int previousValue, int currentValue)
        {
            RequireIsInitialized();

            // Demo - Controller may update view INDIRECTLY...
            Context.CommandManager.InvokeCommand(
                new CounterChangedCommand(previousValue, currentValue));

        }
    }
}