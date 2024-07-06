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

        private InventoryMini _mini;
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log($"Scalability_02_Standard.Start() Click mouse to see action!");
        
            /////////////////////////////////
            // Create Mini
            // It's fairly empty
            _mini = InventoryMiniSingleton.Instance.InventoryMini;
            
            /////////////////////////////////
            // Then add functionality on top.
            InventoryModel model = _mini.ModelLocator.GetItem<InventoryModel>();
            InventoryService service = _mini.ServiceLocator.GetItem<InventoryService>();
            
            // Create new controller
            InventoryController controller = 
                new InventoryController(model, _inventoryView, service);
            
            // Add to mini
            _mini.ControllerLocator.AddItem(controller);
            _mini.ViewLocator.AddItem(_inventoryView);
            
            // Initialize
            _inventoryView.Initialize(_mini.Context);
            controller.Initialize(_mini.Context);
        }

        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }

        
        //  Methods ---------------------------------------
        
        //  Event Handlers --------------------------------
    }
}