using RMC.Core.Architectures.Mini;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Data.Types;

namespace RMC.Mini.Lessons.Scalability.Simple.Mini
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class InventoryModel: BaseModel // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        public Observable<int> InventoryCount { get { return _inventoryCount;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<int> _inventoryCount = new Observable<int>();
        
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _inventoryCount.Value = 0;
            }
        }

        
        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
 
    }
}