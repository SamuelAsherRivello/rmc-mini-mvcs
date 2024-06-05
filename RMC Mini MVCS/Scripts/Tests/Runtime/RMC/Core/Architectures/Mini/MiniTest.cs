using NUnit.Framework;
using RMC.Core.Architectures.Mini.TestMiniWithoutBase;

namespace RMC.Core.Architectures.Mini
{
    
    [Category ("RMC.Mini")]
    public class MiniTest
    {
        private TestMiniWithBase.TestSimpleMiniWithBase _testSimpleMiniWithBase;
        private TestSimpleMini _testSimpleMini;

        [TearDown]
        public void TearDown()
        {
            if (_testSimpleMiniWithBase != null)
            {
                _testSimpleMiniWithBase.Dispose();
                _testSimpleMiniWithBase = null;
            }
            
            if (_testSimpleMini != null)
            {
                _testSimpleMini.Dispose();
                _testSimpleMini = null;
            }
        }
        
        
        [Test]
        public void TestMini_IsNotNull_WhenCreated()
        {
            // Arrange
            
            // Act
            _testSimpleMini = new TestSimpleMini();
            _testSimpleMini.Initialize();
            
            // Assert
            Assert.That(_testSimpleMini, Is.Not.Null);
            Assert.That(_testSimpleMini.Context, Is.Not.Null);
            Assert.That(_testSimpleMini.Model, Is.Not.Null);
            Assert.That(_testSimpleMini.View, Is.Not.Null);
            Assert.That(_testSimpleMini.Controller, Is.Not.Null);
            Assert.That(_testSimpleMini.Service, Is.Not.Null);
            
        }
        
        [Test]
        public void TestMiniWithBase_IsNotNull_WhenCreated()
        {
            // Arrange
            
            // Act
            _testSimpleMiniWithBase = 
                new TestMiniWithBase.TestSimpleMiniWithBase();
            _testSimpleMiniWithBase.Initialize();
            
            // Assert
            Assert.That(_testSimpleMiniWithBase, Is.Not.Null);
            Assert.That(_testSimpleMiniWithBase.Context, Is.Not.Null);
            Assert.That(_testSimpleMiniWithBase.Model, Is.Not.Null);
            Assert.That(_testSimpleMiniWithBase.View, Is.Not.Null);
            Assert.That(_testSimpleMiniWithBase.Controller, Is.Not.Null);
            Assert.That(_testSimpleMiniWithBase.Service, Is.Not.Null);
            
        }
    }
}
