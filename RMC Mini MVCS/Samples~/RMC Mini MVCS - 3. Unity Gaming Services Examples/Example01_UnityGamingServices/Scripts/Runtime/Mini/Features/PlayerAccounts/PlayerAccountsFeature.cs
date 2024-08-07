﻿using RMC.Mini.Features;
using RMC.Mini.Samples.UGS.Mini.Controller;
using RMC.Mini.Samples.UGS.Mini.Model;
using RMC.Mini.Samples.UGS.Mini.Service;
using RMC.Mini.Samples.UGS.Mini.View;

namespace RMC.Mini.Samples.UGS.Mini.Feature
{
    /// <summary>
    /// Set up a collection of related <see cref="IConcern"/> instances
    /// </summary>
    public class PlayerAccountsFeature: BaseFeature // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            // Get from mini
            UgsModel model = MiniMvcs.ModelLocator.GetItem<UgsModel>();

            PlayerAccountsService playerAccountsService = new PlayerAccountsService();
            
            // Create new controller
            PlayerAccountsController controller = 
                new PlayerAccountsController(model, View as PlayerAccountsView, playerAccountsService);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Add Settings
            model.SceneHasNavigationBack.Value = true;
            model.SceneHasNavigationDeveloperConsole.Value = true;
            model.SceneHasAutoAuthentication.Value = false;
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            playerAccountsService.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
        }

        
        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<PlayerAccountsController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<PlayerAccountsController>();
            }
            
            if (MiniMvcs.ViewLocator.HasItem<PlayerAccountsView>())
            {
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<PlayerAccountsView>();
            }
        }


    }
}