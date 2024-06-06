using NUnit.Framework;
using RMC.Core.Experimental;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Model
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Clock")]
    public class ClockModelTest
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
        public void ClockModelTest_DefaultValues_WhenCreated()
        {
            // Arrange
            IContext context = new Context();
            ClockModel clockModel = new ClockModel();
            
            // Act
            clockModel.Initialize(context);
            
            // Assert
            Assert.That(clockModel.Time.Value, Is.EqualTo(0));
            Assert.That(clockModel.TimeDelay.Value, Is.EqualTo(0));
        }
    }
}