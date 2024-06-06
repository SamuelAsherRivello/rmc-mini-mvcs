
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Components;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;
using RMC.Core.Experimental;
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
            if (ContextLocator.Instance.HasItem<Context>())
            {
                ContextLocator.Instance.RemoveItem<Context>();
            }
        }
        
        [Test]
        public void RollABallModel_DefaultValues_WhenCreated()
        {
            // Arrange
            IContext context = new Context();
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
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs_Basic/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs_Basic/PlayerView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs_Basic/UIView");
            
            RollABallSimpleMini rollABallSimpleMini = 
                MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            rollABallSimpleMini.Initialize();
            
            // Assert
            Assert.That(rollABallSimpleMini.RollABallModel.Score.Value, Is.EqualTo(0));
        }
        
        [Test]
        public void Score_Equals1_WithPickup()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs_Basic/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs_Basic/PlayerView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs_Basic/UIView");
            
            RollABallSimpleMini rollABallSimpleMini = 
                MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            rollABallSimpleMini.Initialize();
            GameObject go = new GameObject();
            PickupComponent pickupComponent = go.AddComponent<PickupComponent>();
            rollABallSimpleMini.RollABallController.PlayerView_OnPickup(pickupComponent);
            
            // Assert
            Assert.That(rollABallSimpleMini.RollABallModel.Score.Value, Is.EqualTo(1));
            
        }
        
        
        [Test]
        public void IsGameOver_EqualsFalse_WithoutPickup()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs_Basic/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs_Basic/PlayerView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs_Basic/UIView");
            
            RollABallSimpleMini rollABallSimpleMini = 
                MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            rollABallSimpleMini.Initialize();
            
            // Assert
            Assert.That(rollABallSimpleMini.RollABallModel.IsGameOver.Value, Is.False);
        }
        
        [Test]
        public void IsGameOver_EqualsTrue_Without3Pickup()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs_Basic/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs_Basic/PlayerView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs_Basic/UIView");
            
            RollABallSimpleMini rollABallSimpleMini = 
                MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            rollABallSimpleMini.Initialize();
            GameObject go = new GameObject();
            PickupComponent pickupComponent = go.AddComponent<PickupComponent>();
            rollABallSimpleMini.RollABallController.PlayerView_OnPickup(pickupComponent);
            rollABallSimpleMini.RollABallController.PlayerView_OnPickup(pickupComponent);
            rollABallSimpleMini.RollABallController.PlayerView_OnPickup(pickupComponent);
            
            // Assert
            Assert.That(rollABallSimpleMini.RollABallModel.IsGameOver.Value, Is.True);
        }
    }
}