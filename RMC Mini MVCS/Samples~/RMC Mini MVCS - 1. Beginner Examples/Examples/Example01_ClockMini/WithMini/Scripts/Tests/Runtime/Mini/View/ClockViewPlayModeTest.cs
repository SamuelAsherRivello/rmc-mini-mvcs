using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Controller.Commands;
using RMC.Core.Experimental;
using UnityEngine;
using UnityEngine.TestTools;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.View
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
            if (ContextLocator.Instance.HasItem<Context>())
            {
                ContextLocator.Instance.RemoveItem<Context>();
            }
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
        
        [UnityTest]
        public IEnumerator DebugLog_Something_WhenTimeChangedCommand()
        {
            // Arrange
            IContext context = new Context();
            ClockView clockView = new ClockView();
            
            // Act
            clockView.Initialize(context);
            context.CommandManager.InvokeCommand(new TimeChangedCommand (0, 1));
            
            // Await
            yield return null;
            
            // Assert
            LogAssert.Expect(LogType.Log, new Regex("Elapsed Time: 1 seconds"));
        }
    }
}