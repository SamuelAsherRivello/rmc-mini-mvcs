using System;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.Structure.Simple;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.TestMiniWithoutBase
{
    public class TestActor : IConcern
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
    public class TestModel : TestActor, IModel {}
    public class TestView : TestActor, IView {}

    public class TestController : TestActor, IController
    {
        public virtual void Dispose() {}
    }
    public class TestService : TestActor, IService {}
    public class TestSimpleMini: SimpleMiniMvcs
        <Mini.Context, 
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
                string contextKey = Guid.NewGuid().ToString();
                _context = new Mini.Context(contextKey);
                _model = new TestModel();
                _view = new TestView();
                _service = new TestService();
                _controller = new TestController();
                _context.ModelLocator.AddItem(_model);
                //
                _model.Initialize(_context);
                _view.Initialize(_context);
                _service.Initialize(_context);
                _controller.Initialize(_context);
            }
        }
    }
    
}
