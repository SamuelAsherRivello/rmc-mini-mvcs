using NUnit.Framework;

namespace RMC.Mini.WholeMinis.MiniMvcsTests
{
    [Category ("RMC.Mini")]
    public class MiniMvcsTest
    {
        private SampleMiniMvcs _sampleMiniMvcs;

        [SetUp]
        public void SetUp()
        {
            _sampleMiniMvcs = new SampleMiniMvcs();
        }

        [Test]
        public void Initialize_ShouldSetIsInitializedToTrue()
        {
            _sampleMiniMvcs.Initialize();
            Assert.IsTrue(_sampleMiniMvcs.IsInitialized);
        }

        [Test]
        public void Initialize_ShouldInitializeContextModelAndService()
        {
            _sampleMiniMvcs.Initialize();
            
            var context = _sampleMiniMvcs.Context;
            var model = context.ModelLocator.GetItem<SampleModel>();
            var service = _sampleMiniMvcs.ServiceLocator.GetItem<SampleService>();
            
            Assert.IsNotNull(context);
            Assert.IsNotNull(model);
            Assert.IsNotNull(service);
            Assert.IsTrue(model.IsInitialized);
            Assert.IsTrue(service.IsInitialized);
        }
        
        [Test]
        public void ModelLocator_GetItemReturnsNotNull_WhenCalledTroughContext()
        {
            _sampleMiniMvcs.Initialize();
            
            var model = _sampleMiniMvcs.Context.ModelLocator.GetItem<SampleModel>();
            
            Assert.IsNotNull(model);
        }
        
        [Test]
        public void ModelLocator_GetItemReturnsNotNull_WhenCalledTroughMiniMvcs()
        {
            _sampleMiniMvcs.Initialize();
            
            var model = _sampleMiniMvcs.ModelLocator.GetItem<SampleModel>();
            
            Assert.IsNotNull(model);
        }

    }
}
