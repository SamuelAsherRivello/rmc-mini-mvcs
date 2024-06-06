using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Service;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class MultiSceneServiceUnityEvent : UnityEvent<string> {}

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class MultiSceneService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly MultiSceneServiceUnityEvent OnLoadCompleted = new MultiSceneServiceUnityEvent();
        
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
            }
        }

        //  Methods ---------------------------------------
        public void Load ()
        {
            RequireIsInitialized();
            
            LoadAsync();
        }

        private async void LoadAsync()
        {
            RequireIsInitialized();

            //For simplicity we check a local file to validate.
            //In production, call a server to validate instead
            TextAsset textAsset = Resources.Load<TextAsset>("Texts/MultiSceneMiniText"); //txt file

            //Add cosmetic delay to simulate latency
            await Task.Delay(500);
            
            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                string message = textAsset.ToString();
                OnLoadCompleted.Invoke(message);
            }
        }
        
        
        //  Event Handlers --------------------------------

    }
}