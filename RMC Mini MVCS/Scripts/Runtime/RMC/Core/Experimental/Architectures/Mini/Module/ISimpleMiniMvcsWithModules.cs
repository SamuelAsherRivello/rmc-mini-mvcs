using RMC.Core.Architectures.Mini.Locators;
using RMC.Core.Architectures.Mini.Structure.Simple;

namespace RMC.Core.Experimental.Architectures.Mini.Module
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface ISimpleMiniMvcsWithModules : ISimpleMiniMvcs
    {
        //  Properties ------------------------------------
        public Locator<IModule> ModuleLocator { get; }

        //  Methods ---------------------------------------
        public void LoadModule(IModule module);
        public void UnloadModule<TModule>() where TModule : IModule;
    }
}