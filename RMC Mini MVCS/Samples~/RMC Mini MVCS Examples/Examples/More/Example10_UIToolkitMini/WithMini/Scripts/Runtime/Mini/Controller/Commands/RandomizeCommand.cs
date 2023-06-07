using RMC.Core.Architectures.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller.Commands
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Command is a stand-alone object containing
    /// all arguments needed to perform a request
    /// </summary>
    public class RandomizeCommand : ICommand
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        
        //  Initialization  -------------------------------
        public RandomizeCommand(UIToolkitModel model, UIToolkitView view)
        {
            model.HairIndex.Value = Random.Range(0, view.HairSprites.Count);
            model.FaceIndex.Value = Random.Range(0, view.FaceSprites.Count);
            model.ShirtIndex.Value = Random.Range(0, view.ShirtSprites.Count);
            model.BodyIndex.Value = Random.Range(0, view.BodySprites.Count);
        }
    }
}