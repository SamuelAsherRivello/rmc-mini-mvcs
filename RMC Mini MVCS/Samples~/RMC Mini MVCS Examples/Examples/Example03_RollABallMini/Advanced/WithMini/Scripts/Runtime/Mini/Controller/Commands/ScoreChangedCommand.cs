
using RMC.Core.Architectures.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller.Commands
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This Command is a stand-alone object containing
    /// a value of data.
    /// </summary>
    public class ScoreChangedCommand: ValueCommand<int>
    {
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        
        //  Initialization  -------------------------------
        public ScoreChangedCommand(int value) : 
            base(value)
        {
        }
    }
}