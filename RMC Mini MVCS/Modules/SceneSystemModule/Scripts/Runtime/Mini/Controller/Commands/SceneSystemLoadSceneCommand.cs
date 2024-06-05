using RMC.Core.Architectures.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Modules.SceneSystemModule
{
    /// <summary>
    /// The Command is a stand-alone object containing
    /// all arguments needed to perform a request
    /// </summary>
    public class SceneSystemLoadSceneCommand : ICommand
    {
        //  Properties ------------------------------------
        public string SceneName { get; private set; }

        
        //  Fields ----------------------------------------

        
        //  Initialization  -------------------------------
        public SceneSystemLoadSceneCommand(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}