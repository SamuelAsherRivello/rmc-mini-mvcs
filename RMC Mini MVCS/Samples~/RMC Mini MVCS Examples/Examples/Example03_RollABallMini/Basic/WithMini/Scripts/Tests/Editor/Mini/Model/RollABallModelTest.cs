
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Components;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;
using RMC.Core.Testing;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Model
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.RollABall")]
    public class RollABallModelTest
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
        }
        
        [Test]
        public void RollABallModel_DefaultValues_WhenCreated()
        {
            // Arrange
            IContext context = new Context.Context();
            RollABallModel rollABallModel = new RollABallModel();
            
            // Act
            rollABallModel.Initialize(context);
            
            // Assert
            Assert.That(rollABallModel.Score, Is.Not.EqualTo(0));
            Assert.That(rollABallModel.ScoreMax, Is.Not.EqualTo(0));
            Assert.That(rollABallModel.StatusText, Is.Not.EqualTo(""));
            Assert.That(rollABallModel.IsGameOver, Is.Not.EqualTo(false));
            Assert.That(rollABallModel.IsGamePaused, Is.Not.EqualTo(false));
        }
        
        
        [Test]
        public void Score_Equals0_WithoutPickup()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs/PlayerView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs/UIView");
            
            RollABallMini rollABallMini = 
                MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            rollABallMini.Initialize();
            
            // Assert
            Assert.That(rollABallMini.RollABallModel.Score.Value, Is.EqualTo(0));
        }
        
        [Test]
        public void Score_Equals1_WithPickup()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs/PlayerView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs/UIView");
            
            RollABallMini rollABallMini = 
                MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            rollABallMini.Initialize();
            GameObject go = new GameObject();
            PickupComponent pickupComponent = go.AddComponent<PickupComponent>();
            rollABallMini.RollABallController.PlayerView_OnPickup(pickupComponent);
            
            // Assert
            Assert.That(rollABallMini.RollABallModel.Score.Value, Is.EqualTo(1));
            
        }
        
        
        [Test]
        public void IsGameOver_EqualsFalse_WithoutPickup()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs/PlayerView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs/UIView");
            
            RollABallMini rollABallMini = 
                MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            rollABallMini.Initialize();
            
            // Assert
            Assert.That(rollABallMini.RollABallModel.IsGameOver.Value, Is.False);
        }
        
        [Test]
        public void IsGameOver_EqualsTrue_Without3Pickup()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs/PlayerView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs/UIView");
            
            RollABallMini rollABallMini = 
                MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            rollABallMini.Initialize();
            GameObject go = new GameObject();
            PickupComponent pickupComponent = go.AddComponent<PickupComponent>();
            rollABallMini.RollABallController.PlayerView_OnPickup(pickupComponent);
            rollABallMini.RollABallController.PlayerView_OnPickup(pickupComponent);
            rollABallMini.RollABallController.PlayerView_OnPickup(pickupComponent);
            
            // Assert
            Assert.That(rollABallMini.RollABallModel.IsGameOver.Value, Is.True);
        }
    }
}