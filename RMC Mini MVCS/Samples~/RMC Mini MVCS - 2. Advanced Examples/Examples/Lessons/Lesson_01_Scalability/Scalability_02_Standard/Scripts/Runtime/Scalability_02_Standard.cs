using UnityEngine;

namespace RMC.Mini.Lessons.Scalability.Standard.Mini
{
    /// <summary>
    /// When scaling your project from a simple game to a complex game MiniMvcs
    /// can scale with you. Here are some examples.
    ///
    /// * <see cref="Scalability_01_Simple"/> - Basic setup.
    ///                         - Good for a simple structure
    ///                         - Good for one model, one view, one controller, & one service
    ///                         - Good for single-Scene usage.
    /// 
    /// * <see cref="Scalability_02_Standard"/> - Uses "Singleton".
    ///                         - Good for a complex structure
    ///                         - Good for one or more: models, views, controllers, & services
    ///                         - Good for multi-Scene usage.
    ///
    /// * <see cref="Scalability_03_StandardWithFeature"/> - Uses "Singleton" and "Feature".
    ///                         - Good for a complex structure
    ///                         - Good for one or more: models, views, controllers, & services
    ///                         - Good for multi-Scene usage.
    ///                         - The "Feature" Helps you turn on/off subsystems of your game easily
    /// </summary>
    public class Scalability_02_Standard : MonoBehaviour
    {
        //  Fields ----------------------------------------
        [SerializeField] 
        private InventoryView _inventoryView;
       
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log($"Scalability_02_Standard.Start() Click mouse to see action!");
        
            /////////////////////////////////
            // Note: In this demo the mini is of type...
            // SimpleInventorySimpleMini and extends SimpleMiniMvcs
            
            /////////////////////////////////
            // Create Mini
            // It's pretty empty
            InventoryMini inventoryMini = 
                InventoryMiniSingleton.Instance.InventoryMini;
            
            /////////////////////////////////
            // Then add functionality on top.
            InventoryModel model = inventoryMini.ModelLocator.GetItem<InventoryModel>();
            InventoryService service = inventoryMini.ServiceLocator.GetItem<InventoryService>();
            
            // Create new controller
            InventoryController controller = 
                new InventoryController(model, _inventoryView, service);
            
            // Add to mini
            inventoryMini.ControllerLocator.AddItem(controller);
            inventoryMini.ViewLocator.AddItem(_inventoryView);
            
            // Initialize
            _inventoryView.Initialize(inventoryMini.Context);
            controller.Initialize(inventoryMini.Context);
        }

        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }

        
        //  Methods ---------------------------------------
        
        //  Event Handlers --------------------------------
    }
}