using System;
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
                string contextKey = Guid.NewGuid().ToString();
                _context = new Context(contextKey);

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

        public void UnloadModule<TModule>() where TModule : IModule
        {
            _moduleLocator.RemoveItem<TModule>();
        }
        
        //  Event Handlers --------------------------------

    }
}