using RMC.Mini.Samples.RollABall.WithMini.Mini.View;

namespace RMC.Mini.Samples.RollABall.WithMini.Mini
{
    /// <summary>
    /// This mock class creates a new object instance
    /// for use in testing.
    /// </summary>
    public class MockRollABallMini
    {
        public static RollABallSimpleMini CreateRollABallMini(InputView inputView, 
            PlayerView playerView,
            PickupsView pickupsView,
            UIView uiView)
        {
                
            RollABallSimpleMini rollABallSimpleMini = new RollABallSimpleMini(inputView, playerView, pickupsView, uiView);
            return rollABallSimpleMini;
        }
    }
}