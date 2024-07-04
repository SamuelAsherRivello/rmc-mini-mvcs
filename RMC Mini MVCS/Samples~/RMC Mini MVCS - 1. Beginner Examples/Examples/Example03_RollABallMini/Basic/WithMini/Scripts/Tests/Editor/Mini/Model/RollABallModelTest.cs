
using NUnit.Framework;
using RMC.Core.Utilities.Testing;
using RMC.Mini.Experimental.ContextLocators;
using RMC.Mini.Samples.RollABall.WithMini.Components;
using RMC.Mini.Samples.RollABall.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Mini.Samples.RollABall.WithMini.Mini.Model
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.RollABall")]
    public class RollABallModelTest
    {
        private RollABallSimpleMini _mini;
        private static PrefabManagerForTesting _prefabManagerForTesting;
        private IContext _context;
        
        [SetUp]
        public void Setup()
        {
            //Teardown is called AFTER tests, but ONLY when no exceptions are thrown
            //so also call it here
            TearDown();
            
            //Do setup
            _context = ContextWithLocator.CreateNew();
            _prefabManagerForTesting = new PrefabManagerForTesting();
        }

        [TearDown]
        public void TearDown()
        {
            if (_prefabManagerForTesting != null)
            {
                _prefabManagerForTesting.Clear();
            }

            if (_mini != null)
            {
                _mini.Dispose();
            }
            
            if (_context != null)
            {
                _context.Dispose();
            }
        }
        
        [Test]
        public void RollABallModel_DefaultValues_WhenCreated()
        {
            // Arrange
    
            RollABallModel rollABallModel = new RollABallModel();
            
            // Act
            rollABallModel.Initialize(_context);
            
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
            
           
            _mini = MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            _mini.Initialize();
            
            // Assert
            Assert.That(_mini.RollABallModel.Score.Value, Is.EqualTo(0));
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
            
            _mini = MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            _mini.Initialize();
            GameObject go = new GameObject();
            PickupComponent pickupComponent = go.AddComponent<PickupComponent>();
            _mini.RollABallController.PlayerView_OnPickup(pickupComponent);
            
            // Assert
            Assert.That(_mini.RollABallModel.Score.Value, Is.EqualTo(1));
            
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
            
            _mini = MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            _mini.Initialize();
            
            // Assert
            Assert.That(_mini.RollABallModel.IsGameOver.Value, Is.False);
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
            
            _mini = MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            // Act
            _mini.Initialize();
            GameObject go = new GameObject();
            PickupComponent pickupComponent = go.AddComponent<PickupComponent>();
            _mini.RollABallController.PlayerView_OnPickup(pickupComponent);
            _mini.RollABallController.PlayerView_OnPickup(pickupComponent);
            _mini.RollABallController.PlayerView_OnPickup(pickupComponent);
            
            // Assert
            Assert.That(_mini.RollABallModel.IsGameOver.Value, Is.True);
        }
    }
}