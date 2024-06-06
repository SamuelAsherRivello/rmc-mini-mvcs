using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Features.SceneSystem;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model.Data;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Service.Storage;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Controller
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
                if (!ScriptableObjectModel.HasLoadedService.Value)
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
            ScriptableObjectModel.HasLoadedService.Value = true;
            
            if (configuratorServiceData != null)
            {
                // Set FROM the saved data. Don't save again here.
                ScriptableObjectModel.CharacterData.Value = configuratorServiceData.CharacterData;
                ScriptableObjectModel.EnvironmentData.Value = configuratorServiceData.EnvironmentData;
            }
            else
            {
                ScriptableObjectModel.CharacterData.OnValueChangedRefresh();
                ScriptableObjectModel.EnvironmentData.OnValueChangedRefresh();
            }
        }
    }
}