using NUnit.Framework;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Clock")]
    public class ClockMiniTest
    {
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