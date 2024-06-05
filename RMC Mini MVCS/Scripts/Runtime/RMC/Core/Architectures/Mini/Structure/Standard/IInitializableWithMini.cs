using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

//Keep as: RMC.Core.Architectures.Mini
namespace  RMC.Core.Architectures.Mini
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IInitializableWithMini
    {
        //  Properties  ------------------------------------
        public bool IsInitialized { get; }
        public IMiniMvcs<Mini.Context,
            IModel,
            IView,
            IController,
            IService> MiniMvcs { get; }

        //  General Methods  ------------------------------
        public void Initialize(IMiniMvcs<Mini.Context,
            IModel,
            IView,
            IController,
            IService> miniMvcs);
        
        void RequireIsInitialized();
    }
}