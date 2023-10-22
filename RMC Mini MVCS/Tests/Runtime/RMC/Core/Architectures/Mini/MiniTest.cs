using NUnit.Framework;
using RMC.Core.Architectures.Mini.TestMiniWithoutBase;

namespace RMC.Core.Architectures.Mini
{
    
    [Category ("RMC.Mini")]
    public class MiniTest
    {
        private TestMiniWithBase.TestMiniWithBase _testMiniWithBase;
        private TestMini _testMini;

        [TearDown]
        public void TearDown()
        {
            if (_testMiniWithBase != null)
            {
                _testMiniWithBase.Dispose();
                _testMiniWithBase = null;
            }
            
            if (_testMini != null)
            {
                _testMini.Dispose();
                _testMini = null;
            }
        }
        
        
        [Test]
        public void TestMini_IsNotNull_WhenCreated()
        {
            // Arrange
            
            // Act
            _testMini = new TestMini();
            _testMini.Initialize();
            
            // Assert
            Assert.That(_testMini, Is.Not.Null);
            Assert.That(_testMini.Context, Is.Not.Null);
            Assert.That(_testMini.Model, Is.Not.Null);
            Assert.That(_testMini.View, Is.Not.Null);
            Assert.That(_testMini.Controller, Is.Not.Null);
            Assert.That(_testMini.Service, Is.Not.Null);
            
        }
        
        [Test]
        public void TestMiniWithBase_IsNotNull_WhenCreated()
        {
            // Arrange
            
            // Act
            _testMiniWithBase = 
                new TestMiniWithBase.TestMiniWithBase();
            _testMiniWithBase.Initialize();
            
            // Assert
            Assert.That(_testMiniWithBase, Is.Not.Null);
            Assert.That(_testMiniWithBase.Context, Is.Not.Null);
            Assert.That(_testMiniWithBase.Model, Is.Not.Null);
            Assert.That(_testMiniWithBase.View, Is.Not.Null);
            Assert.That(_testMiniWithBase.Controller, Is.Not.Null);
            Assert.That(_testMiniWithBase.Service, Is.Not.Null);
            
        }
    }
}
