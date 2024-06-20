using RMC.Mini.Controller;
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
    public class DeveloperConsoleController: BaseController // Extending 'base' is optional
        <ConfiguratorModel, DeveloperConsoleView, ConfiguratorService> 
    {
        public DeveloperConsoleController(
            ConfiguratorModel model, DeveloperConsoleView view, ConfiguratorService service) 
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
                _view.OnReset.AddListener(View_OnReset);

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
        private void View_OnReset()
        {
            RequireIsInitialized();

            // Set from Random. Then save here.
            _model.CharacterData.Value = CharacterData.FromDefaultValues();
            _service.SaveCharacterData(_model.CharacterData.Value);
            
            _model.EnvironmentData.Value = EnvironmentData.FromDefaultValues();
            _service.SaveEnvironmentData(_model.EnvironmentData.Value);
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