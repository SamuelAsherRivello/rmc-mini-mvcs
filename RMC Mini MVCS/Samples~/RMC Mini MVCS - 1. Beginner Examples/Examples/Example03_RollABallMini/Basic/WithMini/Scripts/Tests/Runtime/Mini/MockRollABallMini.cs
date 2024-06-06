using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini
{
    /// <summary>
    /// This mock class creates a new object instance
    /// for use in testing.
    /// </summary>
    public class MockRollABallMini
    {
        public static RollABallSimpleMini CreateRollABallMini(InputView inputView, PlayerView playerView,
            UIView uiView)
        {
                
            RollABallSimpleMini rollABallSimpleMini = new RollABallSimpleMini(inputView, playerView, uiView);
            return rollABallSimpleMini;
        }
    }
}