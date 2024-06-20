using UnityEngine;
namespace RMC.Mini.Samples.Clock.WithMini.Mini
{
    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class ClockWithMiniExample : MonoBehaviour
    {
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            
            Debug.Log($"This Scene has no UI. It has only console logging.");
            
            ClockSimpleMini clockSimpleMini = new ClockSimpleMini();
            clockSimpleMini.Initialize();
        }
    }
}