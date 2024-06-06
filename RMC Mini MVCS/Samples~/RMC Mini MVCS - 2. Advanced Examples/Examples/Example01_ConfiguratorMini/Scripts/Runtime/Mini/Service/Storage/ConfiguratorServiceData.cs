using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model.Data;
using RMC.Core.Components.Attributes;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Service.Storage
{
    /// <summary>
    /// Represents a file entry for local disk storage
    /// </summary>
    [CustomFilePath("ConfiguratorServiceData", CustomFilePathLocation.StreamingAssetsPath)]
    public class ConfiguratorServiceData
    {
        //  Fields -----------------------------------------
        [SerializeField] public EnvironmentData EnvironmentData = Model.Data.EnvironmentData.FromDefaultValues();
        
        [SerializeField] 
        public CharacterData CharacterData = Model.Data.CharacterData.FromDefaultValues();
    }
}
