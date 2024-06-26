using RMC.Mini.Samples.RollABall.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Mini.Samples.RollABall.WithMini.Mini
{
    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class RollABallMiniExample : MonoBehaviour
    {
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        [SerializeField] 
        private InputView _inputView;

        [SerializeField] 
        private PlayerView _playerView;

        [SerializeField] 
        private PickupsView _pickupsView;

        [SerializeField] 
        private UIView _uiView;


        //  Unity Methods  --------------------------------
        protected void Start()
        {
            RollABallSimpleMini rollABallSimpleMini = 
                new RollABallSimpleMini(_inputView, _playerView, _pickupsView, _uiView);
            
            rollABallSimpleMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}