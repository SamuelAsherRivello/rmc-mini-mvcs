using RMC.Mini.Controller.Commands;

namespace RMC.Mini.Features.SceneSystem
{
    /// <summary>
    /// The Command is a stand-alone object containing
    /// all arguments needed to perform a request
    /// </summary>
    public class LoadSceneCompletedCommand : ICommand
    {
        //  Properties ------------------------------------
        public string SceneName { get; private set; }

        
        //  Fields ----------------------------------------

        
        //  Initialization  -------------------------------
        public LoadSceneCompletedCommand(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}