using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;
using RMC.Core.Experimental;
using RMC.Core.Testing;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.RollABall")]
    public class RollABallControllerTest
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
        public void Controller_InvokesScoreChangedCommand_WhenModelScoreChanges()
        {
            // Arrange
            InputView inputView = 
                _prefabManagerForTesting.LoadAndInstantiate<InputView>("Prefabs_Basic/InputView");
            Assert.NotNull(inputView);
            
            PlayerView playerView = 
                _prefabManagerForTesting.LoadAndInstantiate<PlayerView>("Prefabs_Basic/PlayerView");
            Assert.NotNull(playerView);
            
            UIView uiView = 
                _prefabManagerForTesting.LoadAndInstantiate<UIView>("Prefabs_Basic/UIView");
            Assert.NotNull(uiView);
            
            RollABallSimpleMini rollABallSimpleMini = 
                MockRollABallMini.CreateRollABallMini(inputView, playerView, uiView);
         
            rollABallSimpleMini.Initialize();
            int score = 0;
            rollABallSimpleMini.Context.CommandManager.AddCommandListener<ScoreChangedCommand>(
                (scoreChangedCommand) =>
                {
                    score = scoreChangedCommand.Value;
                });
            
            // Act
            rollABallSimpleMini.RollABallModel.Score.Value = 99;
            
            // Assert
            Assert.That(score, Is.EqualTo(rollABallSimpleMini.RollABallModel.Score.Value));
        }
    }
}