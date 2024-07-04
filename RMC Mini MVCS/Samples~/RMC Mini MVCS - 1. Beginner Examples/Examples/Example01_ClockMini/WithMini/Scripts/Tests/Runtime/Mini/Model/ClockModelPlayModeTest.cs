using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace RMC.Mini.Samples.Clock.WithMini.Mini.Model
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Clock")]
    public class ClockModelPlayModeTest
    {
        private bool _ignoreFailingMessagesBefore;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _ignoreFailingMessagesBefore = LogAssert.ignoreFailingMessages;
            
            // NOW, INSIDE every test set LogAssert.ignoreFailingMessages = true;
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            LogAssert.ignoreFailingMessages = _ignoreFailingMessagesBefore;
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
            LogAssert.ignoreFailingMessages = true;
            ClockSimpleMini clockSimpleMini = MockClockMini.CreateClockMini();
            
            // Act
            clockSimpleMini.Initialize();
            
            // Assert
            Assert.That(clockSimpleMini.Model.Time.Value, Is.EqualTo(0));
        }
        
        
        [UnityTest]
        public IEnumerator Time_Equals1_WhenWaitForSeconds0()
        {
            // Arrange
            LogAssert.ignoreFailingMessages = true;
            ClockSimpleMini clockSimpleMini = MockClockMini.CreateClockMini();
            
            // Act
            clockSimpleMini.Initialize();
            yield return new WaitForSeconds(1);
            
            // Assert
            Assert.That(clockSimpleMini.Model.Time.Value, Is.EqualTo(0));
        }
        
        [UnityTest]
        public IEnumerator Time_Equals2_WhenWaitForSeconds1()
        {
            // Arrange
            LogAssert.ignoreFailingMessages = true;
            ClockSimpleMini clockSimpleMini = MockClockMini.CreateClockMini();
            
            // Act
            clockSimpleMini.Initialize();
            yield return new WaitForSeconds(2);
            
            // Assert
            Assert.That(clockSimpleMini.Model.Time.Value, Is.EqualTo(1));
        }
    }
}