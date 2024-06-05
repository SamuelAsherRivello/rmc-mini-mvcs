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
    public class MenuView: MonoBehaviour, IView
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
        private Button _playButton;
        
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
                _playButton.onClick.AddListener(PlayButton_OnClicked);
                
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
        private void PlayButton_OnClicked()
        {
            OnBack.Invoke();
        }
        
        
        protected void OnDestroy()
        {
            ComplexModel model = Context?.ModelLocator.GetItem<ComplexModel>();
            model?.ServiceHasLoaded.OnValueChanged.RemoveListener(ServiceHasLoaded_OnValueChanged);

            _playButton.onClick.RemoveListener(PlayButton_OnClicked);
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
            ComplexModel model = Context.ModelLocator.GetItem<ComplexModel>();
            
            _playButton.interactable = model.ServiceHasLoaded.Value;

            //Demonstrates that data is still alive from the 
            //previous scene. Good!
            string verb = "play";
            if (model.Score.Value > model.ScoreMin.Value )
            {
                verb = "resume";
            }
            _statusText.text = $"Want to {verb} the game?";
        }
        
        
        //  Event Handlers --------------------------------
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
    }
}