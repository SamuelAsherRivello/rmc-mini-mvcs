using RMC.Mini.Features;
using RMC.Mini.Samples.Configurator.Mini.Controller;
using RMC.Mini.Samples.Configurator.Mini.Model;
using RMC.Mini.Samples.Configurator.Mini.Service;
using RMC.Mini.Samples.Configurator.Mini.View;

namespace RMC.Mini.Samples.Configurator.Mini.Feature
{
    /// <summary>
    /// Set up a collection of related <see cref="IConcern"/> instances
    /// </summary>
    public class HudFeature: BaseFeature // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            // Get from mini
            ConfiguratorModel model = MiniMvcs.ModelLocator.GetItem<ConfiguratorModel>();
            ConfiguratorService service = MiniMvcs.ServiceLocator.GetItem<ConfiguratorService>();
            
            // Create new controller
            HudController controller = 
                new HudController(model, View as HudView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
        }

        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ViewLocator.HasItem<HudView>())
            {
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<HudView>();
            }
            
            if (MiniMvcs.ControllerLocator.HasItem<HudController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<HudController>();
            }
        }
    }
}