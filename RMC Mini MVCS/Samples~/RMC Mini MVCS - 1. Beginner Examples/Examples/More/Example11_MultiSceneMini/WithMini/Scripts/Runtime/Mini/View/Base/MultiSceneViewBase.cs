using System;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Controller.Events;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// The _view handles user interface and user input
    /// </summary>
    public class MultiSceneViewBase: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly SceneChangeUnityEvent OnRequestSceneChange = new SceneChangeUnityEvent();
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private Text _statusText;

        [SerializeField] 
        private Text _clickCountText;

        [SerializeField] 
        private Button _sceneChangeButton;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                MultiSceneModel model = Context.ModelLocator.GetItem<MultiSceneModel>();
                model.ServiceHasLoaded.OnValueChanged.AddListener(ServiceHasLoaded_OnValueChanged);
                
                _sceneChangeButton.interactable = model.ServiceHasLoaded.Value;
                _sceneChangeButton.onClick.AddListener(SceneChangeButton_OnClicked);
                
                Context.CommandManager.AddCommandListener<SceneNameChangedCommand>(
                    OnSceneNameChangedCommand);

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
        
        
        //  Unity Methods ---------------------------------
        private void SceneChangeButton_OnClicked()
        {
            OnRequestSceneChange.Invoke();
        }
        
        
        protected void OnDestroy()
        {
            MultiSceneModel model = Context.ModelLocator.GetItem<MultiSceneModel>();
            model.ServiceHasLoaded.OnValueChanged.RemoveListener(ServiceHasLoaded_OnValueChanged);

            _sceneChangeButton.onClick.RemoveListener(SceneChangeButton_OnClicked);
            
            Context?.CommandManager?.RemoveCommandListener<SceneNameChangedCommand>(
                OnSceneNameChangedCommand);
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            MultiSceneModel model = Context.ModelLocator.GetItem<MultiSceneModel>();
            _statusText.text = $"Scene: {model.SceneName.Value}";
            
            _clickCountText.text = $"The ClickCount of {model.ClickCount.Value} " +
                                   $"properly persists across scenes.";
            
            _sceneChangeButton.interactable = model.ServiceHasLoaded.Value;
        }
        
        
        //  Event Handlers --------------------------------
        private void ServiceHasLoaded_OnValueChanged(bool previousValue, bool currentValue)
        {
            RequireIsInitialized();

            RefreshUI();
        }
        
        
        private void OnSceneNameChangedCommand(
            SceneNameChangedCommand sceneNameChangedCommand)
        {
            RequireIsInitialized();

            RefreshUI();
            
            // Do not change scene if already in it
            if (sceneNameChangedCommand.CurrentValue ==
                SceneManager.GetActiveScene().name)
            {
                return;
            }
     
            if (_sceneChangeButton != null) _sceneChangeButton.interactable = false;
            AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(sceneNameChangedCommand.CurrentValue);

            if (asyncOperation != null)
            {
                asyncOperation.completed += operation =>
                {
                    //Debug.Log("LoadSceneAsync Completed");
                };
            }
        }
    }
}