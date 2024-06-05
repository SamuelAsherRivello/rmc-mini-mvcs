using RMC.Core.Experimental.Architectures.Mini.Complex;
using UnityEngine;
using UnityEngine.Assertions;

namespace RMC.Core.Architectures.Mini.Modules.SceneSystemModule
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class SceneSystemFeature: BaseFeature // Extending 'base' is optional
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        
        //  Initialization  -------------------------------

        //  Methods ---------------------------------------
        
        public override void Build()
        {
            RequireIsInitialized();
            
            //Model
            SceneSystemModel sceneSystemModel = new SceneSystemModel();
            sceneSystemModel.Initialize(MiniMvcs.Context);

            // Get from scene
            SceneSystemView sceneSystemView = GameObject.FindObjectOfType<SceneSystemView>();
            Assert.IsNotNull(sceneSystemView, $"Add the '{nameof(SceneSystemView)}' Prefab to the scene first.");
   
            // Create new controller
            SceneSystemController controller = new SceneSystemController
            (
                sceneSystemModel, 
                sceneSystemView, 
                new DummyService()
            );
            
            // Add to mini
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ViewLocator.AddItem(sceneSystemView);
            
            // Initialize
            sceneSystemView.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context);
        }

        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ControllerLocator.HasItem<SceneSystemController>())
            {
                MiniMvcs.ControllerLocator.RemoveItem<SceneSystemController>();
                MiniMvcs.ViewLocator.RemoveItem<SceneSystemView>();
            }
            
            if (MiniMvcs.ModelLocator.HasItem<SceneSystemModel>())
            {
                MiniMvcs.ModelLocator.RemoveItem<SceneSystemModel>();
            }
        }

        //  Event Handlers --------------------------------

    }
}
