using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model.Data;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Service.Storage;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Controller
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

        
        //  Event Handlers --------------------------------
        private void View_OnRandomize()
        {
            RequireIsInitialized();

            // Set from Random. Then save here.
            ScriptableObjectModel.EnvironmentData.Value = EnvironmentData.FromRandomValues();
            _service.SaveEnvironmentData(ScriptableObjectModel.EnvironmentData.Value);
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