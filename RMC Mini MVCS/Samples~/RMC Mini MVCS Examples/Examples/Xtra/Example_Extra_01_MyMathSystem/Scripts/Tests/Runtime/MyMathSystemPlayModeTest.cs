using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace RMC.IntroToUnity.Samples.MyMathSystem
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.MyMathSystem")]
    public class MyMathSystemPlayModeTest
    {
        [UnityTest]
        public IEnumerator Add_ResultIs15_When5And10()
        {
            // Arrange
            MyMathSystem myMathSystem = new MyMathSystem();
            
            // Act
            int sum = myMathSystem.Add(5, 10);
            
            // Await
            yield return new WaitForSeconds(1);
            
            // Assert
            Assert.That(sum, Is.EqualTo(15));
        }
    }
}