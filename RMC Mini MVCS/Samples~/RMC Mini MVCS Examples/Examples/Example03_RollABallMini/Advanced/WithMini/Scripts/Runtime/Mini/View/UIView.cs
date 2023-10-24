using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

// ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class OnRestartUnityEvent : UnityEvent {}

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
        
        public Label ScoreLabel { get { return _uiDocument.rootVisualElement.Q<Label>("ScoreLabel");} }
        public Label StatusLabel { get { return _uiDocument.rootVisualElement.Q<Label>("StatusLabel");} }
        public Button RestartButton { get { return _uiDocument.rootVisualElement.Q<Button>("RestartButton");} }
        
        public DialogView DialogView { get { return _dialogView; }}

        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private UIDocument _uiDocument;

        private DialogView _dialogView;

        private int _score = 0;
        private int _scoreMax = 0;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                RestartButton.clicked += RestartButton_OnClicked;

     
                // ///////////////////////////////////////////////////
                // View 
                // There are many options of how to handle this
                //
                // Chosen Option
                // * Create a wrapper class of type IView
                // * This wrapper knows the structure inside
                //
                // Alternative Option
                // * Create a wrapper class of type VisualElement
                // * Create no wrapper and 'speak to the uxml' directly from UIView
                // * Others options too

                _dialogView = new DialogView();
                _dialogView.SetDialogView(_uiDocument.rootVisualElement.Q<VisualElement>("DialogView"));
                _dialogView.Initialize(Context);
                // ///////////////////////////////////////////////////
                
                // Controller
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
        private void RestartButton_OnClicked()
        {
            OnRestart.Invoke();
        }
        
        
        private void UpdateScoreText()
        {
            if (ScoreLabel != null)
            {
                ScoreLabel.text = $"Score: {_score}/{_scoreMax}";
            }
        }
        
        
        //  Event Handlers --------------------------------
        private void OnScoreChangedCommand(ScoreChangedCommand scoreChangedCommand)
        {
            RequireIsInitialized();

            if (ScoreLabel != null)
            {
                _score = scoreChangedCommand.Value;
                UpdateScoreText();
            }
        }

        
        private void OnScoreMaxChangedCommand(ScoreMaxChangedCommand scoreMaxChangedCommand)
        {
            RequireIsInitialized();

            if (ScoreLabel != null)
            {
                _scoreMax = scoreMaxChangedCommand.Value;
                UpdateScoreText();
            }
        }
        
        private void OnStatusChangedCommand(StatusChangedCommand statusChangedCommand)
        {
            RequireIsInitialized();

            if (StatusLabel != null)
            {
                StatusLabel.text = $"{statusChangedCommand.Value}";
            }
        }

    }
}