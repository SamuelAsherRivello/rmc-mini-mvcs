using System;
using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

namespace RMC.Mini
{
    public class TestModel2 : BaseModel
    {
        
    }
    public class TestView2 : IView
    {
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        private bool _isInitialized = false;
        private IContext _context;
        public virtual void Initialize(IContext context)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }

        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
    }
    public class TestController2 : BaseController
        <BaseModel,
            TestView2, 
            BaseService>
    {
        public TestController2(
            BaseModel model, 
            TestView2 view, 
            BaseService service) : base(model, view, service)
        {
        }
    }
    
    public class TestService2 : BaseService
    {
        
    }
    
    public class TestSimpleMiniWithBase: SimpleMiniMvcs
        <Context, 
        TestModel2, 
        TestView2, 
        TestController2,
        TestService2>
    {
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                //
                _context = new Context();
                _model = new TestModel2();
                _view = new TestView2();
                _service = new TestService2();
                _controller = new TestController2(_model, _view, _service);
                
                //
                _model.Initialize(_context);
                _view.Initialize(_context);
                _service.Initialize(_context);
                _controller.Initialize(_context);
            }
        }
    }
}
