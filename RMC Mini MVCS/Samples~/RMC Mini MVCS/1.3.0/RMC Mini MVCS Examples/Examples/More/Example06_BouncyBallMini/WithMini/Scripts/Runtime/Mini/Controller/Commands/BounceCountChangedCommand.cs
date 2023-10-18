
using RMC.Core.Architectures.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.Controller.Commands
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This Command is a stand-alone object containing
    /// the before and after value during a data change
    /// </summary>
    public class BounceCountChangedCommand: ValueChangedCommand<int>
    {
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        
        //  Initialization  -------------------------------
        public BounceCountChangedCommand(int previousValue, int currentValue) : 
            base(previousValue, currentValue)
        {
        }
    }
}