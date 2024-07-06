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
    public class MenuFeature: BaseFeature // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            // Get from mini
            UgsModel model = MiniMvcs.ModelLocator.GetItem<UgsModel>();
            AuthenticationService service = MiniMvcs.ServiceLocator.GetItem<AuthenticationService>();
            
            // Create new controller
            MenuController controller = 
                new MenuController(model, View as MenuView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Add Settings
            model.SceneHasNavigationBack.Value = false;
            model.SceneHasNavigationDeveloperConsole.Value = true;
            model.SceneHasAutoAuthentication.Value = false;
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
            
        }

        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ViewLocator.HasItem<MenuView>())
            {
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<MenuView>();
            }
            
            if (MiniMvcs.ControllerLocator.HasItem<MenuController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<MenuController>();
            }
        }
        

    }
}