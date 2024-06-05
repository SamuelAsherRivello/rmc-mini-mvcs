using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;
using RMC.Core.Experimental.Architectures.Mini.Complex;

namespace RMC.Core.Experimental.Architectures.Mini.Module
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
    public class MiniMvcsWithModules: MiniMvcsComplex
            <Context, 
                IModel, 
                IView, 
                IController,
                IService>, IMiniMvcsWithModules
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
                
                _context = new Context();

                // STANDARD
                // ModelLocator is created in superclass
                _viewLocator = new Locator<IView>();
                _controllerLocator = new Locator<IController>();
                _serviceLocator = new Locator<IService>();
                
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