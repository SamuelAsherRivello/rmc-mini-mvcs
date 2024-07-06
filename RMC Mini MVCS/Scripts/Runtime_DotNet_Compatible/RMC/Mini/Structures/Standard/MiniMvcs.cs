using System;
using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

//Keep As:RMC.Mini
namespace RMC.Mini
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
        where TModel : class, IModel 
        where TView : class, IView
        where TController : class, IController 
        where TService : class, IService 
    
    {

        //  Events ----------------------------------------


        //  Fields ----------------------------------------
        protected bool _isInitialized = false;
        protected TContext _context;
        protected readonly Locator<TView> _viewLocator;
        protected readonly Locator<TController> _controllerLocator;
        protected readonly Locator<TService>  _serviceLocator;

        //  Properties ------------------------------------


        public bool IsInitialized { get { return _isInitialized;} }

        public TContext Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// The ModelLocator is the only ModelLocator that already exists in the
        /// Context. So we fetch it from there.
        /// </summary>
        public Locator<TModel> ModelLocator { get { return _context?.ModelLocator.RecastLocatorAs<TModel>();} }
        public Locator<TView> ViewLocator { get { return _viewLocator;} }
        public Locator<TController> ControllerLocator { get { return _controllerLocator;} }
        public Locator<TService> ServiceLocator { get { return _serviceLocator;} }

        //  Initialization --------------------------------
        public MiniMvcs()
        {
            // Model
            // ...ModelLocator is already fetched from context
            
            // Others
            _viewLocator = new Locator<TView>();
            _controllerLocator = new Locator<TController>();
            _serviceLocator = new Locator<TService>();
            
        }
        
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