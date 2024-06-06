using System;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model.Data;
using RMC.Core.Architectures.Mini.Samples.Configurator.Standard.Gameplay;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="CustomizeCharacterController"/>
    /// 
    /// </summary>
    public class CustomizeCharacterView: MonoBehaviour, IView
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
        private Player _player;
        
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
                model.CharacterData.OnValueChanged.AddListener(CharacterData_OnValueChanged);
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
            model?.CharacterData.OnValueChanged.RemoveListener(CharacterData_OnValueChanged);
            
            // Optional: Handle any cleanup here...
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            ConfiguratorModel model = Context.ModelLocator.GetItem<ConfiguratorModel>();
            RandomizeButton.SetEnabled(model.HasLoadedService.Value);
            StatusLabel.text = $"Set The\nCharacter\nColors";
        }
        
        
        //  Event Handlers --------------------------------
        private void RandomizeButton_OnClicked()
        {
            OnRandomize.Invoke();
        }
        
        private void CharacterData_OnValueChanged(CharacterData previousValue, CharacterData currentValue)
        {
            RequireIsInitialized();
            RefreshUI();
            _player.CharacterData = currentValue;
        }
    }
}