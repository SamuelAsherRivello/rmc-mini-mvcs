using System.Collections;
using NUnit.Framework;
using RMC.Core.Experimental;
using UnityEngine;
using UnityEngine.TestTools;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Model
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Clock")]
    public class ClockModelPlayModeTest
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
        public void Values_AreDefault_WhenCreated()
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
        
        
        [Test]
        public void Time_Equals0_WhenWaitForSeconds0()
        {
            // Arrange
            ClockSimpleMini clockSimpleMini = MockClockMini.CreateClockMini();
            
            // Act
            clockSimpleMini.Initialize();
            
            // Assert
            Assert.That(clockSimpleMini.Model.Time.Value, Is.EqualTo(0));
        }
        
        
        [UnityTest]
        public IEnumerator Time_Equals1_WhenWaitForSeconds1()
        {
            // Arrange
            ClockSimpleMini clockSimpleMini = MockClockMini.CreateClockMini();
            
            // Act
            clockSimpleMini.Initialize();
            yield return new WaitForSeconds(1);
            
            // Assert
            Assert.That(clockSimpleMini.Model.Time.Value, Is.EqualTo(1));
        }
    }
}