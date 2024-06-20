using System;
using System.Threading.Tasks;
using RMC.Mini.Service;
using Unity.Services.Analytics;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Mini.Samples.UGS.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class AnalyticsServiceUnityEvent : UnityEvent<AnalyticsService> {}

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class AnalyticsService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        //public readonly AuthenticationServiceUnityEvent OnSignInComplete = new AuthenticationServiceUnityEvent();
        
        //  Properties ------------------------------------
        
        /// <summary>
        /// Wrap Unity implementation for convenience
        /// </summary>
        private Unity.Services.Analytics.IAnalyticsService UnityAnalyticsService
        {
            get
            {
                return Unity.Services.Analytics.AnalyticsService.Instance;
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
           
        }
        
        public async Task RecordEvent (BaseAnalyticsEvent customEvent)
        {
            RequireIsInitialized();

            if (!IsInitializedUnityServices)
            {
                await UnityServicesInitializeAsync();
            }
            
            if (customEvent as CustomEvent == null)
            {
                throw new Exception("Ugs customEvent instance must be extend Unity CustomEvent");
            }
            
            try
            {
                //Debug.Log("RecordEvent() Send : " + customEvent.Name);
                UnityAnalyticsService.RecordEvent(customEvent);
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }
        
        public async Task RecordEvent (string eventName)
        {
            RequireIsInitialized();

            if (!IsInitializedUnityServices)
            {
                await UnityServicesInitializeAsync();
            }
            
            try
            {
                UnityAnalyticsService.RecordEvent(eventName);
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
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