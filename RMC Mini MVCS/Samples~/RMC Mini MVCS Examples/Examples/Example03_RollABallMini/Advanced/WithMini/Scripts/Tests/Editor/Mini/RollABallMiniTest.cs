using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;
using RMC.Core.Experimental;
using RMC.Core.Testing;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.RollABall")]
    public class RollABallMiniTest
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
            if (ContextLocator.Instance.HasItem<Context.Context>())
            {
                ContextLocator.Instance.RemoveItem<Context.Context>();
            }
        }
        
        [Test]
        public void RollABallMini_DoesNotThrow_WhenInitialize()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs_Advanced/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs_Advanced/PlayerView");
            PickupsView pickupsView = 
                _prefabManagerForTesting.LoadAndInstantiate<PickupsView>("Prefabs_Advanced/PickupsView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs_Advanced/UIView");
            
            // Act
            Assert.DoesNotThrow(() =>
            {
                // Assert
                RollABallMini rollABallMini = 
                    MockRollABallMini.CreateRollABallMini(inputView, playerView, pickupsView, uiView);
            });
        }
    }
}