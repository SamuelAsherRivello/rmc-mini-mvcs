using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class GridMoveExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        [SerializeField] 
        private GridView _gridView;

        [SerializeField] 
        private PlayerView _playerView;

        [SerializeField] 
        private UIView _uiView;

        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            GridMoveMini gridMoveMini = 
                new GridMoveMini(_gridView, _playerView, _uiView);
            
            gridMoveMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}