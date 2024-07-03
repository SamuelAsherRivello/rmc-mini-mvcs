using NUnit.Framework;

namespace RMC.Mini.WholeMinis.MiniMvcsWithFeatureTests
{
    [Category ("RMC.Mini")]
    public class MiniMvcsWithFeatureTest
    {
        private SampleMiniMvcsWithFeature _mini;

        [SetUp]
        public void SetUp()
        {
            _mini = new SampleMiniMvcsWithFeature();
            _mini.Initialize();
            _mini.AddFeature<SampleFeature>(new SampleFeature());
      
        }

        [Test]
        public void Initialize_ShouldInitializeContextModelAndService()
        {
            
            var context = _mini.Context;
            var model = context.ModelLocator.GetItem<SampleModel>();
            var service = _mini.ServiceLocator.GetItem<SampleService>();
            
            Assert.IsNotNull(context);
            Assert.IsNotNull(model);
            Assert.IsNotNull(service);
            Assert.IsTrue(model.IsInitialized);
            Assert.IsTrue(service.IsInitialized);
        }
        
        [Test]
        public void ModelLocator_GetItemReturnsNotNull_WhenCalledTroughContext()
        {
            var model = _mini.Context.ModelLocator.GetItem<SampleModel>();
            
            Assert.IsNotNull(model);
        }
        
        
        
        [Test]
        public void ModelLocator_GetItemReturnsNotNull_WhenCalledTroughMiniMvcs()
        {
            var model = _mini.ModelLocator.GetItem<SampleModel>();
            
            Assert.IsNotNull(model);
        }
        
    }
}
