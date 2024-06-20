using System.Diagnostics;
using System.Threading.Tasks;
using RMC.Mini.Controller;
using RMC.Mini.Samples.Clock.WithMini.Mini.Controller.Commands;
using RMC.Mini.Samples.Clock.WithMini.Mini.Model;
using RMC.Mini.Samples.Clock.WithMini.Mini.Service;
using RMC.Mini.Samples.Clock.WithMini.Mini.View;

namespace RMC.Mini.Samples.Clock.WithMini.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class ClockController: BaseController // Extending 'base' is optional
        <ClockModel, ClockView, ClockService> 
    {

        //  Fields ----------------------------------------
        // Initialize and start the stopwatch
        private Stopwatch _stopwatch = new Stopwatch();


        //  Initialization  -------------------------------
        public ClockController(ClockModel model, ClockView view, ClockService service)
            : base(model, view, service)
        {
            
        }
        
        
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                _model.Time.OnValueChanged.AddListener(Model_OnTimerChanged);
                _service.OnLoaded.AddListener(Service_OnLoaded);
                _service.Load();
            }
        }
        
        
        //  Methods ---------------------------------------
        private void StartTicking()
        {
            RequireIsInitialized();

            _stopwatch.Start();
            
            ClockHelper.StartTicking(async() => 
            {
                // Round the number for readability
                float time = (float)System.Math.Round(_stopwatch.Elapsed.TotalSeconds, 0);
                if (time > 0)
                {
                    // Update the time
                    _model.Time.Value = time;
                }
                
                // Wait a short amount
                await Task.Delay(_model.TimeDelay.Value);
            });
        }

        
        //  Event Handlers --------------------------------
        private void Service_OnLoaded()
        {
            RequireIsInitialized();
            _model.TimeDelay.Value = _service.TimeDelay;
            
            StartTicking();
        }
        
        
        private void Model_OnTimerChanged(float previousValue, float currentValue)
        {
            RequireIsInitialized();
            
            // Demo - Controller may update view INDIRECTLY...
            Context.CommandManager.InvokeCommand(
                new TimeChangedCommand(previousValue, currentValue));

        }
    }
}