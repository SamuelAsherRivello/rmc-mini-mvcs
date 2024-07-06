using System;
using RMC.Mini.Controller;
using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

namespace RMC.Mini.WholeMinis.SimpleMiniMvcsTests
{
    public class TestActor1 : IConcern
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
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
    }
    public class TestModel1 : TestActor1, IModel {}
    public class TestView1: TestActor1, IView {}

    public class TestController1 : TestActor1, IController
    {
        public virtual void Dispose() {}
    }
    public class TestService1 : TestActor1, IService {}
    public class SampleSimpleMiniMvcs: SimpleMiniMvcs
    <Context, 
        TestModel1, 
        TestView1, 
        TestController1,
        TestService1>
    {
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                //
                _context = new Context();
                _model = new TestModel1();
                _view = new TestView1();
                _service = new TestService1();
                _controller = new TestController1();
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