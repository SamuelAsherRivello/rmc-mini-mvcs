using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class LeftController: IController
    {
    
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        
        //  Fields ----------------------------------------
        private readonly LeftView _leftView;
        private readonly SpawnerMiniModel _spawnerMiniModel;
        private readonly RightView _rightViewPrefab;
        RightView _spawnedRightView;
        RightController _spawnedRightController;
        
        //  Initialization  -------------------------------
        public LeftController(SpawnerMiniModel spawnerMiniModel, 
            LeftView leftView, RightView rightViewPrefab)
        {
            _spawnerMiniModel = spawnerMiniModel;
            _leftView = leftView;
            _rightViewPrefab = rightViewPrefab;
        }
        
        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                //View
                _leftView.OnIncrementCounter.AddListener(LeftView_OnIncrementCounter);
                _leftView.OnSpawnOrDestroy.AddListener(LeftView_OnSpawnOrDestroy);
                RefreshUI();
            }
        }
        

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            RequireIsInitialized();
            if (_spawnerMiniModel.IsSpawnMode.Value)
            {
                _leftView.SpawnToggleButtonText.text = "Spawn View";
            }
            else
            {
                _leftView.SpawnToggleButtonText.text = "Destroy View";
            }
        }


        //  Event Handlers --------------------------------
        private void LeftView_OnSpawnOrDestroy()
        {
            RequireIsInitialized();
  
            if (_spawnerMiniModel.IsSpawnMode.Value)
            {
                // Spawn Right 
                _spawnedRightView = GameObject.Instantiate(_rightViewPrefab).GetComponent<RightView>();
                _spawnedRightView.Initialize(Context);
                _spawnedRightController = new RightController(_spawnerMiniModel, _spawnedRightView);
                _spawnedRightController.Initialize(Context);
            }
            else
            {
                // Destroy Right 
                _spawnedRightController.Dispose();
                GameObject.Destroy(_spawnedRightView.gameObject);
            }
            
            _spawnerMiniModel.IsSpawnMode.Value = !_spawnerMiniModel.IsSpawnMode.Value;
            RefreshUI();
          
        }
        
        private void LeftView_OnIncrementCounter()
        {
            RequireIsInitialized();
            _spawnerMiniModel.Counter.Value++;
        }
        
    }
}