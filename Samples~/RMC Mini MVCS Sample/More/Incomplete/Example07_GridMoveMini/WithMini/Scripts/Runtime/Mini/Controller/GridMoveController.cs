using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class GridMoveController : IController  
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
        private GridMoveModel _model;
        
        //View
        private PlayerView _playerView;
        private UIView _uiView;
        private GridView _gridView;
        
        //Controller
        private AudioController _audioController;
        
        //  Initialization  -------------------------------
        public GridMoveController(
            GridMoveModel model, 
            GridView gridView, 
            PlayerView playerView, 
            UIView uiView)
        {
            _model = model;
            _gridView = gridView;
            _playerView = playerView;
            _uiView = uiView;
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //Model
                _model.GridCellIndex.OnValueChanged.AddListener(Model_GridCellIndex_OnValueChanged);
                _model.StatusText.OnValueChanged.AddListener(Model_StatusText_OnValueChanged);
                
                //View
                _gridView.OnCellClick.AddListener(GridView_OnCellClicked);
                _playerView.OnMoveComplete.AddListener(PlayerView_OnMoveCompleted);
                
                //
                _model.StatusText.Value = "Click Grid Cell To Move";
                
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
        private void GridView_OnCellClicked(int gridCellIndex)
        {
            RequireIsInitialized();

            // DEMONSTRATES 'FUZZY' STATE - When we want to set a value immediately
            // but the view 'takes some time' to render it (e.g. w/ animation)
            // Here, if the player is still
            // moving, then don't allow a new value yet
            if (!_model.GridCellIndexIsPending.Value)
            {
                _model.GridCellIndexIsPending.Value = true;
                _model.GridCellIndex.Value = gridCellIndex;
            }
            
            //TODO: Consider 'wrapping' concepts of...
            // 1. 'value' and 'pending'
            // 2. or 'value' and 'isLocked'
            //into one structure (e.g. Observable<> or LockObservable<>) to 
            //ease use cases when state is fuzzy.

        }

   
        
            
        private void Model_StatusText_OnValueChanged(string previousValue, string currentValue)
        {
            RequireIsInitialized();
            
            // Context.CommandManager.InvokeCommand(
            //     new PlayAudioClipCommand("AudioClips/Bounce01"));
            
        }
        
        private void Model_GridCellIndex_OnValueChanged(int previousValue, int currentValue)
        {
            //
            _model.StatusText.Value = $"Cell #{currentValue}...";
            Vector3 nextPosition = _gridView.GetCellUIPositionByIndex(currentValue);
            _playerView.Move(nextPosition);
        }
        
        private void PlayerView_OnMoveCompleted()
        {
            _model.GridCellIndexIsPending.Value = false;
            _model.StatusText.Value = $"Cell #{_model.GridCellIndex.Value}!";
        }
    }
}