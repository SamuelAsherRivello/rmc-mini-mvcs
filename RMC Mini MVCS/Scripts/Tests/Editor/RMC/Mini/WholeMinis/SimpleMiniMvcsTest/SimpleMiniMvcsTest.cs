using NUnit.Framework;

namespace RMC.Mini.WholeMinis.SimpleMiniMvcsTests
{
    
    [Category ("RMC.Mini")]
    public class SimpleMiniMvcsTest
    {
        private SampleSimpleMiniMvcs _sampleSimpleMiniMvcs;

        [SetUp]
        public void Setup()
        {
            _sampleSimpleMiniMvcs = new SampleSimpleMiniMvcs();
        }
        
        [TearDown]
        public void TearDown()
        {
            if (_sampleSimpleMiniMvcs != null)
            {
                _sampleSimpleMiniMvcs.Dispose();
                _sampleSimpleMiniMvcs = null;
            }
        }
        
        
        [Test]
        public void TestMini_IsNotNull_WhenCreated()
        {
            // Arrange
            _sampleSimpleMiniMvcs.Initialize();
            
            // Act
    
            
            // Assert
            Assert.That(_sampleSimpleMiniMvcs, Is.Not.Null);
            Assert.That(_sampleSimpleMiniMvcs.Context, Is.Not.Null);
            Assert.That(_sampleSimpleMiniMvcs.Model, Is.Not.Null);
            Assert.That(_sampleSimpleMiniMvcs.View, Is.Not.Null);
            Assert.That(_sampleSimpleMiniMvcs.Controller, Is.Not.Null);
            Assert.That(_sampleSimpleMiniMvcs.Service, Is.Not.Null);
            
        }
    }
}
