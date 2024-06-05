using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Experimental.Architectures.Mini.Complex
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public abstract class BaseFeature: IFeature
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IMiniMvcsComplex<Context, IModel, IView, IController, IService> MiniMvcs { get; private set; }

        //  Fields ----------------------------------------
        public IView View { get; private set; }
        
        //  Initialization  -------------------------------
        public virtual void Initialize(IMiniMvcsComplex<Context, IModel, IView, IController, IService> miniMvcs)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                MiniMvcs = miniMvcs;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }

        //  Methods ---------------------------------------
        public void AddView(IView view)
        {
            //Do not require initialized
            View = view;
        }
        
        public virtual void Build()
        {
            //MUST override and do NOT call base
            throw new Exception($"Must override {nameof(Build)} method.");
        }
        
        public virtual void Dispose()
        {
            //MUST override and do NOT call base
            throw new Exception($"Must override {nameof(Dispose)} method.");
        }

        //  Event Handlers --------------------------------

  
    }
}