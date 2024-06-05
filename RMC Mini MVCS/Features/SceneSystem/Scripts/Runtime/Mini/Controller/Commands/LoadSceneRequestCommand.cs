using RMC.Core.Architectures.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Features.SceneSystem
{
    /// <summary>
    /// The Command is a stand-alone object containing
    /// all arguments needed to perform a request
    /// </summary>
    public class LoadSceneRequestCommand : ICommand
    {
        //  Properties ------------------------------------
        public string SceneName { get; private set; }

        
        //  Fields ----------------------------------------

        
        //  Initialization  -------------------------------
        public LoadSceneRequestCommand(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}