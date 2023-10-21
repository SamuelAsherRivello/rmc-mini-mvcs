using UnityEngine;
namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class ClockWithMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            ClockMini clockMini = new ClockMini();
            clockMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}