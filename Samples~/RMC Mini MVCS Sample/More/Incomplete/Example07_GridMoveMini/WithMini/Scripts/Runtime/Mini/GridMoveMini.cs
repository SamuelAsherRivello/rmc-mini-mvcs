using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This Command is a stand-alone object containing
    /// a value of data.
    /// </summary>
    public class GridMoveMini : IMiniMvcs
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public Context.Context Context { get { return _context;} }
        public GridMoveModel GridMoveModel { get { return _gridMoveModel;} }
        public GridView GridView { get { return _gridView;} }
        public PlayerView PlayerView { get { return _playerView;} }
        public UIView UIView { get { return _uiView;} }
        public GridMoveController GridMoveController { get { return _gridMoveController;} }
        public AudioController AudioController { get { return _audioController;} }
        
        //  Fields ----------------------------------------
 
        private bool _isInitialized = false;
        
        //Context
        private Context.Context _context;
        
        //Model
        private GridMoveModel _gridMoveModel;
        
        //View
        private GridView _gridView;
        private PlayerView _playerView;
        private UIView _uiView;
        
        //Controller
        private GridMoveController _gridMoveController;
        private AudioController _audioController;


        //  Initialization  -------------------------------
        public GridMoveMini(GridView gridView, PlayerView playerView, UIView uiView)
        {
            _gridView = gridView;
            _playerView = playerView;
            _uiView = uiView;
        }
        
        
        //  Initialization --------------------------------
        public void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                //Context
                _context = new Context.Context();
                
                //Model
                _gridMoveModel = new GridMoveModel();
   
                //Controller
                _gridMoveController = new GridMoveController(
                    _gridMoveModel, 
                    _gridView, 
                    _playerView, 
                    _uiView);
            
                //Model
                _gridMoveModel.Initialize(_context);
                
                //View
                _gridView.Initialize(_context);
                _playerView.Initialize(_context);
                _uiView.Initialize(_context);
                
                //Demo - Mini supports arbitrary actors (e.g. a second Controller)
                _audioController = new AudioController();
                _audioController.Initialize(_context);
                
                //Controller (Init this main controller last)
                _gridMoveController.Initialize(_context);
                _gridMoveModel.GridCellIndex.Value = 1;
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
    }
}