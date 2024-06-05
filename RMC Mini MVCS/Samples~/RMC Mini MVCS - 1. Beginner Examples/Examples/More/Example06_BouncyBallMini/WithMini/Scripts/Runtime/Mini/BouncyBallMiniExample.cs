using RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class BouncyBallMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        //Mini does NOT require any MonoBehaviours ...
        //But here is an optional one to ease UI setup ...
        [SerializeField] 
        private BouncyBallView _view;

        //  Unity Methods  --------------------------------
        protected void Start()
        {
            BouncyBallMini bouncyBallMini = new BouncyBallMini(_view);
            bouncyBallMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}