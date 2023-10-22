using RMC.Core.Architectures.Mini.Context;

namespace RMC.Core.Experimental.Architectures.Mini.Module
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IMiniMvcsWithModules : IMiniMvcs
    {
        //  Properties ------------------------------------
        public Locator<IModule> ModuleLocator { get; }

        //  Methods ---------------------------------------
        public void LoadModule(IModule module);
        public void UnloadModule<TModule>() where TModule : IModule;
    }
}