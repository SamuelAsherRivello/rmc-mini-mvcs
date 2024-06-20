using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

//Keep As:RMC.Mini
namespace RMC.Mini
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IInitializableWithMini
    {
        //  Properties  ------------------------------------
        public bool IsInitialized { get; }
        public IMiniMvcs<global::RMC.Mini.Context,
            IModel,
            IView,
            IController,
            IService> MiniMvcs { get; }

        //  General Methods  ------------------------------
        public void Initialize(IMiniMvcs<global::RMC.Mini.Context,
            IModel,
            IView,
            IController,
            IService> miniMvcs);
        
        void RequireIsInitialized();
    }
}