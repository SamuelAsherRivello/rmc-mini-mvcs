using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithoutMini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class OnRestartUnityEvent : UnityEvent<Dialog> {}

    /// <summary>
    /// The UI render interactive control elements to the user
    /// </summary>
    public class UI: MonoBehaviour
    {
        //  Events ----------------------------------------
        [HideInInspector]
        public readonly OnRestartUnityEvent OnRestart = new OnRestartUnityEvent();

        //  Properties ------------------------------------
        public Text ScoreText { get { return _scoreText;} }
        public Text StatusText { get { return _statusText;} }

        //  Fields ----------------------------------------
        [SerializeField] 
        private Text _scoreText;

        [SerializeField] 
        private Text _statusText;

        [SerializeField] 
        private Button _restartButton;

        [SerializeField] 
        private Dialog _dialogPrefab;

        //  Unity Methods ---------------------------------
        protected void Start()
        {
            _restartButton.onClick.AddListener(() => { OnRestart.Invoke(_dialogPrefab); });

        }

        
        //  Methods ---------------------------------------

    }
}