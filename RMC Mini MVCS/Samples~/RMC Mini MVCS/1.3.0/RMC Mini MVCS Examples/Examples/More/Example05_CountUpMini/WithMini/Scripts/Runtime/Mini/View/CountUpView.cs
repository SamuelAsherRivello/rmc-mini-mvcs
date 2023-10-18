using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class ScreenClickedUnityEvent : UnityEvent {}

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class CountUpView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector]
        public readonly ScreenClickedUnityEvent OnScreenClicked = new ScreenClickedUnityEvent();

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        public Text TitleText { get { return _titleText;} }
        public Text StatusText { get { return _statusText;} }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private Text _titleText;

        [SerializeField] 
        private Text _statusText;

        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //
                Context.CommandManager.AddCommandListener<CounterChangedCommand>(
                    OnCounterValueChangedCommand);

                // Demo - Optional: View may READ model DIRECTLY...
                var x = Context.ModelLocator.GetItem<CountUpModel>();
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
        protected void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RequireIsInitialized();
                
                OnScreenClicked.Invoke();
            }
        }
        
        protected void OnDestroy()
        {
            Context.CommandManager.RemoveCommandListener<CounterChangedCommand>(
                OnCounterValueChangedCommand);
        }

        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        private void OnCounterValueChangedCommand(CounterChangedCommand counterChangedCommand)
        {
            RequireIsInitialized();

            _statusText.text = $"Counter: {counterChangedCommand.CurrentValue}";
        }

    }
}