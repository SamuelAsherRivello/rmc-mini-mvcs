using System;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Locators;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

//Keep as: RMC.Core.Architectures.Mini
namespace  RMC.Core.Architectures.Mini
{
    /// <summary>
    /// The MiniMvcs is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    /// </summary>
    public class MiniMvcs
        <TContext,
        TModel,
        TView,
        TController,
        TService> 
        
        : IMiniMvcs<
            TContext, 
            TModel, 
            TView, 
            TController, 
            TService> 
    
        where  TContext : IContext 
        where TModel : IModel 
        where TView : IView
        where TController : IController 
        where TService : IService 
    
    {

        //  Events ----------------------------------------


        //  Fields ----------------------------------------
        protected bool _isInitialized = false;
        protected TContext _context;
        protected Locator<TView>   _viewLocator;
        protected Locator<TController> _controllerLocator;
        protected Locator<TService>  _serviceLocator;

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public TContext Context { get { return _context;} }
        
        /// <summary>
        /// The ModelLocator is the only ModelLocator that already exists in the
        /// Context. So we fetch it from there.
        /// </summary>
        public Locator<TModel> ModelLocator { get { return _context.ModelLocator as Locator<TModel>;} }
        public Locator<TView> ViewLocator { get { return _viewLocator;} }
        public Locator<TController> ControllerLocator { get { return _controllerLocator;} }
        public Locator<TService> ServiceLocator { get { return _serviceLocator;} }

        //  Initialization --------------------------------
        public virtual void Initialize()
        {
            throw new Exception("MustOverrideMethod");
        }

        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}