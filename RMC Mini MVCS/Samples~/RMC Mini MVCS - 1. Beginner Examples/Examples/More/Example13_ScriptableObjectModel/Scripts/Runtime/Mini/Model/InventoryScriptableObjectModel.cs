using RMC.Core.Data.Types;
using RMC.Core.Experimental.Architectures.Mini.ScriptableObjectModels;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.SOM.Mini
{
    /// <summary>
    /// The <see cref="InventoryScriptableObjectModel"/> uses a MonoBehaviour
    /// to hold a ScriptableObject for easy editing in the inspector for initial values
    ///
    /// This demo GETS (but does not SET ) data on the ScriptableObject at runtime. 
    /// </summary>
    public class InventoryScriptableObjectModel: ScriptableObjectModel 
    {
        //  Properties ------------------------------------
        public Observable<int> InventoryCount { get { return _inventoryCount;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<int> _inventoryCount = new Observable<int>();

        [SerializeField] 
        private InventoryData _inventoryData;
        
        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------
      
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Exactly once, the value is copied from the ScriptableObject as getter
                // There is no setter for the initial value. In this demo, you must set the
                // Initial value through the Unity Inspector
                _inventoryCount.Value = _inventoryData.InitialInventoryCount;
            }
        }

        //  Unity Methods ----------------------------------
        protected void Awake()
        {
            // 
            Initialize(InventoryMiniSingleton.Instance.InventoryMini.Context);
        }

        
        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
 
    }
}