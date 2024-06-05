using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Data.Types.Storage;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data;
using RMC.MiniMvcs.Samples.Configurator.Mini.Service.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.MiniMvcs.Samples.Configurator.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class ConfiguratorServiceUnityEvent : UnityEvent<ConfiguratorServiceData> {}

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class ConfiguratorService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly ConfiguratorServiceUnityEvent OnLoadCompleted = new ConfiguratorServiceUnityEvent();
        
        
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        private ConfiguratorServiceData _configuratorServiceData;
        
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                bool hasData = LocalDiskStorage.Instance.Has<ConfiguratorServiceData>();
                if (!hasData)
                {
                    var defaultData = new ConfiguratorServiceData();
                    LocalDiskStorage.Instance.Save<ConfiguratorServiceData>(defaultData);
                }
            }
        }

        
        //  Methods ---------------------------------------
        public void Load ()
        {
            RequireIsInitialized();
            
            bool hasData = LocalDiskStorage.Instance.Has<ConfiguratorServiceData>();

            if (!hasData)
            {
                Debug.LogError("Error: LoadScore failed.");
                return;
            }
            
            _configuratorServiceData =  LocalDiskStorage.Instance.Load<ConfiguratorServiceData>();

            OnLoadCompleted.Invoke(_configuratorServiceData);
        }
        
        
        public void SaveEnvironmentData (EnvironmentData environmentData)
        {
            RequireIsInitialized();
            
            bool hasData = LocalDiskStorage.Instance.Has<ConfiguratorServiceData>();

            if (!hasData)
            {
                Debug.LogError("Error: SaveCharacterData failed.");
                return;
            }
            
            _configuratorServiceData = LocalDiskStorage.Instance.Load<ConfiguratorServiceData>();
            _configuratorServiceData.EnvironmentData = environmentData;
            LocalDiskStorage.Instance.Save<ConfiguratorServiceData>(_configuratorServiceData);

            OnLoadCompleted.Invoke(_configuratorServiceData);
        }
        
        
        public void SaveCharacterData (CharacterData characterData)
        {
            RequireIsInitialized();
            
            bool hasData = LocalDiskStorage.Instance.Has<ConfiguratorServiceData>();

            if (!hasData)
            {
                Debug.LogError("Error: SaveCharacterData failed.");
                return;
            }
            
            _configuratorServiceData = LocalDiskStorage.Instance.Load<ConfiguratorServiceData>();
            _configuratorServiceData.CharacterData = characterData;
            LocalDiskStorage.Instance.Save<ConfiguratorServiceData>(_configuratorServiceData);

            OnLoadCompleted.Invoke(_configuratorServiceData);
        }
        
        
        //  Event Handlers --------------------------------
    }
}