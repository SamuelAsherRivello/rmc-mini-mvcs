
using RMC.Mini.Controller.Commands;

namespace RMC.Mini.Samples.SpawnerMini.WithMini.Mini.Controller.Commands
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