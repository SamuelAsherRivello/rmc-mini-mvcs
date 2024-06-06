using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.View;
using UnityEngine;
namespace RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class CountUpMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        //Mini does NOT require any MonoBehaviours ...
        //But here is an optional one to ease UI setup ...
        [SerializeField] 
        private CountUpView _view;

        //  Unity Methods  --------------------------------
        protected void Start()
        {
            CountUpSimpleMini countUpSimpleMini = new CountUpSimpleMini(_view);
            countUpSimpleMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}