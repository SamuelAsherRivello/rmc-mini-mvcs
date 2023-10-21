
using RMC.Core.Architectures.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.Controller.Commands
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This Command is a stand-alone object containing
    /// the before and after value during a data change
    /// </summary>
    public class CounterChangedCommand : ValueChangedCommand<int>
    {

        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        
        //  Initialization  -------------------------------
        public CounterChangedCommand(int previousValue, int currentValue) : 
            base(previousValue, currentValue)
        {
        }
    }
}