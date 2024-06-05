using System;
using RMC.Core.Architectures.Mini;
using RMC.Core.Architectures.Mini.Model;
using UnityEngine;

namespace RMC.Core.Experimental.Architectures.Mini.ScriptableObjectModels
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public abstract class MonoBehaviorModel: MonoBehaviour, IModel
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
        
        //  Unity Methods ----------------------------------

        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}