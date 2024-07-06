using RMC.Mini.Controller;
using RMC.Mini.Features;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;
using UnityEngine;

namespace RMC.Mini.WholeMinis.MiniMvcsWithFeatureTests
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
    
    public class SampleFeature: BaseFeature // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        //  Methods ---------------------------------------
        public override void Build()
        {
            RequireIsInitialized();
            
            // Model
            SampleModel model = new SampleModel();
        
            // View
            SampleView view = new SampleView();
                
            // Service
            SampleService service = new SampleService();

            // Controller
            SampleController controller = new SampleController(model, view, service);
            
            // Add To Locators
            MiniMvcs.ViewLocator.AddItem(view);
            MiniMvcs.ControllerLocator.AddItem(controller);
            MiniMvcs.ServiceLocator.AddItem(service);
            
            // Initialize
            model.Initialize(MiniMvcs.Context); //Added to locator inside
            view.Initialize(MiniMvcs.Context);
            service.Initialize(MiniMvcs.Context);
            controller.Initialize(MiniMvcs.Context); //do last
        }

        
        public override void Dispose()
        {
            RequireIsInitialized();
            
            if (MiniMvcs.ModelLocator.HasItem<SampleModel>())
            {
                MiniMvcs.ModelLocator.RemoveAndDisposeItem<SampleModel>();
            }
            
            if (MiniMvcs.ViewLocator.HasItem<SampleView>())
            {
                MiniMvcs.ViewLocator.RemoveAndDisposeItem<SampleView>();
            }
            
            if (MiniMvcs.ControllerLocator.HasItem<SampleController>())
            {
                MiniMvcs.ControllerLocator.RemoveAndDisposeItem<SampleController>();
            }
            
            if (MiniMvcs.ServiceLocator.HasItem<SampleService>())
            {
                MiniMvcs.ServiceLocator.RemoveAndDisposeItem<SampleService>();
            }
        }
    }



    /// <summary>
    /// See <see cref="MiniMvcs{TContext,TModel,TView,TController,TService}"/>
    /// </summary>
    public class SampleMiniMvcsWithFeature: MiniMvcs
            <Context, 
                IModel, 
                IView, 
                IController,
                IService> // TODO: Do a test with IBlah for all here as shown.
                          //       Then, Do another test with concrete classes
    {
        
        //  Fields ----------------------------------------

        
        //  Properties ------------------------------------
        private FeatureBuilder FeatureBuilder { get; set; }
        
        //  Initialization  -------------------------------
        public override void Initialize()
        {
            
            Debug.Log($"{this.GetType().Name}.Initialize()) ");
            
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                // Context
                _context = new Context();
                
                // Model
                
                
                // View
                
                
                // Controller
     
                
                // Service

                
                // Feature
                FeatureBuilder = new FeatureBuilder();
                FeatureBuilder.Initialize(this);
                
            }
        }

        
        //  Methods  -------------------------------
        public bool HasFeature<TFeature>(string key = "") where TFeature : IFeature
        {
            return FeatureBuilder.HasFeature<TFeature>(key);
        }
        
        public void AddFeature<TFeature>(TFeature feature, string key = "") where TFeature : IFeature
        {
            FeatureBuilder.AddFeature<TFeature>(feature, key);
        }
        
        public void RemoveFeature<TFeature>(bool willDispose, string key = "") where TFeature : IFeature
        {
            FeatureBuilder.RemoveFeature<TFeature>(key, willDispose);
        }
        
    }
}