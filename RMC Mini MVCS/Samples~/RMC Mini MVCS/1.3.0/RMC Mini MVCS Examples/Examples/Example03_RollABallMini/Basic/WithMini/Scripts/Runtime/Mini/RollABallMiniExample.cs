using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class RollABallMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        [SerializeField] 
        private InputView _inputView;

        [SerializeField] 
        private PlayerView _playerView;

        [SerializeField] 
        private UIView _uiView;

        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            RollABallMini rollABallMini = 
                new RollABallMini(_inputView, _playerView, _uiView);
            
            rollABallMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}