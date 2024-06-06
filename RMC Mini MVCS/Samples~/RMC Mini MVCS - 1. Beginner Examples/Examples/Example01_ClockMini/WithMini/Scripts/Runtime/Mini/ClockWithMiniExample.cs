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
            
            Debug.Log($"This Scene has no UI. It has only console logging.");
            
            ClockSimpleMini clockSimpleMini = new ClockSimpleMini();
            clockSimpleMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}