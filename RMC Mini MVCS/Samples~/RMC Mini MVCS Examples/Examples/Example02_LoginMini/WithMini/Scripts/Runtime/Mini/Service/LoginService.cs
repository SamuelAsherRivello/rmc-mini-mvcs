using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class OnLoginCompletedUnityEvent : UnityEvent<UserData, bool> {}

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class LoginService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly OnLoginCompletedUnityEvent OnLoginCompleted = new OnLoginCompletedUnityEvent();
        
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
        public void Login (UserData userData)
        {
            RequireIsInitialized();
            
            LoginAsync(userData);
        }

        private async void LoginAsync(UserData userData)
        {
            RequireIsInitialized();

            //For simplicity we check a local file to validate.
            //In production, call a server to validate instead
            TextAsset textAsset = Resources.Load<TextAsset>("Texts/LoginWithMiniText"); //txt file

            //Add cosmetic delay to simulate latency
            await Task.Delay(500);
            
            bool wasSuccess = false;
            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                string correctPassword = textAsset.ToString();
                wasSuccess = userData.Password == correctPassword;
            }
            
            OnLoginCompleted.Invoke(userData, wasSuccess);
        }
        
        
        //  Event Handlers --------------------------------

    }
}