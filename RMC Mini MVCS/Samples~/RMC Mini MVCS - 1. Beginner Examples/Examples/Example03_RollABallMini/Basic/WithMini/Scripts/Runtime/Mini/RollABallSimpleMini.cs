using System;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;
using RMC.Core.Architectures.Mini.Structure.Simple;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This Command is a stand-alone object containing
    /// a value of data.
    /// </summary>
    public class RollABallSimpleMini : ISimpleMiniMvcs
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public Context Context { get { return _context;} }
        public RollABallModel RollABallModel { get { return _rollABallModel;} }
        public InputView InputView { get { return _inputView;} }
        public PlayerView PlayerView { get { return _playerView;} }
        public UIView UIView { get { return _uiView;} }
        public RollABallController RollABallController { get { return _rollABallController;} }
        public AudioController AudioController { get { return _audioController;} }
        public RollABallService RollABallService { get { return _rollABallService;} }
        
        //  Fields ----------------------------------------
 
        private bool _isInitialized = false;
        
        //Context
        private Context _context;
        
        //Model
        private RollABallModel _rollABallModel;
        
        //View
        private InputView _inputView;
        private PlayerView _playerView;
        private UIView _uiView;
        
        //Controller
        private RollABallController _rollABallController;
        private AudioController _audioController;
        
        //Service
        private RollABallService _rollABallService;


        //  Initialization  -------------------------------
        public RollABallSimpleMini(InputView inputView, PlayerView playerView, UIView uiView)
        {
            _inputView = inputView;
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
                _context = new Context();
                
                //Model
                _rollABallModel = new RollABallModel();
   
                //Service
                _rollABallService = new RollABallService();
                
                //Controller
                _rollABallController = new RollABallController(
                    _rollABallModel, 
                    _inputView, 
                    _playerView, 
                    _uiView, 
                    _rollABallService);
            
                //Model
                _rollABallModel.Initialize(_context);
                
                //View
                _inputView.Initialize(_context);
                _playerView.Initialize(_context);
                _uiView.Initialize(_context);
                
                //Service
                _rollABallService.Initialize(_context);
                
                //Demo - Mini supports arbitrary actors (e.g. a second Controller)
                _audioController = new AudioController();
                _audioController.Initialize(_context);
                
                //Controller (Init this main controller last)
                _rollABallController.Initialize(_context);
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