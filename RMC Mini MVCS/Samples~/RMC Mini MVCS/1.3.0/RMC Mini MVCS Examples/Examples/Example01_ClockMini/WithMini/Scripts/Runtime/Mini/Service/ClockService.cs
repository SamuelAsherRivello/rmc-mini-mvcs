using System;
using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Service;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable CS4014
namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class LoadedUnityEvent : UnityEvent {}

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class ClockService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly LoadedUnityEvent OnLoaded = new LoadedUnityEvent();

        //  Properties ------------------------------------
        public int TimeDelay { get { return _timeDelay;} }
        
        //  Fields ----------------------------------------
        private int _timeDelay;

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                _timeDelay = 0;
            }
        }

        //  Methods ---------------------------------------
        public void Load ()
        {
            RequireIsInitialized();

            LoadAsync();
        }

        private async Task LoadAsync()
        {
            RequireIsInitialized();

            TextAsset textAsset = Resources.Load<TextAsset>("Texts/ClockWithMiniText"); //txt file

            //Add cosmetic delay to simulate latency
            await Task.Delay(500);
            
            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                _timeDelay = Int32.Parse(textAsset.ToString());
                OnLoaded.Invoke();
            }
        }
        
        //  Event Handlers --------------------------------
    }
}