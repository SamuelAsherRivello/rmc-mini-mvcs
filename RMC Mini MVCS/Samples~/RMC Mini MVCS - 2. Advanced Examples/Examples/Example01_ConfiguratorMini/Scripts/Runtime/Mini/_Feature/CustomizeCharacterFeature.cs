using RMC.Core.Architectures.Mini.Features;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Feature
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class CustomizeCharacterFeature: BaseFeature // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        
        //  Initialization  -------------------------------

        //  Methods ---------------------------------------

        
        public override void Build()
        {
            RequireIsInitialized();
            
            // Get from mini
            ConfiguratorModel model = MiniMvcs.ModelLocator.GetItem<ConfiguratorModel>();
            ConfiguratorService service = MiniMvcs.ServiceLocator.GetItem<ConfiguratorService>();
            
            // Create new controller
            CustomizeCharacterController controller = 
                new CustomizeCharacterController(model, View as CustomizeCharacterView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
            
            // Set Mode
            model.HasNavigationBack.Value = true;
            model.HasNavigationDeveloperConsole.Value = true;
        }

        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<CustomizeCharacterController>())
            {
                MiniMvcs.ControllerLocator.RemoveItem<CustomizeCharacterController>();
                MiniMvcs.ViewLocator.RemoveItem<CustomizeCharacterView>();
            }
        }

        //  Event Handlers --------------------------------

    }
}