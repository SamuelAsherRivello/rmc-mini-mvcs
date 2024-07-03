using RMC.Core.Observables;
using UnityEngine;

namespace RMC.Mini.Lessons.Observables
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

            string message = "Hello World!";
            Debug.Log("Status.Value = " + message); 
            Status.Value = message;
        }

        private void Status_OnValueChanged(string previousValue, string currentValue)
        {
            Debug.Log("Status_OnValueChanged() currentValue = " + currentValue); //Hello World!
        }
        
    }
}