using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Controller.Commands;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Controller
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Clock")]
    public class ClockControllerTest
    {
        [Test]
        public void Controller_InvokesTimeChangedCommand_WhenModelTimeChanges()
        {
            // Arrange
            ClockMini clockMini = MockClockMini.CreateClockMini();
            clockMini.Initialize();
            float time = 0;
            clockMini.Context.CommandManager.AddCommandListener<TimeChangedCommand>((timeChangedCommand) =>
            {
                time = timeChangedCommand.CurrentValue;
            });
            
            
            // Act
            clockMini.Model.Time.Value = 10;
            
            // Assert
            Assert.That(time, Is.EqualTo(clockMini.Model.Time.Value));
        }
        
        
    }
}