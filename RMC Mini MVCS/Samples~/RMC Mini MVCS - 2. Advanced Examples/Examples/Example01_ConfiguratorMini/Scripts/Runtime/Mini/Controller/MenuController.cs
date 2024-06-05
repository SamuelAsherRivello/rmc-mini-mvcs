using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Modules.SceneSystemModule;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data;
using RMC.MiniMvcs.Samples.Configurator.Mini.Service;
using RMC.MiniMvcs.Samples.Configurator.Mini.Service.Storage;
using RMC.MiniMvcs.Samples.Configurator.Mini.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RMC.MiniMvcs.Samples.Configurator.Mini.Controller
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
                _view.OnDebugConsole.AddListener(View_OnDebugConsole);
                
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

        
        //  Event Handlers --------------------------------

        
        
        private void View_OnCustomizeCharacter()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new SceneSystemLoadSceneCommand(ConfiguratorConstants.Scene02_CustomizeCharacter));
        }
        

        private void View_OnCustomizeEnvironment()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new SceneSystemLoadSceneCommand(ConfiguratorConstants.Scene03_CustomizeEnvironment));
        }
        
        private void View_OnPlay()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new SceneSystemLoadSceneCommand(ConfiguratorConstants.Scene04_Game));
        }
        
        
        private void View_OnDebugConsole()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new SceneSystemLoadSceneCommand(ConfiguratorConstants.Scene05_DebugConsole));
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