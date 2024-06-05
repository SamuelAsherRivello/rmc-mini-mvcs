using RMC.Core.Components.Attributes;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Service.Storage
{
    [CustomFilePath("ComplexLocalDiskStorage", CustomFilePathLocation.StreamingAssetsPath)]
    public class ComplexLocalDiskStorageData 
    {
        [SerializeField] 
        public int Score = 0;
    }
}
