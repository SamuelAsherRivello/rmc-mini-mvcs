using UnityEngine;

namespace RMC.IntroToUnity.Samples.MyMathSystem
{
    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class MyMathSystemExample: MonoBehaviour
    {
        protected void Awake ()
        {
            MyMathSystem myMathSystem = new MyMathSystem();

            int a = 5;
            int b = 10;
            int result = myMathSystem.Add(a, b);
            
            Debug.Log($"This Scene has no UI. It has only console logging.");
            Debug.Log($"Result = {result}");
        }

    }
}