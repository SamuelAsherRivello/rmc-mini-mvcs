
using RMC.Core.Architectures.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Controller.Commands
{
    /// <summary>
    /// This Command is a stand-alone object containing
    /// the before and after value during a data change
    /// </summary>
    public class TimeChangedCommand: ValueChangedCommand<float>
    {
        //  Initialization  -------------------------------
        public TimeChangedCommand(float previousValue, float currentValue) : 
            base(previousValue, currentValue)
        {
        }
    }
}