using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;
using UnityEngine;

namespace RMC.Mini.WholeMinis.MiniMvcsTests
{
    public class SampleModel : BaseModel
    {
        private string _sampleData;

        public string SampleData
        {
            get
            {
                RequireIsInitialized();
                return _sampleData;
            }
            set
            {
                RequireIsInitialized();
                _sampleData = value;
            }
        }

        public override void Initialize(IContext context)
        {
            base.Initialize(context);
            _sampleData = "Default Data";
        }

        public void PrintSampleData()
        {
            RequireIsInitialized();
        }
    }
    
    public class SampleController : BaseController
        <SampleModel,SampleView, SampleService>
    {
        public SampleController(SampleModel model, SampleView view, SampleService service) : 
            base(model, view, service)
        {
            
        }
    }

    public class SampleView : DummyView
    {
    }
    

    public class SampleService : BaseService
    {
        private string _serviceData;

        public string ServiceData
        {
            get
            {
                RequireIsInitialized();
                return _serviceData;
            }
            set
            {
                RequireIsInitialized();
                _serviceData = value;
            }
        }

        public override void Initialize(IContext context)
        {
            base.Initialize(context);
            _serviceData = "Default Data";
        }

        public void PrintServiceData()
        {
            RequireIsInitialized();
        }
    }
    

    /// <summary>
    /// See <see cref="MiniMvcs{TContext,TModel,TView,TController,TService}"/>
    /// </summary>
    public class SampleMiniMvcs: MiniMvcs
            <Context, 
                IModel, 
                IView, 
                IController,
                IService> // TODO: Do a test with IBlah for all here as shown.
                          //       Then, Do another test with concrete classes
    {
        
        //  Fields ----------------------------------------

        //  Properties ------------------------------------
        
        //  Initialization  -------------------------------
        public override void Initialize()
        {
            
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                // Context
                _context = new Context();
                
                // Model
                SampleModel model = new SampleModel();
        
                // View
                SampleView view = new SampleView();
                
                // Service
                SampleService service = new SampleService();

                // Controller
                SampleController controller = new SampleController(model, view, service);
            
                // Add To Locators
                ViewLocator.AddItem(view);
                ControllerLocator.AddItem(controller);
                ServiceLocator.AddItem(service);
            
                // Initialize
                model.Initialize(Context); //Added to locator inside
                view.Initialize(Context);
                service.Initialize(Context);
                controller.Initialize(Context); //do last
                
            }
        }

        
        //  Methods  -------------------------------
        
    }
}