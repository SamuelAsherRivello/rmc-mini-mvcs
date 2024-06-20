using System;
using RMC.Mini.View;
using RMC.Mini.Samples.Configurator.Mini.Controller;
using RMC.Mini.Samples.Configurator.Mini.Model;
using RMC.Mini.Samples.Configurator.Mini.Model.Data;
using RMC.Mini.Samples.Configurator.Standard.Gameplay;
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
    /// Relates to the <see cref="DeveloperConsoleController"/>
    /// 
    /// </summary>
    public class DeveloperConsoleView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnReset = new UnityEvent();
        
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        public Button ResetButton { get { return _uiDocument?.rootVisualElement.Q<Button>("ResetButton"); }}
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

        [SerializeField] 
        private Player _player;

        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                ResetButton.clicked += ResetButtonOnClicked;
                
                ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
                model.CharacterData.OnValueChanged.AddListener(CharacterData_OnValueChanged);
                model.EnvironmentData.OnValueChanged.AddListener(EnvironmentData_OnValueChanged);
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
            // Optional: Handle any cleanup here...
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
            ResetButton.SetEnabled(model.HasLoadedService.Value && 
                                   (!model.CharacterDataIsDefault() || !model.EnvironmentDataIsDefault()));
            
            StatusLabel.text = $"Reset The\nColors";
        }
        
        
        //  Event Handlers --------------------------------
        private void CharacterData_OnValueChanged(CharacterData previousValue, CharacterData currentValue)
        {
            RequireIsInitialized();
            RefreshUI();
            
            _player.CharacterData = currentValue;
        }
        
        private void EnvironmentData_OnValueChanged(EnvironmentData previousValue, EnvironmentData currentValue)
        {
            RequireIsInitialized();
            RefreshUI();
            _environment.EnvironmentData = currentValue;
        }
        
        private void ResetButtonOnClicked()
        {
            OnReset.Invoke();
            RefreshUI();
        }
    }
}