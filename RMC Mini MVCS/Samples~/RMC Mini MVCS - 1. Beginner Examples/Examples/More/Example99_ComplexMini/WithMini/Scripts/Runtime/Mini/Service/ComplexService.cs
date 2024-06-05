using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Service.Storage;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Data.Types.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class ComplexServiceUnityEvent : UnityEvent<int> {}

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class ComplexService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly ComplexServiceUnityEvent OnLoadScoreCompleted = new ComplexServiceUnityEvent();
        
        //  Properties ------------------------------------
        public int Score { get { return _complexLocalDiskStorageData.Score;} }
        
        //  Fields ----------------------------------------
        private ComplexLocalDiskStorageData _complexLocalDiskStorageData;
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                bool hasData = LocalDiskStorage.Instance.Has<ComplexLocalDiskStorageData>();
                if (!hasData)
                {
                    var defaultData = new ComplexLocalDiskStorageData();
                    LocalDiskStorage.Instance.Save<ComplexLocalDiskStorageData>(defaultData);
                }
            }
        }

        //  Methods ---------------------------------------
        public void LoadScore ()
        {
            RequireIsInitialized();
            
            bool hasData = LocalDiskStorage.Instance.Has<ComplexLocalDiskStorageData>();

            if (!hasData)
            {
                Debug.LogError("Error: LoadScore failed.");
                return;
            }
            
            _complexLocalDiskStorageData =  LocalDiskStorage.Instance.Load<ComplexLocalDiskStorageData>();
            OnLoadScoreCompleted.Invoke(_complexLocalDiskStorageData.Score);
        }
        
        public void SaveScore (int score)
        {
            RequireIsInitialized();
            
            bool hasData = LocalDiskStorage.Instance.Has<ComplexLocalDiskStorageData>();

            if (!hasData)
            {
                Debug.LogError("Error: SaveScore failed.");
                return;
            }
            
            _complexLocalDiskStorageData = LocalDiskStorage.Instance.Load<ComplexLocalDiskStorageData>();
            _complexLocalDiskStorageData.Score = score;
            LocalDiskStorage.Instance.Save<ComplexLocalDiskStorageData>(_complexLocalDiskStorageData);

            OnLoadScoreCompleted.Invoke(_complexLocalDiskStorageData.Score);
        }
        
        
        //  Event Handlers --------------------------------

    }
}