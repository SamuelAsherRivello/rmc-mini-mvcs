using RMC.Mini.Features;
using RMC.Mini.Samples.UGS.Mini.Controller;
using RMC.Mini.Samples.UGS.Mini.Model;
using RMC.Mini.Samples.UGS.Mini.Service;
using RMC.Mini.Samples.UGS.Mini.View;

namespace RMC.Mini.Samples.UGS.Mini.Feature
{
    /// <summary>
    /// Set up a collection of related <see cref="IConcern"/> instances
    /// </summary>
    public class DeveloperConsoleFeature: BaseFeature // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            // Get from mini
            UgsModel model = MiniMvcs.ModelLocator.GetItem<UgsModel>();

            // Create **THREE* Services
            CloudSaveService cloudSaveService = new CloudSaveService();
            UserGeneratedContentService userGeneratedContentService = new UserGeneratedContentService();
            
            MiniMvcs.ServiceLocator.AddItem(cloudSaveService);
            MiniMvcs.ServiceLocator.AddItem(userGeneratedContentService);
            
            cloudSaveService.Initialize(MiniMvcs.Context);
            userGeneratedContentService.Initialize(MiniMvcs.Context);
            
            // Create new controller
            DeveloperConsoleController controller =
                new DeveloperConsoleController(model,
                    View as DeveloperConsoleView);
            
            // Add services. Since its more than one, create a custom method
            controller.AddServices
            (
                cloudSaveService,
                userGeneratedContentService
            );
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Add Settings
            model.SceneHasNavigationBack.Value = true;
            model.SceneHasNavigationDeveloperConsole.Value = true;
            model.SceneHasAutoAuthentication.Value = true;
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
            

            
        }

        
        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<DeveloperConsoleController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<DeveloperConsoleController>();
                MiniMvcs.ServiceLocator.RemoveAndDisposeItem<CloudSaveService>();
                MiniMvcs.ServiceLocator.RemoveAndDisposeItem<UserGeneratedContentService>();
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<DeveloperConsoleView>();
            }
            
        }
    }
}