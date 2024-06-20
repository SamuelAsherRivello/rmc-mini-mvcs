
using RMC.Mini.Controller.Commands;

namespace RMC.Mini.Samples.RollABall.WithMini.Mini.Controller.Commands
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This Command is a stand-alone object containing
    /// a value of data.
    /// </summary>
    public class StatusChangedCommand: ValueCommand<string>
    {
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        
        //  Initialization  -------------------------------
        public StatusChangedCommand(string value) : 
            base(value)
        {
        }
    }
}