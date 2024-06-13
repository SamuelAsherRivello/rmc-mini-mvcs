using RMC.Core.Architectures.Mini.Features;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.Feature
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
            UgsModel model = MiniMvcs.ModelLocator.GetItem<UgsModel>();
            AuthenticationService authenticationService = MiniMvcs.ServiceLocator.GetItem<AuthenticationService>();
            AnalyticsService analyticsService = MiniMvcs.ServiceLocator.GetItem<AnalyticsService>();
            
            // Create new controller
            HudController controller = 
                new HudController(
                    model, 
                    View as HudView, 
                    authenticationService, 
                    analyticsService);
            
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
            
            if (MiniMvcs.ControllerLocator.HasItem<HudController>())
            {
                //TODO: Autodispose on removeitem?
                MiniMvcs.ControllerLocator.GetItem<HudController>().Dispose();
                MiniMvcs.ControllerLocator.RemoveItem<HudController>();
                MiniMvcs.ViewLocator.RemoveItem<HudView>();
            }
        }


    }
}