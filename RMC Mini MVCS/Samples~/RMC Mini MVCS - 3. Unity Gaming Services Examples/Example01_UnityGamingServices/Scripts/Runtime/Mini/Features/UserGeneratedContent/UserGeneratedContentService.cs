using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RMC.Mini.Service;
using RMC.Mini.Samples.UGS.Mini.Feature;
using Unity.Services.Authentication;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using Unity.Services.Ugc;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Mini.Samples.UGS.Mini.Service
{
    public class ContentsListUnityEvent : UnityEvent<PagedResults<Content>> {}
    public class ContentUnityEvent : UnityEvent<Content> {}
    public class ContentIdUnityEvent : UnityEvent<string> {}
    
    //  Namespace Properties ------------------------------
    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class UserGeneratedContentService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public ContentsListUnityEvent OnGetContentsListComplete = new ContentsListUnityEvent();
        
        [HideInInspector] 
        public ContentUnityEvent OnSaveAPlayerDataComplete = new ContentUnityEvent();
        
        [HideInInspector] 
        public ContentIdUnityEvent OnDeleteContentComplete = new ContentIdUnityEvent();

        
        //  Properties ------------------------------------
        /// <summary>
        /// Wrap Unity implementation for convenience
        /// </summary>
        private Unity.Services.Ugc.IUgcService UnityUgcService
        {
            get
            {
                return Unity.Services.Ugc.UgcService.Instance;
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
        public async Task GetContentsListAsync ()
        {
            RequireIsInitialized();
            
            try
            {
                PagedResults<Content> contents = await UnityUgcService.GetContentsAsync(new GetContentsArgs());
     
                OnGetContentsListComplete.Invoke(contents);
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
        
        public async Task SaveAPlayerDataAsync (Dictionary<string, SaveItem> playerData)
        {
            RequireIsInitialized();
            
            try
            {
                string requiredFolder = "UgcSourceAssets";
                string requiredFolderPath = FindFullPathAnywhereInAssets(requiredFolder);

                if (string.IsNullOrEmpty(requiredFolderPath))
                {
                    throw new Exception($"SaveAPlayerDataAsync() failed. Add a '{requiredFolder}' folder anywhere in /Assets/.");
                }
                
                string contentPath = Path.Combine(Application.dataPath, requiredFolderPath, "MyMod.txt");
                string thumbnailPath = Path.Combine(Application.dataPath, requiredFolderPath, "MyMod.png");
                //
                using var contentStream = File.Open(contentPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var thumbnailStream = File.Open(thumbnailPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                DateTime dateTime = DateTime.Now;
                string name = $"New outfit available ({dateTime:H:mm:ss})";
                string description = $"Great stuff since ({dateTime:MM/dd/yy H:mm:ss})";
                var createContentArgs =  new CreateContentArgs(name, description, contentStream)
                    {
                        IsPublic = true,
                        TagsId = null,
                        Thumbnail = thumbnailStream
                    };

                Content createdContent = await UnityUgcService.CreateContentAsync(createContentArgs);
                OnSaveAPlayerDataComplete.Invoke(createdContent);
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
        
        public static string FindFullPathAnywhereInAssets(string folderName = "UgcSourceAssets")
        {
            return FindFolderRecursive(Application.dataPath, folderName);
        }

        private static string FindFolderRecursive(string currentDir, string folderName)
        {
            foreach (string dir in Directory.GetDirectories(currentDir))
            {
                if (Path.GetFileName(dir).Equals(folderName, System.StringComparison.OrdinalIgnoreCase))
                {
                    return dir;
                }

                string foundPath = FindFolderRecursive(dir, folderName);
                if (!string.IsNullOrEmpty(foundPath))
                {
                    return foundPath;
                }
            }

            return null;
        }
        public async Task DeleteContentAsync(string contentId)
        {
            RequireIsInitialized();
            
            if (string.IsNullOrEmpty(contentId))
            {
                throw new System.ArgumentNullException(nameof(contentId));
            }
            
            try
            {
                await UnityUgcService.DeleteContentAsync(contentId);
                OnDeleteContentComplete.Invoke(contentId);
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
        
        /// <summary>
        /// Only for user in the <see cref="DeveloperConsoleFeature"/>
        /// </summary>
        public async Task DeleteAllContentAsync()
        {
            // Listen to load
            List<string> contentIds = new List<string>();
            OnGetContentsListComplete.AddListener(arg0 =>
            {
                foreach (var x in arg0.Results)
                {
                    contentIds.Add(x.Id);
                }
                
            });
            
            // Load
            await GetContentsListAsync();
            
            // Delete 
            foreach (var contentId in contentIds)
            {
                await DeleteContentAsync(contentId);
            }
        }
        
        
        public void OpenDashboard ()
        {
            RequireIsInitialized();
            
            Application.OpenURL("https://cloud.unity.com/ugc");
        }
        
        public void OpenDocumentation ()
        {
            RequireIsInitialized();
            
            Application.OpenURL("https://docs.unity3d.com/Packages/com.unity.services.ugc@latest/");
        }
        
        //  Event Handlers --------------------------------
  
    }
}