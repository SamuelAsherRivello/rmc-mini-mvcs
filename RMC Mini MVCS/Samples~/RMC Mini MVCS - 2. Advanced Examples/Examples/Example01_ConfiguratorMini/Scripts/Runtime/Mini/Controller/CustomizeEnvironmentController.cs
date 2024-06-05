using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data;
using RMC.MiniMvcs.Samples.Configurator.Mini.Service;
using RMC.MiniMvcs.Samples.Configurator.Mini.Service.Storage;
using RMC.MiniMvcs.Samples.Configurator.Mini.View;
using UnityEngine.SceneManagement;

namespace RMC.MiniMvcs.Samples.Configurator.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class CustomizeEnvironmentController: BaseController // Extending 'base' is optional
        <ConfiguratorModel, CustomizeEnvironmentView, ConfiguratorService> 
    {
        public CustomizeEnvironmentController(
            ConfiguratorModel model, CustomizeEnvironmentView view, ConfiguratorService service) 
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
                _view.OnRandomize.AddListener(View_OnRandomize);

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
        private void View_OnRandomize()
        {
            RequireIsInitialized();

            // Set from Random. Then save here.
            _model.EnvironmentData.Value = EnvironmentData.FromRandomValues();
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