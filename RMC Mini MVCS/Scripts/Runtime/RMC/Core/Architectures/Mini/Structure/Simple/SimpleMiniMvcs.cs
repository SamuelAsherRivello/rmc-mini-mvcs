using System;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.Structure.Simple
{
    /// <summary>
    /// The SimpleMiniMvcs is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    ///
    /// This structure has a maximum of one of each type of concern.
    ///
    /// It is best only for VERY simple use cases.
    /// 
    /// </summary>
    public class SimpleMiniMvcs
        <TContext,
        TModel,
        TView,
        TController,
        TService> : ISimpleMiniMvcs, IDisposable
    
        where TContext : IContext 
        where TModel : IModel 
        where TView : IView
        where TController : IController 
        where TService : IService 
    
    {
        protected TContext _context;
        protected TController _controller;


        //  Fields ----------------------------------------
        protected bool _isInitialized = false;
        protected TModel _model;
        protected TService _service;
        protected TView _view;
        public TContext Context { get { return _context;} }
        public TModel Model { get { return _model;} }
        public TView View { get { return _view;} }
        public TController Controller { get { return _controller;} }

        public TService Service { get { return _service;} }
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }


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

        public virtual void Dispose()
        {
            _isInitialized = false;
            _context.Dispose();
            
        }

        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
  
    }
}