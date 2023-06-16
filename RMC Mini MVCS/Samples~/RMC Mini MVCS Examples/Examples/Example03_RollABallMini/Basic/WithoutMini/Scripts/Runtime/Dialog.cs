using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithoutMini
{
    //  Namespace Properties ------------------------------
    public class ConfirmUnityEvent : UnityEvent {}
    public class CancelUnityEvent : UnityEvent {}
    
    //  Class Attributes ----------------------------------

    /// <summary>
    /// The UI render interactive control elements to the user
    /// </summary>
    public class Dialog: MonoBehaviour
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly CancelUnityEvent OnCancel = new CancelUnityEvent();
        
        [HideInInspector] 
        public readonly ConfirmUnityEvent OnConfirm = new ConfirmUnityEvent();
    
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        [SerializeField] private Text _bodyText;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _confirmButton;
        
        //  Unity Methods ---------------------------------
        protected void Start()
        {
            _cancelButton.onClick.AddListener(CancelButton_OnClicked);
            _confirmButton.onClick.AddListener(ConfirmButton_OnClicked);
        }

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