using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class GameView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnScorePoints = new UnityEvent();
        
        [HideInInspector] 
        public readonly UnityEvent OnReset = new UnityEvent();
        
        public readonly UnityEvent OnSave = new UnityEvent();

        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private Text _statusText;
        
        [SerializeField] 
        private Button _scoreButton;
        
        [SerializeField] 
        private Button _resetButton;

        [SerializeField] 
        private Button _saveButton;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                ComplexModel model = Context.ModelLocator.GetItem<ComplexModel>();
                model.ServiceHasLoaded.OnValueChanged.AddListener(ServiceHasLoaded_OnValueChanged);
                model.IsServiceDirty.OnValueChanged.AddListener(IsServiceDirty_OnValueChanged);
                model.Score.OnValueChanged.AddListener(Score_OnValueChanged);
                
                _scoreButton.onClick.AddListener(PlayButton_OnClicked);
                _resetButton.onClick.AddListener(ResetButton_OnClicked);
                _saveButton.onClick.AddListener(SaveButton_OnClicked);
                
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

            _scoreButton?.onClick.RemoveListener(PlayButton_OnClicked);
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            ComplexModel model = Context.ModelLocator.GetItem<ComplexModel>();
            _scoreButton.interactable = model.ServiceHasLoaded.Value &&
                                        !model.IsGameOver;
            
            _resetButton.interactable = model.ServiceHasLoaded.Value &&
                                        model.Score.Value != 0;

            _saveButton.interactable = model.ServiceHasLoaded.Value &&
                                       model.IsServiceDirty.Value;
            
            if (!model.IsGameOver)
            {
                _statusText.text = $"Keep Playing! MaxScore: {model.ScoreMax.Value}";
            }
            else
            {
                _statusText.text = $"You Win! MaxScore: {model.ScoreMax.Value}";
            }
        }
        
        
        //  Event Handlers --------------------------------
        private void PlayButton_OnClicked()
        {
            ComplexModel model = Context.ModelLocator.GetItem<ComplexModel>();
            if (!model.IsGameOver)
            {
                OnScorePoints.Invoke();
            }
        }
        
        
        private void ResetButton_OnClicked()
        {
            OnReset.Invoke();
        }
        
        private void SaveButton_OnClicked()
        {
            OnSave.Invoke();
        }
        
        private void ServiceHasLoaded_OnValueChanged(bool previousValue, bool currentValue)
        {
            RequireIsInitialized();

            RefreshUI();
        }
        
        private void IsServiceDirty_OnValueChanged(bool previousValue, bool currentValue)
        {
            RequireIsInitialized();

            RefreshUI();
        }
        
        private void Score_OnValueChanged(int previousValue, int currentValue)
        {
            RequireIsInitialized();

            RefreshUI();
        }
    }
}