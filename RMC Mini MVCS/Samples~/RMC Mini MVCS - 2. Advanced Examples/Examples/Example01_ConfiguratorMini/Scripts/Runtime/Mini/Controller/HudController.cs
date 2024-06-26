using RMC.Mini.Controller;
using RMC.Mini.Features.SceneSystem;
using RMC.Mini.Samples.Configurator.Mini.Model;
using RMC.Mini.Samples.Configurator.Mini.Model.Data;
using RMC.Mini.Samples.Configurator.Mini.Service;
using RMC.Mini.Samples.Configurator.Mini.View;

namespace RMC.Mini.Samples.Configurator.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class HudController: BaseController // Extending 'base' is optional
        <ConfiguratorModel, HudView, ConfiguratorService> 
    {
        public HudController(
            ConfiguratorModel model, HudView view, ConfiguratorService service) 
            : base(model, view, service)
        {
            
        }

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                _view.OnBack.AddListener(View_OnBack);
                _view.OnDeveloperConsole.AddListener(View_OnDeveloperConsole);
            }
        }

        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------

        
        private void View_OnBack()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(ConfiguratorConstants.Scene01_Menu));
        }
        
        private void View_OnDeveloperConsole()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(ConfiguratorConstants.Scene05_DeveloperConsole));
        }
    }
}