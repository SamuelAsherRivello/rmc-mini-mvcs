using RMC.Core.Architectures.Mini.Features;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.Feature
{
    /// <summary>
    /// Set up a collection of related <see cref="IConcern"/> instances
    /// </summary>
    public class UserGeneratedContentFeature: BaseFeature // Extending 'base' is optional
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
            UserGeneratedContentModel userGeneratedContentModel = new UserGeneratedContentModel();
            
            // Create service
            UserGeneratedContentService userGeneratedContentService = new UserGeneratedContentService();
            
            // Create new controller
            UserGeneratedContentController controller = 
                new UserGeneratedContentController(userGeneratedContentModel, View as UserGeneratedContentView, userGeneratedContentService);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Set Mode
            ugsModel.SceneHasNavigationBack.Value = true;
            ugsModel.SceneHasNavigationDeveloperConsole.Value = true;
            ugsModel.SceneHasAutoAuthentication.Value = true;
            
            // Initialize
            userGeneratedContentModel.Initialize(MiniMvcs.Context); // Adds itself to a locator inside
            View.Initialize(MiniMvcs.Context);
            userGeneratedContentService.Initialize(MiniMvcs.Context);
            
            controller.Initialize(MiniMvcs.Context);
        }

        
        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<UserGeneratedContentController>())
            {
                //TODO: Autodispose on removeitem?
                MiniMvcs.ControllerLocator.GetItem<UserGeneratedContentController>().Dispose();
                MiniMvcs.ControllerLocator.RemoveItem<UserGeneratedContentController>();
                MiniMvcs.ModelLocator.RemoveItem<UserGeneratedContentModel>();
                MiniMvcs.ViewLocator.RemoveItem<UserGeneratedContentView>();
            }
        }
    }
}