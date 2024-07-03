using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace RMC.Mini.Samples.Clock.WithMini.Mini.View
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Clock")]
    public class ClockViewPlayModeTest
    {
        [TearDown]
        public void TearDown()
        {
        }
        
        [UnityTest]
        public IEnumerator DebugLog_Nothing_WhenCreated()
        {
            // Arrange
            IContext context = new Context();
            ClockView clockView = new ClockView();
            
            // Act
            clockView.Initialize(context);
            
            // Await
            yield return null;
            
            // Assert
            LogAssert.NoUnexpectedReceived();   
        }
        
    }
}