using System;

namespace RMC.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public abstract class BaseModel: IModel
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }

        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        //  Initialization  -------------------------------
        public virtual void Initialize(IContext context)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                // Register Me
                Context.ModelLocator.AddItem(this);
            }
        }

        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        //  Dispose Methods --------------------------------
        public virtual void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Methods ---------------------------------------
        

        //  Event Handlers --------------------------------

    }
}