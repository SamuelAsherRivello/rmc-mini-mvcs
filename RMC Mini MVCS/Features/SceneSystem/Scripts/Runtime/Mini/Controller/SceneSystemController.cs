using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Service;
using UnityEngine.SceneManagement;

namespace RMC.Core.Architectures.Mini.Features.SceneSystem
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class SceneSystemController: BaseController // Extending 'base' is optional
        <SceneSystemModel, SceneSystemView, DummyService> 
    {
        public SceneSystemController(
            SceneSystemModel model, SceneSystemView view, DummyService service) 
            : base(model, view, service)
        {
        }

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                Context.CommandManager.AddCommandListener<LoadSceneRequestCommand>(OnLoadSceneRequestCommand);
            }
        }

        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        
        /// <summary>
        /// Before the change is made
        /// </summary>
        /// <param name="loadSceneRequestCommand"></param>
        private async void OnLoadSceneRequestCommand(
            LoadSceneRequestCommand loadSceneRequestCommand)
        {
            RequireIsInitialized();
            
            // Do not change scene if already in it
            if (loadSceneRequestCommand.SceneName ==
                SceneManager.GetActiveScene().name)
            {
                return;
            }

            await _view.FadeScreenOnAsync();
            
            // Make the change
            var asyncOperation =  SceneManager.LoadSceneAsync(loadSceneRequestCommand.SceneName);

            if (asyncOperation != null)
            {
                asyncOperation.completed += async operation =>
                {
                    await _view.FadeScreenOffAsync();
                    
                    // Notify the change is complete!
                    // Note: This is not strictly useful, but it's a demo of how
                    // to have such a 'completed' command
                    Context.CommandManager.InvokeCommand(
                        new LoadSceneCompletedCommand(SceneManager.GetActiveScene().name));
                };
            }
        }
    }
}