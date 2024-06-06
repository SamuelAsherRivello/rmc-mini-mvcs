using RMC.Core.Data.Types;
using UnityEngine;

namespace RMC.IntroToUnity.Examples
{
    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class ObservableExample: MonoBehaviour
    {
        public Observable<string> Status = new Observable<string>();
        
        protected void Awake () 
        {
            Status.OnValueChanged.AddListener(Status_OnValueChanged);

            Debug.Log($"This Scene has no UI. It has only console logging.");
            Status.Value = "Hello World!";
        }

        private void Status_OnValueChanged(string previousValue, string currentValue)
        {
            Debug.Log(currentValue); //Hello World!
        }
        
    }
}