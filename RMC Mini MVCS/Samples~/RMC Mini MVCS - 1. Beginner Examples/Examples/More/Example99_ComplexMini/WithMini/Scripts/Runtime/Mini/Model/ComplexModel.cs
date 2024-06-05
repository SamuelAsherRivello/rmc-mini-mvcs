using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------
    public enum GameMode
    {
        Menu,
        Game
    }
    
    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class ComplexModel: BaseModel // Extending 'base' is optional
    {
        //  Enums ----------------------------------------


        //  Properties ------------------------------------
        public Observable<GameMode> GameMode { get { return _gameMode;} }
        public Observable<string> SceneName { get { return _sceneName;} }
        public Observable<bool> ServiceHasLoaded { get { return _serviceHasLoaded;} }
        public Observable<bool> IsServiceDirty { get { return _isServiceDirty;} }
        public Observable<int> Score { get { return _score;} }
        public Observable<int> ScoreMin { get { return _scoreMin;} }

        public Observable<int> ScoreMax { get { return _scoreMax;} }
        public bool IsGameOver
        {
            get
            {
                return Score.Value >= ScoreMax.Value;
            }
        }

        
        //  Fields ----------------------------------------
        private readonly Observable<GameMode> _gameMode = new Observable<GameMode>();
        private readonly Observable<string> _sceneName = new Observable<string>();
        private readonly Observable<bool> _serviceHasLoaded = new Observable<bool>();
        private readonly Observable<bool> _isServiceDirty = new Observable<bool>();
        private readonly Observable<int> _score = new Observable<int>();
        private readonly Observable<int> _scoreMax = new Observable<int>();
        private readonly Observable<int> _scoreMin = new Observable<int>();

        public const string MultiSceneMiniExample01 = "Scene01_Menu";
        public const string MultiSceneMiniExample02 = "Scene02_Game";
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                //
                _sceneName.OnValueChanged.AddListener(SceneName_OnValueChanged);
                
                // Set Defaults
                _sceneName.Value = "";
                _serviceHasLoaded.Value = false;
                _isServiceDirty.Value = false;
                _scoreMin.Value = 0;
                _score.Value = _score.Value;
                _scoreMax.Value = 3;
            }
        }

        //  Methods ---------------------------------------
 

        //  Event Handlers --------------------------------
        private void SceneName_OnValueChanged(string previousValue, string currentValue)
        {
            Context.CommandManager.InvokeCommand(
                new SceneNameChangedCommand(previousValue, currentValue));
        }
    }
}