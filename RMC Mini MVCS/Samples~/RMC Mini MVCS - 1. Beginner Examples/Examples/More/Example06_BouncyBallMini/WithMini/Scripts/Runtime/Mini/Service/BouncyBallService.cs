using System;
using RMC.Core.Architectures.Mini.Service;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class LoadedUnityEvent : UnityEvent {}

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class BouncyBallService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly LoadedUnityEvent OnLoaded = new LoadedUnityEvent();

        //  Properties ------------------------------------
        public int BounceCountMax { get { return _bounceCountMax;} }
        
        //  Fields ----------------------------------------
        private int _bounceCountMax;

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                _bounceCountMax = 0;
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

            TextAsset textAsset = Resources.Load<TextAsset>("Texts/BouncyBallWithMiniText"); //txt file

            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                _bounceCountMax = Int32.Parse(textAsset.ToString());
                OnLoaded.Invoke();
            }
        }
        
        //  Event Handlers --------------------------------

    }
}