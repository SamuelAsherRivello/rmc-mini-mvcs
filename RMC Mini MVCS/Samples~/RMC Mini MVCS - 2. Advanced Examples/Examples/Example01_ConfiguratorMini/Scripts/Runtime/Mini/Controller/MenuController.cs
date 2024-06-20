using RMC.Mini.Controller;
using RMC.Mini.Features.SceneSystem;
using RMC.Mini.Samples.Configurator.Mini.Model;
using RMC.Mini.Samples.Configurator.Mini.Model.Data;
using RMC.Mini.Samples.Configurator.Mini.Service;
using RMC.Mini.Samples.Configurator.Mini.Service.Storage;
using RMC.Mini.Samples.Configurator.Mini.View;

namespace RMC.Mini.Samples.Configurator.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class MenuController: BaseController // Extending 'base' is optional
        <ConfiguratorModel, MenuView, ConfiguratorService> 
    {
        public MenuController(
            ConfiguratorModel model, MenuView view, ConfiguratorService service) 
            : base(model, view, service)
        {
        }

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                //
                _view.OnPlay.AddListener(View_OnPlay);
                _view.OnCustomizeCharacter.AddListener(View_OnCustomizeCharacter);
                _view.OnCustomizeEnvironment.AddListener(View_OnCustomizeEnvironment);
                Context.CommandManager.AddCommandListener<LoadSceneRequestCommand>(OnLoadSceneRequestCommand);
      
                
                // Load the data as needed
                _service.OnLoadCompleted.AddListener(Service_OnLoadCompleted);
                if (!_model.HasLoadedService.Value)
                {
                    _service.Load();
                }
                else
                {
                    Service_OnLoadCompleted(null);
                }
            }
        }


        //  Methods ---------------------------------------
        public override void Dispose()
        {
            base.Dispose();
            Context.CommandManager.RemoveCommandListener<LoadSceneRequestCommand>(OnLoadSceneRequestCommand);
        }


        //  Event Handlers --------------------------------
        private void OnLoadSceneRequestCommand(LoadSceneRequestCommand loadSceneRequestCommand)
        {
            //Note: its optional to observe this command and toggle off the UI
            //THis is just a demo of how to do it
            _view.PlayGameButton.SetEnabled(false);
            _view.CustomizeCharacterButton.SetEnabled(false);
            _view.CustomizeEnvironmentButton.SetEnabled(false);
        }
        
        private void View_OnCustomizeCharacter()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(ConfiguratorConstants.Scene02_CustomizeCharacter));
        }
        

        private void View_OnCustomizeEnvironment()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(ConfiguratorConstants.Scene03_CustomizeEnvironment));
        }
        
        private void View_OnPlay()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(ConfiguratorConstants.Scene04_Game));
        }

        
        private void Service_OnLoadCompleted(ConfiguratorServiceData configuratorServiceData)
        {
            RequireIsInitialized();
            _model.HasLoadedService.Value = true;
            
            if (configuratorServiceData != null)
            {
                // Set FROM the saved data. Don't save again here.
                _model.CharacterData.Value = configuratorServiceData.CharacterData;
                _model.EnvironmentData.Value = configuratorServiceData.EnvironmentData;
            }
            else
            {
                _model.CharacterData.OnValueChangedRefresh();
                _model.EnvironmentData.OnValueChangedRefresh();
            }
        }
    }
}