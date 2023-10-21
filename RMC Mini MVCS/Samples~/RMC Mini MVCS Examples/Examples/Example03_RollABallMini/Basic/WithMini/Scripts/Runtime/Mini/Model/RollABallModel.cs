using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class RollABallModel: BaseModel // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<bool> IsGameOver { get { return _isGameOver;} }
        public Observable<bool> IsGamePaused { get { return _isGamePaused;} }
        public Observable<int> Score { get { return _score;} }
        public Observable<int> ScoreMax { get { return _scoreMax;} }
        public Observable<string> StatusText { get { return _statusText;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<bool> _isGameOver = new Observable<bool>();
        private readonly Observable<bool> _isGamePaused = new Observable<bool>();
        private readonly Observable<int> _score = new Observable<int>();
        private readonly Observable<int> _scoreMax = new Observable<int>();
        private readonly Observable<string> _statusText = new Observable<string>();
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Set Defaults
                _isGameOver.Value = false;
                _isGamePaused.Value = false;
                _score.Value = 0;
                _scoreMax.Value = 0;
                _statusText.Value = "";
            }
        }
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}