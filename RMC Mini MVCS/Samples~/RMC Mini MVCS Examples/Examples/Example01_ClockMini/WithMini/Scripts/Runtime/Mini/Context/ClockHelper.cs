using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// This utility class hides the complexity of running
    /// repeated code without using a MonoBehaviour.
    /// </summary>
    public static class ClockHelper 
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------

        
        //  Methods ---------------------------------------
        public static async void StartTicking(Func<Task> fun)
        {
            // Here is a timer. I purposefully did not use any
            // MonoBehaviour, Coroutines, or 3rd party libraries
            
            // ConfigureAwait must be true to get unity main thread context
            var tcs = new TaskCompletionSource<bool>();
            await Task.Run(() => { tcs.SetResult(true);});
            tcs.Task.ConfigureAwait(true).GetAwaiter().OnCompleted(async() =>
            {
                if (Application.isPlaying)
                {
                    //Call
                    await fun.Invoke();
                    
                    //Repeat
                    StartTicking(fun);
                }
            });
        }

        //  Event Handlers --------------------------------
    }
}