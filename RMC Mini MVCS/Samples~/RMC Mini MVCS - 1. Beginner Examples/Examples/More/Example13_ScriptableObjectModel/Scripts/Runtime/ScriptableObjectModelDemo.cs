using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.SOM.Mini
{
    /// <summary>
    /// Demo of using a ScriptableObject as a Model
    /// </summary>
    public class ScriptableObjectModelDemo : MonoBehaviour
    {
        //  Fields ----------------------------------------
        [SerializeField] 
        private InventoryView _inventoryView;
       
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log($"ScriptableObjectModelDemo.Start() Click mouse to see action!");
        
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
            InventoryScriptableObjectModel scriptableObjectModel = inventoryMini.ModelLocator.GetItem<InventoryScriptableObjectModel>();
            InventoryService service = inventoryMini.ServiceLocator.GetItem<InventoryService>();
            
            // Create new controller
            InventoryController controller = 
                new InventoryController(scriptableObjectModel, _inventoryView, service);
            
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