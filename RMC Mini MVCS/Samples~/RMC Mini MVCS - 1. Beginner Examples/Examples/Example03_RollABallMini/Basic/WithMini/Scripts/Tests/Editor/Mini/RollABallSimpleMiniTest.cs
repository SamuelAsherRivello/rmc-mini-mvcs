using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;
using RMC.Core.Experimental;
using RMC.Core.Testing;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.RollABall")]
    public class RollABallSimpleMiniTest
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
        public void RollABallMini_DoesNotThrow_WhenInitialize()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs_Basic/InputView");
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs_Basic/PlayerView");
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs_Basic/UIView");
            
            // Act
            Assert.DoesNotThrow(() =>
            {
                // Assert
                RollABallSimpleMini rollABallSimpleMini = 
                    MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
            });
        }
    }
}