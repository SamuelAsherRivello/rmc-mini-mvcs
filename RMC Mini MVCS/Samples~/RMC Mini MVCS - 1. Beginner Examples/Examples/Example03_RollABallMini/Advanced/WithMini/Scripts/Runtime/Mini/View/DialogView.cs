using System;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------
    
    //  Class Attributes ----------------------------------

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class DialogView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnCancel = new UnityEvent();
        
        [HideInInspector] 
        public readonly UnityEvent OnConfirm = new UnityEvent();
    
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        public Label BodyLabel { get { return _bodyLabel;} }
        public Button CancelButton { get { return _cancelButton;} }
        public Button ConfirmButton { get { return _confirmButton;} }
        
        public bool IsVisible
        {
            get
            {
                return _dialogView.style.visibility.value == Visibility.Visible;
            }
            set
            {
                if (value)
                {
                    _dialogView.style.visibility = Visibility.Visible;
                }
                else
                {
                    _dialogView.style.visibility = Visibility.Hidden;
                }
             
            }
        }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;
        
        // Passed in
        private VisualElement _dialogView;
        
        // Queried
        private Label _bodyLabel;
        private Button _cancelButton;
        private Button _confirmButton;
        
        
        //  Initialization  -------------------------------
        public void SetDialogView (VisualElement dialogView)
        {
            _dialogView = dialogView;
            _bodyLabel = dialogView.Q<Label>("BodyLabel");
            _cancelButton = dialogView.Q<Button>("CancelButton");
            _confirmButton = dialogView.Q<Button>("ConfirmButton");
        }
        
        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                _cancelButton.clicked += CancelButton_OnClicked;
                _confirmButton.clicked += ConfirmButton_OnClicked;
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