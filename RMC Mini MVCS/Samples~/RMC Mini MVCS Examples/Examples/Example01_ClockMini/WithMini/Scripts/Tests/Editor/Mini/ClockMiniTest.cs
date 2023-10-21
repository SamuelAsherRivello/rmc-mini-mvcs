using NUnit.Framework;
using RMC.Core.Experimental;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Clock")]
    public class ClockMiniTest
    {
        [TearDown]
        public void TearDown()
        {
            if (ContextLocator.Instance.HasItem<Context.Context>())
            {
                ContextLocator.Instance.RemoveItem<Context.Context>();
            }
        }
        
        [Test]
        public void ClockMini_DoesNotThrow_WhenInitialize()
        {
            // Arrange
            ClockMini clockMini = MockClockMini.CreateClockMini();
            
            // Assert
            Assert.DoesNotThrow(() =>
            {
                
                //Act
                clockMini.Initialize();
                
            });
        }
        
        
    }
}