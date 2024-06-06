using System.Collections;
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Components;
using RMC.Core.Experimental;
using RMC.Core.Testing;
using UnityEngine;
using UnityEngine.TestTools;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.RollABall")]
    public class PlayerViewTest
    {
        private static PrefabManagerForTesting _prefabManagerForTesting;
        
        [SetUp]
        public void Setup()
        {
            _prefabManagerForTesting = new PrefabManagerForTesting();
        }

        [TearDown]
        public void TearDown()
        {
            _prefabManagerForTesting.Clear();
     
            if (ContextLocator.Instance.HasItem<Context>())
            {
                ContextLocator.Instance.RemoveItem<Context>();
            }
        }

        
        [UnityTest]
        public IEnumerator OnPickup_IsNotInvoked_WhenNoCollision()
        {
            // Arrange
            IContext context = new Context();
            GameObject gameObject = new GameObject();
            PlayerView playerView = gameObject.AddComponent<PlayerView>();
            
            bool isOnPickup = false;
            playerView.OnPickup.AddListener((pickupComponent) =>
            {
                isOnPickup = false;
            });
            
            // Act
            playerView.Initialize(context);
            
            // Await
            yield return null;
            
            // Assert
            Assert.That(isOnPickup, Is.False);
        }
        
        
        [UnityTest]
        public IEnumerator OnPickup_IsInvoked_WhenCollision()
        {
            // Arrange
            IContext context = new Context();
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs_Advanced/PlayerView");

            bool isOnPickup = false;
            playerView.OnPickup.AddListener((pickupComponent) =>
            {
                isOnPickup = true;
            });
            
            // Act
            playerView.Initialize(context);
            Pickup pickup = 
                _prefabManagerForTesting.LoadAndInstantiate<Pickup>("Prefabs_Advanced/Pickup");
            
            // Await
            yield return _prefabManagerForTesting.WaitForUnityLifeCycle();

            // Assert
            Assert.That(isOnPickup, Is.True);
        }
    }
    
}