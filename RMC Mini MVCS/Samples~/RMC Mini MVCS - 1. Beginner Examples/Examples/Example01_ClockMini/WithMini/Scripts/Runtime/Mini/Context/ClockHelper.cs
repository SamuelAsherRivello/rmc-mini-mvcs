using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RMC.Mini.Samples.Clock.WithMini.Mini
{
    /// <summary>
    /// This utility class hides the complexity of running
    /// repeated code without using a UnityEngine namespace.
    /// </summary>
    public static class ClockHelper 
    {
        //  Methods ---------------------------------------
        public static async void StartTicking(Func<Task> fun)
        {
            // Here is a timer. I purposefully did not use any
            // MonoBehaviour, Coroutines, or 3rd party libraries
            // ConfigureAwait must be true to get the main thread context
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
    }
}