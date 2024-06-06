using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.SOM.Mini
{
    /// <summary>
    /// The ScriptableObject to store data
    /// </summary>
    [CreateAssetMenu(fileName = "InventoryData", menuName = "RMC/Mini Mvcs/ScriptableObject Model Demo/InventoryData", order = -100)]
    public class InventoryData: ScriptableObject // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        public int InitialInventoryCount { get { return _initialInventoryCount;} }
        
        //  Fields ----------------------------------------
        [SerializeField] 
        private int _initialInventoryCount = 1;

        //  Initialization  -------------------------------

        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------

    }
}