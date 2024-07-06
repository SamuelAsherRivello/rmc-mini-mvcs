using RMC.Mini.Lessons.Scalability.Standard.Mini;
using RMC.Mini.Lessons.Scalability.StandardWithFeature.Mini;
using UnityEngine;

namespace RMC.Mini.Lessons.Scalability.Simple.Mini
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
    public class Scalability_01_Simple : MonoBehaviour
    {
        //  Fields ----------------------------------------
        [SerializeField] 
        private InventoryView _inventoryView;

        private SimpleInventorySimpleMini _mini;
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log($"Scalability_01_Simple.Start() Click mouse to see action!");

            /////////////////////////////////
            // Create the mini
            _mini = new SimpleInventorySimpleMini(_inventoryView);
            
            /////////////////////////////////
            // Initialize
            _mini.Initialize();
        }

        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }

        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
    }
}