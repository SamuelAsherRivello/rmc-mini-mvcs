using System;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.Features
{
    /// <summary>
    /// A <see cref="BaseFeature"/> is a collection of one or more <see cref="IConcern"/>
    /// You can turn on and off something in the game (like an inventory system)
    /// by adding or removing your custom inventory-related <see cref="IFeature"/>
    /// </summary>
    public abstract class BaseFeature: IFeature
    {
        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IMiniMvcs<Mini.Context, IModel, IView, IController, IService> MiniMvcs { get; private set; }

        //  Fields ----------------------------------------
        public IView View { get; private set; }
        
        //  Initialization  -------------------------------
        public virtual void Initialize(IMiniMvcs<Mini.Context, IModel, IView, IController, IService> miniMvcs)
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
    }
}