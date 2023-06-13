using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Components;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class RollABallController : IController  
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        
        //Context
        private IContext _context;
        
        //Model
        private RollABallModel _model;
        
        //View
        private PlayerView _playerView;
        private UIView _uiView;
        private InputView _inputView;
        
        //Controller
        private AudioController _audioController;
        
        //Service
        private RollABallService _service;
        
        //  Initialization  -------------------------------
        public RollABallController(
            RollABallModel model, 
            InputView inputView, 
            PlayerView playerView, 
            UIView uiView, 
            RollABallService service)
        {
            _model = model;
            _inputView = inputView;
            _playerView = playerView;
            _uiView = uiView;
            _service = service;
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //Model
                _model.Score.OnValueChanged.AddListener(Model_Score_OnValueChanged);
                _model.ScoreMax.OnValueChanged.AddListener(Model_ScoreMax_OnValueChanged);
                _model.StatusText.OnValueChanged.AddListener(Model_StatusText_OnValueChanged);
                _model.StatusText.Value = "Loading...";
                
                //View
                _inputView.OnInput.AddListener(InputView_OnInput);
                _uiView.OnRestart.AddListener(UIView_OnRestart);
                _playerView.OnPickup.AddListener(PlayerView_OnPickup);
                
                //Service
                _service.OnLoaded.AddListener(Service_OnLoaded);
                _service.Load();
            }
        }


        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
        private void UIView_OnRestart(DialogView dialogViewPrefab)
        {
            //Pause
            _model.IsGamePaused.Value = true;
            
            // Show Prompt
            DialogView dialogView = GameObject.Instantiate(dialogViewPrefab);
            dialogView.Initialize(Context);
            
            dialogView.OnCancel.AddListener(() =>
            {
                GameObject.Destroy(dialogView.gameObject);
                
                //Unpause
                _model.IsGamePaused.Value = false;
                
            });
            
            dialogView.OnConfirm.AddListener(() =>
            {
                GameObject.Destroy(dialogView.gameObject);
                SceneManager.LoadScene("RollABallWithMiniExample");
                
                //Unpause
                _model.IsGamePaused.Value = false;
            });
        }
        
        
        private void Service_OnLoaded()
        {
            RequireIsInitialized();

            _model.Score.Value = 0;
            _model.ScoreMax.Value = _service.ScoreMax;
            _model.StatusText.Value = "Use Arrow Keys";
        }
        
        private void InputView_OnInput(Vector3 input)
        {
            RequireIsInitialized();
            
            if (_model.IsGameOver.Value || _model.IsGamePaused.Value)
            {
                return;
            }
            
            Context.CommandManager.InvokeCommand(new InputCommand(input));
        }
        
        public void PlayerView_OnPickup(PickupComponent pickupComponent)
        {
            RequireIsInitialized();

            if (_model.IsGameOver.Value)
            {
                return;
            }
            
            pickupComponent.gameObject.SetActive (false);
            
            Context.CommandManager.InvokeCommand(
                new PlayAudioClipCommand("AudioClips/Bounce01"));
            
            if (++_model.Score.Value >= _model.ScoreMax.Value)
            {
                _model.IsGameOver.Value = true;
                _model.StatusText.Value = "You Win!";
            }
        }
        
        private void Model_Score_OnValueChanged(int previousValue, int currentValue)
        {
            RequireIsInitialized();
            
            Context.CommandManager.InvokeCommand(
                new ScoreChangedCommand(currentValue));
        }
        
        private void Model_ScoreMax_OnValueChanged(int previousValue, int currentValue)
        {
            RequireIsInitialized();
            
            Context.CommandManager.InvokeCommand(
                new ScoreMaxChangedCommand(currentValue));
        }
        
        private void Model_StatusText_OnValueChanged(string previousValue, string currentValue)
        {
            RequireIsInitialized();
            
            Context.CommandManager.InvokeCommand(
                new StatusChangedCommand(currentValue));
        }
    }
}