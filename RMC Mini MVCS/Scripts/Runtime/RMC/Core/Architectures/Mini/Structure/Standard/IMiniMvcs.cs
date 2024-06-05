using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Locators;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.Structure.Simple;
using RMC.Core.Architectures.Mini.View;

//Keep as: RMC.Core.Architectures.Mini
namespace  RMC.Core.Architectures.Mini
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
