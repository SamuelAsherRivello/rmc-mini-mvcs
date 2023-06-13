using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini
{
    /// <summary>
    /// This mock class creates a new object instance
    /// for use in testing.
    /// </summary>
    public class MockRollABallMini
    {
        public static RollABallMini CreateRollABallMini(InputView inputView, PlayerView playerView,
            UIView uiView)
        {
                
            RollABallMini rollABallMini = new RollABallMini(inputView, playerView, uiView);
            return rollABallMini;
        }
    }
}