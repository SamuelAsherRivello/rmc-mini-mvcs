using System;
using System.Collections;
using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

#pragma warning disable CS4014
// ReSharper disable PossibleLossOfFraction
namespace RMC.Core.Architectures.Mini.Samples.Clock.WithoutMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class ClockWithoutMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        private readonly Observable<int> _timeDelay = new Observable<int>();
        private readonly Observable<float> _time = new Observable<float>();
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            LoadAsync();
        }

        //  Methods ---------------------------------------
        private async Task LoadAsync()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Texts/ClockWithoutMiniText"); //txt file

            //Add cosmetic delay to simulate latency
            await Task.Delay(500);
            
            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                _timeDelay.Value = Int32.Parse(textAsset.ToString());
                StartTicking();
            }
        }
        
        
        private void StartTicking()
        {
            StartCoroutine(StartTicking_Coroutine());
        }
        
        
        private IEnumerator StartTicking_Coroutine()
        {
            // Round the number for readability
            float time = Mathf.Round(Time.time);
            if (time > 0)
            {
                // Update the time
                _time.Value = time;
                OnTimeChanged(_time.Value);
            }
            
            // Wait a short amount
            yield return new WaitForSeconds(_timeDelay.Value/1000);

            // Repeat
            StartTicking();
        }

        
        //  Event Handlers --------------------------------
        private void OnTimeChanged(float time)
        {
            // For simplicity, the 'rendering' of our view is just a debug log.
            Debug.Log($"Elapsed Time: {time} seconds");
        }
    }
}













