using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Experimental.Architectures.Mini.Complex
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IMiniMvcsComplex 
        <
        TContext, 
        TModel, 
        TView, 
        TController, 
        TService> 
    
        : IMiniMvcs
    
        where TContext : IContext 
        where TModel : IModel 
        where TView : IView 
        where TController : IController 
        where TService : IService
    {
        TContext Context { get; }

        /// <summary>
        /// The ModelLocator is the only ModelLocator that already exists in the
        /// Context. So we fetch it from there.
        /// </summary>
        Locator<TModel> ModelLocator { get; }

        Locator<TView> ViewLocator { get; }
        Locator<TController> ControllerLocator { get; }
        Locator<TService> ServiceLocator { get; }
    }
        //  Properties ------------------------------------
    

        //  Methods ---------------------------------------
    }
