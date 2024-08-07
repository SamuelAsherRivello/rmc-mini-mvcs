using NUnit.Framework;
using RMC.Mini.Samples.Clock.WithMini.Mini.Controller.Commands;

namespace RMC.Mini.Samples.Clock.WithMini.Mini.Controller
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Clock")]
    public class ClockControllerTest
    {
        [TearDown]
        public void TearDown()
        {
        }
        
        [Test]
        public void Controller_InvokesTimeChangedCommand_WhenModelTimeChanges()
        {
            // Arrange
            ClockSimpleMini clockSimpleMini = MockClockMini.CreateClockMini();
            clockSimpleMini.Initialize();
            float time = 0;
            clockSimpleMini.Context.CommandManager.AddCommandListener<TimeChangedCommand>((timeChangedCommand) =>
            {
                time = timeChangedCommand.CurrentValue;
            });
            
            
            // Act
            clockSimpleMini.Model.Time.Value = 10;
            
            // Assert
            Assert.That(time, Is.EqualTo(clockSimpleMini.Model.Time.Value));
        }
        
        
    }
}