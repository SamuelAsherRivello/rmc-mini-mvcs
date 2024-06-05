using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class HudView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnBack = new UnityEvent();
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private Text _statusText;

        [SerializeField] 
        private Text _scoreText;

        [SerializeField] 
        private Button _backButton;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                ComplexModel model = Context.ModelLocator.GetItem<ComplexModel>();
                model.ServiceHasLoaded.OnValueChanged.AddListener(ServiceHasLoaded_OnValueChanged);
                model.Score.OnValueChanged.AddListener(Score_OnValueChanged);
  
                _backButton.interactable = model.ServiceHasLoaded.Value;
                _backButton.onClick.AddListener(BackButton_OnClicked);
                
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
        protected void OnDestroy()
        {
            ComplexModel model = Context?.ModelLocator.GetItem<ComplexModel>();
            model?.ServiceHasLoaded.OnValueChanged.RemoveListener(ServiceHasLoaded_OnValueChanged);

            _backButton.onClick.RemoveListener(BackButton_OnClicked);
            
            Context?.CommandManager?.RemoveCommandListener<SceneNameChangedCommand>(
                OnSceneNameChangedCommand);
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            ComplexModel model = Context.ModelLocator.GetItem<ComplexModel>();
            
            _backButton.interactable = model.ServiceHasLoaded.Value && model.GameMode.Value == GameMode.Game;
            _statusText.text = $"Scene: {model.SceneName.Value}";
            _scoreText.text = $"Score: {model.Score.Value}";
        }
        
        //  Event Handlers --------------------------------
        
        private void BackButton_OnClicked()
        {
            OnBack.Invoke();
        }

        private void ServiceHasLoaded_OnValueChanged(bool previousValue, bool currentValue)
        {
            RequireIsInitialized();
            RefreshUI();
   
        }
        
        private void Score_OnValueChanged(int previousValue, int currentValue)
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
     
            if (_backButton != null) _backButton.interactable = false;
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