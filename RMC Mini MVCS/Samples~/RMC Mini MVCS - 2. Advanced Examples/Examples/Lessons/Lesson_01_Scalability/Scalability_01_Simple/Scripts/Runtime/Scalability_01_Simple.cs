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
       
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log($"Scalability_01_Simple.Start() Click mouse to see action!");

            /////////////////////////////////
            // Note: In this demo the mini is of type...
            // SimpleInventorySimpleMini and extends SimpleMiniMvcs
            
            /////////////////////////////////
            // Create the mini
            SimpleInventorySimpleMini simpleInventorySimpleMini = 
                new SimpleInventorySimpleMini(_inventoryView);
            
            /////////////////////////////////
            // Initialize
            simpleInventorySimpleMini.Initialize();
        }

        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }

        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
    }
}