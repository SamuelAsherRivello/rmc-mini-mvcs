using RMC.Core.Architectures.Mini.Service;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.SOM.Mini
{
    //  Namespace Properties ------------------------------
    public class InventoryServiceUnityEvent : UnityEvent<string> {}

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class InventoryService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly InventoryServiceUnityEvent OnLoadCompleted = new InventoryServiceUnityEvent();
        
        
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
            }
        }

        
        //  Methods ---------------------------------------
        public void Load ()
        {
            RequireIsInitialized();
            
            //TODO: Load Data from some source...
            var dummyMessage = "Hello World!";
            
            OnLoadCompleted.Invoke(dummyMessage);
        }
        
        //  Event Handlers --------------------------------
    }
}