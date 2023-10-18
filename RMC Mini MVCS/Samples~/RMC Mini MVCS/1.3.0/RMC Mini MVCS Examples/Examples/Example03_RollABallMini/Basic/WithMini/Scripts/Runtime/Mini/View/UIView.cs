using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class OnRestartUnityEvent : UnityEvent<DialogView> {}

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class UIView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector]
        public readonly OnRestartUnityEvent OnRestart = new OnRestartUnityEvent();

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }

        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private Text _scoreText;

        [SerializeField] 
        private Text _statusText;

        [SerializeField] 
        private Button _restartButton;

        [SerializeField] 
        private DialogView _dialogViewPrefab;

        private int _score = 0;
        private int _scoreMax = 0;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                if (_restartButton != null)
                {
                    _restartButton.onClick.AddListener(() =>
                    {
                        OnRestart.Invoke(_dialogViewPrefab);
                    });
                }
                
                //
                Context.CommandManager.AddCommandListener<ScoreChangedCommand>(
                    OnScoreChangedCommand);
                Context.CommandManager.AddCommandListener<ScoreMaxChangedCommand>(
                    OnScoreMaxChangedCommand);
                Context.CommandManager.AddCommandListener<StatusChangedCommand>(
                    OnStatusChangedCommand);
                
                // Refresh
                UpdateScoreText();
            }
        }

        
        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Unity Methods ---------------------------------
        protected void OnDestroy()
        {
            Context?.CommandManager?.RemoveCommandListener<ScoreChangedCommand>(
                OnScoreChangedCommand);
            Context?.CommandManager?.RemoveCommandListener<StatusChangedCommand>(
                OnStatusChangedCommand);
        }

        
        //  Methods ---------------------------------------
        private void UpdateScoreText()
        {
            if (_scoreText != null)
            {
                _scoreText.text = $"Score: {_score}/{_scoreMax}";
            }
        }
        
        
        //  Event Handlers --------------------------------
        private void OnScoreChangedCommand(ScoreChangedCommand scoreChangedCommand)
        {
            RequireIsInitialized();

            if (_scoreText != null)
            {
                _score = scoreChangedCommand.Value;
                UpdateScoreText();
            }
        }

        
        private void OnScoreMaxChangedCommand(ScoreMaxChangedCommand scoreMaxChangedCommand)
        {
            RequireIsInitialized();

            if (_scoreText != null)
            {
                _scoreMax = scoreMaxChangedCommand.Value;
                UpdateScoreText();
            }
        }
        
        private void OnStatusChangedCommand(StatusChangedCommand statusChangedCommand)
        {
            RequireIsInitialized();

            if (_statusText != null)
            {
                _statusText.text = $"{statusChangedCommand.Value}";
            }
        }

    }
}