using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------
    public class ConfirmUnityEvent : UnityEvent {}
    public class CancelUnityEvent : UnityEvent {}
    
    //  Class Attributes ----------------------------------

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class DialogView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly CancelUnityEvent OnCancel = new CancelUnityEvent();
        
        [HideInInspector] 
        public readonly ConfirmUnityEvent OnConfirm = new ConfirmUnityEvent();
    
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;
        
        [SerializeField] private Text _bodyText;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _confirmButton;
        
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //
                _cancelButton.onClick.AddListener(CancelButton_OnClicked);
                _confirmButton.onClick.AddListener(ConfirmButton_OnClicked);
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
        private void CancelButton_OnClicked()
        {
            OnCancel.Invoke();
        }
        
        private void ConfirmButton_OnClicked()
        {
           OnConfirm.Invoke();
        }
        

    }
}