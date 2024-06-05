using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Experimental.Architectures.Mini.Complex
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IInitializableWithMiniComplex
    {
        //  Properties  ------------------------------------
        public bool IsInitialized { get; }
        public IMiniMvcsComplex<Context,
            IModel,
            IView,
            IController,
            IService> MiniMvcs { get; }

        //  General Methods  ------------------------------
        public void Initialize(IMiniMvcsComplex<Context,
            IModel,
            IView,
            IController,
            IService> MiniMvcs);
        
        void RequireIsInitialized();
    }
}