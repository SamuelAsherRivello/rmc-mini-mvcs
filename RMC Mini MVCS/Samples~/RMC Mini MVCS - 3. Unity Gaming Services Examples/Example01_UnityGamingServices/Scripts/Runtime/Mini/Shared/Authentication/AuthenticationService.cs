using System;
using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Service;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class AuthenticationServiceUnityEvent : UnityEvent<AuthenticationService> {}

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class AuthenticationService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly AuthenticationServiceUnityEvent OnSignInComplete = new AuthenticationServiceUnityEvent();
        public readonly AuthenticationServiceUnityEvent OnSignInFailed = new AuthenticationServiceUnityEvent();
        public readonly AuthenticationServiceUnityEvent OnSignOutComplete = new AuthenticationServiceUnityEvent();
        public readonly AuthenticationServiceUnityEvent OnExpire = new AuthenticationServiceUnityEvent();
        
        //  Properties ------------------------------------
        
        /// <summary>
        /// Wrap Unity implementation for convenience
        /// </summary>
        private Unity.Services.Authentication.IAuthenticationService UnityAuthenticationService
        {
            get
            {
                return Unity.Services.Authentication.AuthenticationService.Instance;
            }
        }
        
        
        //wrap with getter 
        public PlayerInfo PlayerInfo
        {
            get
            {
                if (!IsSignedIn)
                {
                    throw new Exception("PlayerInfo() failed. Must call IsSignedIn first.");
                }
                return UnityAuthenticationService.PlayerInfo;
            }
        }
        
        public bool IsSignedIn
        {
            get
            {
                return UnityAuthenticationService.IsSignedIn;
            }
        }

        public bool IsInitializedUnityServices { get; private set; } = false;
        
        //  Fields ----------------------------------------
        
        
        //  Initialization  -------------------------------
        public override async void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                IsInitializedUnityServices = false;
                await UnityServicesInitializeAsync();
           
            }
        }


        //  Methods ---------------------------------------
        private async Task UnityServicesInitializeAsync()
        {
            if (IsInitializedUnityServices)
            {
                throw new Exception("Already initialized. Check IsInitializedUnityServices first.");
            }
            
            try
            {
                await UnityServices.InitializeAsync();
                IsInitializedUnityServices = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            UnityAuthenticationService.SignedIn += () => 
            {
                OnSignInComplete.Invoke(this);
            };

            UnityAuthenticationService.SignInFailed += (err) => 
            {
                Debug.LogError(err);
                OnSignInFailed.Invoke(this);
            };

            UnityAuthenticationService.SignedOut += () => {
                OnSignOutComplete.Invoke(this);
            };

            UnityAuthenticationService.Expired += () =>
            {
                OnExpire.Invoke(this);
            };
        }
        
        public async Task SignInAsync ()
        {
            RequireIsInitialized();

            if (!IsInitializedUnityServices)
            {
                await UnityServicesInitializeAsync();
            }
            
            try
            {
                Debug.Log("SignInAnonymouslyAsync");
                await UnityAuthenticationService.SignInAnonymouslyAsync();
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }
        
        public void SignOutAsync ()
        {
            RequireIsInitialized();

            if (!UnityAuthenticationService.IsSignedIn)
            {
                return;
            }
            UnityAuthenticationService.SignOut();
        }
        
        public void OpenDashboard ()
        {
            RequireIsInitialized();
            
            Application.OpenURL("https://cloud.unity.com/player-authentication");
        }
        
        public void OpenDocumentation ()
        {
            RequireIsInitialized();
            
            Application.OpenURL("https://docs.unity3d.com/Packages/com.unity.services.authentication@latest/");
        }
        
        //  Event Handlers --------------------------------
    }
}