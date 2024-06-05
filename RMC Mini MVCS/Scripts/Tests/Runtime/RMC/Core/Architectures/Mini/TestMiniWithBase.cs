using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.TestMiniWithBase
{
    public class TestModel : BaseModel
    {
        
    }
    public class TestView : IView
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
    public class TestController : BaseController
        <BaseModel,
            TestView, 
            BaseService>
    {
        public TestController(
            BaseModel model, 
            TestView view, 
            BaseService service) : base(model, view, service)
        {
        }
    }
    
    public class TestService : BaseService
    {
        
    }
    
    public class TestMiniWithBase: MiniMvcs
        <Context.Context, 
        TestModel, 
        TestView, 
        TestController,
        TestService>
    {
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                //
                _context = new Context.Context();
                _model = new TestModel();
                _view = new TestView();
                _service = new TestService();
                _controller = new TestController(_model, _view, _service);
                
                //
                _model.Initialize(_context);
                _view.Initialize(_context);
                _service.Initialize(_context);
                _controller.Initialize(_context);
            }
        }
    }
    
}
