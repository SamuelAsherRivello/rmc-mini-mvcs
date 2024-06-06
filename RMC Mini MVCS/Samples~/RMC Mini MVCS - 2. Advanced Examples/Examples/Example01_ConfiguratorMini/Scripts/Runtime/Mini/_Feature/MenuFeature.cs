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
    public class MenuFeature: BaseFeature // Extending 'base' is optional
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
            MenuController controller = 
                new MenuController(model, View as MenuView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
            
            // Set Mode
            model.HasNavigationBack.Value = false;
            model.HasNavigationDeveloperConsole.Value = true;
        }

        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<MenuController>())
            {
                //TODO: Maybe make RemoveItem(willDispose==true) for all locators?
                MiniMvcs.ControllerLocator.GetItem<MenuController>().Dispose();
                MiniMvcs.ControllerLocator.RemoveItem<MenuController>();
                MiniMvcs.ViewLocator.RemoveItem<MenuView>();
            }
        }

        //  Event Handlers --------------------------------

    }
}