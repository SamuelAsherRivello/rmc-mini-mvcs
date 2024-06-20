using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

//Keep As:RMC.Mini
namespace RMC.Mini
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IMiniMvcs 
        <
        TContext, 
        TModel, 
        TView, 
        TController, 
        TService> 
    
        : ISimpleMiniMvcs
    
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
