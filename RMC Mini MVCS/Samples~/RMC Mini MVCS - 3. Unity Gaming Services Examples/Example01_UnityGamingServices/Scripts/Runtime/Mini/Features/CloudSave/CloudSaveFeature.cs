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
    public class CloudSaveFeature: BaseFeature // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            // Model - Shared
            UgsModel ugsModel = MiniMvcs.Context.ModelLocator.GetItem<UgsModel>();
                
            // Model - Local
            CloudSaveModel cloudSaveModel = new CloudSaveModel();

            // Create service
            CloudSaveService service = new CloudSaveService();
            
            // Create new controller
            CloudSaveController controller = 
                new CloudSaveController(cloudSaveModel, View as CloudSaveView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Set Mode
            ugsModel.SceneHasNavigationBack.Value = true;
            ugsModel.SceneHasNavigationDeveloperConsole.Value = true;
            ugsModel.SceneHasAutoAuthentication.Value = true;
            
            // Initialize
            cloudSaveModel.Initialize(MiniMvcs.Context); // Adds itself to a locator inside
            View.Initialize(MiniMvcs.Context);
            service.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
        }

        
        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ModelLocator.HasItem<CloudSaveModel>())
            {
                MiniMvcs.ModelLocator.RemoveAndDisposeItem<CloudSaveModel>();
            }
            
            if (MiniMvcs.ViewLocator.HasItem<CloudSaveView>())
            {
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<CloudSaveView>();
            }
            
            if (MiniMvcs.ControllerLocator.HasItem<CloudSaveController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<CloudSaveController>();
            }
            
        }
    }
}