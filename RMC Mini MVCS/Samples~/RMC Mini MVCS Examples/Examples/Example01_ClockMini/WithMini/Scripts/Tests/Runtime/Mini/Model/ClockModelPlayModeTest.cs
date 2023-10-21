using System.Collections;
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Context;
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
            if (ContextLocator.Instance.HasItem<Context.Context>())
            {
                ContextLocator.Instance.RemoveItem<Context.Context>();
            }
        }
        
        
        [Test]
        public void Values_AreDefault_WhenCreated()
        {
            // Arrange
            IContext context = new Context.Context();
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
            ClockMini clockMini = MockClockMini.CreateClockMini();
            
            // Act
            clockMini.Initialize();
            
            // Assert
            Assert.That(clockMini.Model.Time.Value, Is.EqualTo(0));
        }
        
        
        [UnityTest]
        public IEnumerator Time_Equals1_WhenWaitForSeconds1()
        {
            // Arrange
            ClockMini clockMini = MockClockMini.CreateClockMini();
            
            // Act
            clockMini.Initialize();
            yield return new WaitForSeconds(1);
            
            // Assert
            Assert.That(clockMini.Model.Time.Value, Is.EqualTo(1));
        }
    }
}