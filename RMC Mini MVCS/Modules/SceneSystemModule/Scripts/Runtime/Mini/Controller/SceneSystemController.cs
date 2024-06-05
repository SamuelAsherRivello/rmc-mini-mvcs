using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using UnityEngine.SceneManagement;

namespace RMC.Core.Architectures.Mini.Modules.SceneSystemModule
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
                Context.CommandManager.AddCommandListener<SceneSystemLoadSceneCommand>(OnSceneSystemLoadSceneCommand);
            }
        }

        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        private async void OnSceneSystemLoadSceneCommand(
            SceneSystemLoadSceneCommand sceneSystemLoadSceneCommand)
        {
            RequireIsInitialized();
            
            // Do not change scene if already in it
            if (sceneSystemLoadSceneCommand.SceneName ==
                SceneManager.GetActiveScene().name)
            {
                return;
            }

            await _view.FadeScreenOnAsync();
            
            var asyncOperation =  SceneManager.LoadSceneAsync(sceneSystemLoadSceneCommand.SceneName);

            if (asyncOperation != null)
            {
                asyncOperation.completed += async operation =>
                {
                    await _view.FadeScreenOffAsync();
                };
            }
        }
    }
}