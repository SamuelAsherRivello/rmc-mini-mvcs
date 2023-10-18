using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class ClockController: BaseController // Extending 'base' is optional
        <ClockModel, ClockView, ClockService> 
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------

        public ClockController(ClockModel model, ClockView view, ClockService service) 
            : base(model, view, service)
        {
            
        }

        //  Initialization  -------------------------------
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

            ClockHelper.StartTicking(async() => 
            {
                // Round the number for readability
                float time = Mathf.Round(Time.time);
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