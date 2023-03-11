using NUnit.Framework;
using RMC.Core.Architectures.Mini.TestMiniWithoutBase;

namespace RMC.Core.Architectures.Mini
{
    
    [Category ("RMC.Mini")]
    public class MiniTest
    {
        [Test]
        public void TestMini_IsNotNull_WhenCreated()
        {
            // Arrange
            
            // Act
            TestMini testMini = new TestMini();
            testMini.Initialize();
            
            // Assert
            Assert.That(testMini, Is.Not.Null);
        }
        
        [Test]
        public void TestMiniWithBase_IsNotNull_WhenCreated()
        {
            // Arrange
            
            // Act
            TestMiniWithBase.TestMiniWithBase testMiniWithBase = 
                new TestMiniWithBase.TestMiniWithBase();
            
            testMiniWithBase.Initialize();
            
            // Assert
            Assert.That(testMiniWithBase, Is.Not.Null);
        }
    }
}
