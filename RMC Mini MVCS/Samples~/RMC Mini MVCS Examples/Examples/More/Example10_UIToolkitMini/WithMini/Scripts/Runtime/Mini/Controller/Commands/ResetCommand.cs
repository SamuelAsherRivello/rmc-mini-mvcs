using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller.Commands
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Command is a stand-alone object containing
    /// all arguments needed to perform a request
    /// </summary>
    public class ResetCommand : ICommand
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        
        //  Initialization  -------------------------------
        public ResetCommand(UIToolkitModel model)
        {
            ExecuteAsync(model);
        }

        private async void ExecuteAsync(UIToolkitModel model)
        {
            //Body first
            model.BodyIndex.Value = model.InitialCharacterData.BodyIndex;
            
            //Then the rest
            model.HairIndex.Value = model.InitialCharacterData.HairIndex;
            await Task.Delay(200);
            model.FaceIndex.Value = model.InitialCharacterData.FaceIndex;
            await Task.Delay(200);
            model.ShirtIndex.Value = model.InitialCharacterData.ShirtIndex;
            await Task.Delay(200);
 
        }
    }
}