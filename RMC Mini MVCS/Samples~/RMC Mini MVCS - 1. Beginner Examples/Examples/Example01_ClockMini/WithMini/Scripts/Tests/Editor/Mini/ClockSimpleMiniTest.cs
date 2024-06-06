using NUnit.Framework;
using RMC.Core.Experimental;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Clock")]
    public class ClockSimpleMiniTest
    {
        [TearDown]
        public void TearDown()
        {
            if (ContextLocator.Instance.HasItem<Context>())
            {
                ContextLocator.Instance.RemoveItem<Context>();
            }
        }
        
        [Test]
        public void ClockMini_DoesNotThrow_WhenInitialize()
        {
            // Arrange
            ClockSimpleMini clockSimpleMini = MockClockMini.CreateClockMini();
            
            // Assert
            Assert.DoesNotThrow(() =>
            {
                
                //Act
                clockSimpleMini.Initialize();
                
            });
        }
        
        
    }
}