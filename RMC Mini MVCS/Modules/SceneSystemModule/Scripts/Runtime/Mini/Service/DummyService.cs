using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Service;

namespace RMC.Core.Architectures.Mini.Modules.SceneSystemModule
{
    //  Namespace Properties ------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class DummyService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        
        
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
        
        //  Event Handlers --------------------------------
    }
}