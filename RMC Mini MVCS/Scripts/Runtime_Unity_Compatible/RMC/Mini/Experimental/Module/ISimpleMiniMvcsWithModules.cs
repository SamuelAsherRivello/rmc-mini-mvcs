namespace RMC.Mini.Experimental.Module
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