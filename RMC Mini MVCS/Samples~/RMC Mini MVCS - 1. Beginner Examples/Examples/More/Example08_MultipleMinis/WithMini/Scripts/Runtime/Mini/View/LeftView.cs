using System;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class LeftView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnIncrementCounter = new UnityEvent();
        
        
        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        
        //  Fields ----------------------------------------

        [SerializeField] 
        private Button _incrementCounterButton;

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                //
                _incrementCounterButton?.onClick.AddListener(
                    IncrementCounterButton_OnClicked);
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

        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        private void IncrementCounterButton_OnClicked()
        {
            OnIncrementCounter.Invoke();
        }
    }
}