﻿using RMC.Core.Experimental.Architectures.Mini.Complex;
using RMC.MiniMvcs.Samples.Configurator.Mini.Controller;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model;
using RMC.MiniMvcs.Samples.Configurator.Mini.Service;
using RMC.MiniMvcs.Samples.Configurator.Mini.View;

namespace RMC.MiniMvcs.Samples.Configurator.Mini.Feature
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class DebugConsoleFeature: BaseFeature // Extending 'base' is optional
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
            DebugConsoleController controller = 
                new DebugConsoleController(model, View as DebugConsoleView, service);
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(View);
            
            // Initialize
            View.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
            
            // Set Mode
            model.HasBackNavigation.Value = true;
        }

        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<DebugConsoleController>())
            {
                MiniMvcs.ControllerLocator.RemoveItem<DebugConsoleController>();
                MiniMvcs.ViewLocator.RemoveItem<DebugConsoleView>();
            }
        }

        //  Event Handlers --------------------------------

    }
}