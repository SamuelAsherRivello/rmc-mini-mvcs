using UnityEngine;

namespace RMC.Mini.Lessons.Scalability.StandardWithFeature.Mini
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
    public class Scalability_03_StandardWithFeature : MonoBehaviour
    {
        //  Fields ----------------------------------------
        [SerializeField] 
        private InventoryView _inventoryView;

        private InventoryMini mini;
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log($"Scalability_03_StandardWithFeature.Start() Click mouse to see action!");
            
            /////////////////////////////////
            // Note: In this demo the mini is of type...
            // InventoryMini and extends MiniMvcs
            
            /////////////////////////////////
            // Create the mini
            mini = InventoryMiniSingleton.Instance.InventoryMini;
            
            /////////////////////////////////
            // Add functionality on top
            AddFeature();
        }

        protected void OnDestroy()
        {
            //Note: OnDestroy is called when the game ends or the scene changes
            //Sometimes you want to remove a feature when the scene changes
            //This is optional and here is one solution.
            RemoveFeature();
            
            // Optional: Handle any cleanup here...
        }

        //  Methods ---------------------------------------
        private void AddFeature()
        {
            InventoryFeature inventoryFeature = new InventoryFeature();
            inventoryFeature.AddView(_inventoryView);
            mini.AddFeature<InventoryFeature>(inventoryFeature);
        }
        
        private void RemoveFeature()
        {
            if (InventoryMiniSingleton.IsShuttingDown)
            {
                return;
            }
            InventoryMini mini = InventoryMiniSingleton.Instance.InventoryMini;
            
            //You can easily, optionally remove the feature too (e.g. when scene changes)
            //mini.RemoveFeature<InventoryFeature>();
        }
        
        //  Event Handlers --------------------------------
    }
}