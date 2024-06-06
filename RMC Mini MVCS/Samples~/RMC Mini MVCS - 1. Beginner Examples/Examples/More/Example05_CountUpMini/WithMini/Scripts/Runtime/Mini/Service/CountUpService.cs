using System;
using RMC.Core.Architectures.Mini.Service;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class LoadedUnityEvent : UnityEvent {}

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class CountUpService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly LoadedUnityEvent OnLoaded = new LoadedUnityEvent();

        //  Properties ------------------------------------
        public int CounterInitialValue { get { return _counterInitialValue;} }
        
        //  Fields ----------------------------------------
        private int _counterInitialValue;

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                _counterInitialValue = 0;
            }
        }

        //  Methods ---------------------------------------
        public void Load ()
        {
            RequireIsInitialized();
            
            LoadAsync();
        }

        private void LoadAsync()
        {
            RequireIsInitialized();

            TextAsset textAsset = Resources.Load<TextAsset>("Texts/CountUpWithMiniText"); //txt file

            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                _counterInitialValue = Int32.Parse(textAsset.ToString());
                OnLoaded.Invoke();
            }
        }
        
        
        //  Event Handlers --------------------------------

    }
}