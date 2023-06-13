
using System.Collections;
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;
using UnityEngine.TestTools;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Service
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.RollABall")]
    public class RollABallServiceTest
    {
        [Test]
        public void Values_AreDefault_WhenCreated()
        {
            // Arrange
            IContext context = new Context.Context();
            RollABallService rollABallService = new RollABallService();
            
            // Act
            rollABallService.Initialize(context);
            
            // Assert
            Assert.That(rollABallService.ScoreMax, Is.EqualTo(0));
        }
        
        
        [UnityTest]
        public IEnumerator ScoreMax_Is3_WhenLoaded()
        {
            // Arrange
            IContext context = new Context.Context();
            RollABallService rollABallService = new RollABallService();
            
            int scoreMax = 0;
            rollABallService.Initialize(context);
            rollABallService.OnLoaded.AddListener(() =>
            {
                scoreMax = rollABallService.ScoreMax;
            });
            
            // Act
            rollABallService.Load();
            yield return new WaitUntil(() => scoreMax != 0);

            // Assert
            Assert.That(scoreMax, Is.EqualTo(3));
        }
        

    }
}