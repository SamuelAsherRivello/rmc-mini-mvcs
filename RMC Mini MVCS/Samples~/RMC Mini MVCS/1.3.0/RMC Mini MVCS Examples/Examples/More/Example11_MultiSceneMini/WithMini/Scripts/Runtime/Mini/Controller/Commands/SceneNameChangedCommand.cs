using RMC.Core.Architectures.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Controller.Commands
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Command is a stand-alone object containing
    /// all arguments needed to perform a request
    /// </summary>
    public class SceneNameChangedCommand : ValueChangedCommand<string>
    {
        public SceneNameChangedCommand(string previousValue, string currentValue) : 
            base(previousValue, currentValue)
        {
        }
    }
}