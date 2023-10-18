
using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.Calculator.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.Calculator.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class CalculatorView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        public UnityEvent OnAdd = new UnityEvent();
        public UnityEvent OnReset = new UnityEvent();
    
        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        public InputField AInputField { get { return _aInputField;} }
        public InputField BInputField { get { return _bInputField;} }
        public InputField ResultInputField { get { return _resultInputField;} }
        public Button AddButton { get { return _addButton;} }
        public Button ResetButton { get { return _resetButton;} }
        
        
        //  Fields ----------------------------------------
        [SerializeField] private InputField _aInputField;
        [SerializeField] private InputField _bInputField;
        [SerializeField] private InputField _resultInputField;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _resetButton;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                _aInputField.onValueChanged.AddListener(AnyInputField_OnValueChanged);
                _bInputField.onValueChanged.AddListener(AnyInputField_OnValueChanged);
                _addButton.onClick.AddListener(AddButton_OnClicked);
                _resetButton.onClick.AddListener(ResetButton_OnClicked);
                AnyInputField_OnValueChanged("");
                
                //
                CalculatorModel calculatorModel = Context.ModelLocator.GetItem<CalculatorModel>();
                calculatorModel.A.OnValueChanged.AddListener(
                    (p, c) => _aInputField.text = c.ToString());
                
                calculatorModel.B.OnValueChanged.AddListener(
                    (p, c) => _bInputField.text = c.ToString());
                
                calculatorModel.Result.OnValueChanged.AddListener(
                    (p, c) => _resultInputField.text = c.ToString());
                
                //
                //Alternative Solution (Not Needed)
                //Context.CommandManager.AddCommandListener<ResetCommand>(OnResetCommand);
            }
        }


        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Other Methods ---------------------------------
        //Alternative Solution (Not Needed)
        // private void OnResetCommand(ResetCommand e)
        // {
        //     _aInputField.text = "0";
        //     _bInputField.text = "0";
        //     _resultInputField.text = "0";
        // }
        
        //  Event Handlers --------------------------------
        private void AnyInputField_OnValueChanged(string ignoreArg)
        {
            RequireIsInitialized();

            bool aHasValidInput = _aInputField.text.Length > 0 &&
                                 _aInputField.text != "0";
            
            bool bHasValidInput = _bInputField.text.Length > 0 &&
                                  _bInputField.text != "0";

            bool hasValidInput = aHasValidInput && 
                                 bHasValidInput;
            
            bool hasAnyInput = aHasValidInput || 
                               bHasValidInput;

            _addButton.interactable = hasValidInput;
            _resetButton.interactable = hasAnyInput;
        }
        
        
        private void AddButton_OnClicked()
        {
            RequireIsInitialized();

            OnAdd.Invoke();
        }
        
        
        private void ResetButton_OnClicked()
        {
            RequireIsInitialized();

            OnReset.Invoke();
        }
    }
}
