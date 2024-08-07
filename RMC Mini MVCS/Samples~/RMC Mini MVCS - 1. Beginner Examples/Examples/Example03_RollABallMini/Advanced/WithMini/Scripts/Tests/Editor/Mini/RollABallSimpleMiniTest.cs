using NUnit.Framework;
using RMC.Core.Utilities.Testing;
using RMC.Mini.Samples.RollABall.WithMini.Mini.View;

namespace RMC.Mini.Samples.RollABall.WithMini.Mini
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
                RollABallSimpleMini rollABallSimpleMini = 
                    MockRollABallMini.CreateRollABallMini(inputView, playerView, pickupsView, uiView);
            });
        }
    }
}