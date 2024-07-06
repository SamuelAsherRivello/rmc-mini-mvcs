using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

namespace RMC.Mini.Experimental.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The MiniMvcsWithModules is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    ///
    /// It is compatible with <see cref="IModule"/>
    /// </summary>
    public class MiniMvcsWithModules: MiniMvcs
            <Context, 
                IModel, 
                IView, 
                IController,
                IService>, ISimpleMiniMvcsWithModules
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Locator<IModule> ModuleLocator { get { return _moduleLocator;} }


        //  Fields ----------------------------------------
        protected Locator<IModule>  _moduleLocator;

        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                // Context
                _context = new Context();

                // EXTRA
                _moduleLocator = new Locator<IModule>();
            }
        }

        //  Methods ---------------------------------------
        public void LoadModule(IModule module)
        {
            _moduleLocator.AddItem(module);
            module.Initialize();
        }

        public void UnloadModule<TModule>(bool willDispose) where TModule : IModule
        {
            throw new System.NotImplementedException();
        }


        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        


        
        //  Event Handlers --------------------------------

    }
}