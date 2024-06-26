using RMC.Core.Observables;
using RMC.Mini.Model;

namespace RMC.Mini.Samples.RollABall.WithMini.Mini.Model
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class RollABallModel: BaseModel // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        public Observable<bool> IsGameOver { get { return _isGameOver;} }
        public Observable<bool> IsGamePaused { get { return _isGamePaused;} }
        public Observable<int> Score { get { return _score;} }
        public Observable<int> ScoreMax { get { return _scoreMax;} }
        public Observable<string> StatusText { get { return _statusText;} }
        public Observable<float> PlayerMovementSpeed { get { return _playerMovementSpeed;} }

        
        //  Fields ----------------------------------------
        private readonly Observable<bool> _isGameOver = new Observable<bool>();
        private readonly Observable<bool> _isGamePaused = new Observable<bool>();
        private readonly Observable<int> _score = new Observable<int>();
        private readonly Observable<int> _scoreMax = new Observable<int>();
        private readonly Observable<string> _statusText = new Observable<string>();
        private readonly Observable<float> _playerMovementSpeed = new Observable<float>();
        
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
                _playerMovementSpeed.Value = 0.7f;
            }
        }
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}