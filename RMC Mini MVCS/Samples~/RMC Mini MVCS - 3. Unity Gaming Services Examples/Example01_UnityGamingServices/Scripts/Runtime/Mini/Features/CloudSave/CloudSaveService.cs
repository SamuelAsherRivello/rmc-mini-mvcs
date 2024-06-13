using System.Collections.Generic;
using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Service;
using Unity.Services.Authentication;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class LoadCloudSaveUnityEvent : UnityEvent<Dictionary<string, Item>> {}
    public class SaveCloudSaveUnityEvent : UnityEvent<Dictionary<string, string>> {}
    public class DeleteCloudSaveUnityEvent : UnityEvent {}
    
    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class CloudSaveService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public LoadCloudSaveUnityEvent OnLoadAllPlayerDataComplete = new LoadCloudSaveUnityEvent();
        
        [HideInInspector] 
        public SaveCloudSaveUnityEvent OnSaveAPlayerDataComplete = new SaveCloudSaveUnityEvent();
        
        [HideInInspector] 
        public DeleteCloudSaveUnityEvent OnDeleteAllPlayerDataComplete = new DeleteCloudSaveUnityEvent();

        
        //  Properties ------------------------------------
        /// <summary>
        /// Wrap Unity implementation for convenience
        /// </summary>
        private Unity.Services.CloudSave.ICloudSaveService UnityCloudSaveService
        {
            get
            {
                return Unity.Services.CloudSave.CloudSaveService.Instance;
            }
        }
        
        
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
        public async Task LoadAllPlayerDataAsync ()
        {
            RequireIsInitialized();
            
            try
            {
                Dictionary<string, Item> results = 
                    await UnityCloudSaveService.Data.Player.LoadAllAsync();
                OnLoadAllPlayerDataComplete.Invoke(results);
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
        
        public async Task SaveAPlayerDataAsync (Dictionary<string, SaveItem> saveData)
        {
            RequireIsInitialized();
            
            try
            {
                var results = await UnityCloudSaveService.Data.Player.SaveAsync(saveData);
                OnSaveAPlayerDataComplete.Invoke(results);
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

        public async Task DeleteAllPlayerDataAsync ()
        {
            RequireIsInitialized();
            
            try
            {
                await UnityCloudSaveService.Data.Player.DeleteAllAsync();
                OnDeleteAllPlayerDataComplete.Invoke();
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
        
        public void OpenDashboard ()
        {
            RequireIsInitialized();
            
            Application.OpenURL("https://cloud.unity.com/cloud-save");
        }
        
        public void OpenDocumentation ()
        {
            RequireIsInitialized();
            
            Application.OpenURL("https://docs.unity3d.com/Packages/com.unity.services.cloudsave@latest/");
        }
        
        //  Event Handlers --------------------------------
    }
}