using System;
using RMC.Mini.View;
using RMC.Mini.Samples.Configurator.Mini.Controller;
using RMC.Mini.Samples.Configurator.Mini.Model;
using RMC.Mini.Samples.Configurator.Mini.Model.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Gameplay_Environment = RMC.Mini.Samples.Configurator.Standard.Gameplay.Environment;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Mini.Samples.Configurator.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="CustomizeEnvironmentController"/>
    /// 
    /// </summary>
    public class CustomizeEnvironmentView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnRandomize = new UnityEvent();
        
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        public Button RandomizeButton { get { return _uiDocument?.rootVisualElement.Q<Button>("RandomizeButton"); }}
        
        public Label StatusLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("StatusLabel"); }}
        
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        [Header("Gameplay")]
        [SerializeField] 
        private Gameplay_Environment _environment;
        
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
                model.EnvironmentData.OnValueChanged.AddListener(EnvironmentData_OnValueChanged);
                RandomizeButton.clicked += RandomizeButton_OnClicked;
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
            model?.EnvironmentData.OnValueChanged.RemoveListener(EnvironmentData_OnValueChanged);

            // Optional: Handle any cleanup here...
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
            RandomizeButton.SetEnabled(model.HasLoadedService.Value);
            StatusLabel.text = $"Set The\nEnvironment\nColors";
        }
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void RandomizeButton_OnClicked()
        {
            OnRandomize.Invoke();
        }
        
        private void EnvironmentData_OnValueChanged(EnvironmentData previousValue, EnvironmentData currentValue)
        {
            RequireIsInitialized();
            RefreshUI();
            _environment.EnvironmentData = currentValue;
        }
    }
}