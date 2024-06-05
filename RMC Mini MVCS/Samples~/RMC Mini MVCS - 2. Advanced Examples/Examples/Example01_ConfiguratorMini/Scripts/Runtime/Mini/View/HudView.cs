using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using RMC.MiniMvcs.Samples.Configurator.Mini.Controller;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.MiniMvcs.Samples.Configurator.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="HudController"/>
    /// 
    /// </summary>
    public class HudView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnBack = new UnityEvent();
        
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        public Label StatusLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("StatusLabel"); }}
        public Button BackButton { get { return _uiDocument?.rootVisualElement.Q<Button>("BackButton"); }}

        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
                model.HasLoadedService.OnValueChanged.AddListener(ServiceHasLoaded_OnValueChanged);

                BackButton.clicked += BackButton_OnClicked;
                
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
            ConfiguratorModel model = Context?.ModelLocator.GetItem<ConfiguratorModel>();
            model?.HasLoadedService.OnValueChanged.RemoveListener(ServiceHasLoaded_OnValueChanged);

            // Optional: Handle any cleanup here...
        }

        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
            StatusLabel.text = SceneManager.GetActiveScene().name;
            BackButton.SetEnabled(model.HasLoadedService.Value && model.HasBackNavigation.Value);
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
    }
}