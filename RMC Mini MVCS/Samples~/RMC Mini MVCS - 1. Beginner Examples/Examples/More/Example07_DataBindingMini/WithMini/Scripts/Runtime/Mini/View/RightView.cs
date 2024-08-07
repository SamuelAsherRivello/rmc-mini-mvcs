using System;
using RMC.Mini.View;
using RMC.Mini.Samples.DataBindingMini.WithMini.Mini.Model;
using UnityEngine;
using UnityEngine.UI;

namespace RMC.Mini.Samples.DataBindingMini.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class RightView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        
        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        //  Fields ----------------------------------------

        [SerializeField] 
        private InputField _inputField;

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                // Binding - Setup
                DataBindingMiniModel dataBindingMiniModel =
                    Context.ModelLocator.GetItem<DataBindingMiniModel>();
                
                // Binding - If Model Changes...
                dataBindingMiniModel.Message.OnValueChanged.AddListener(
                    Message_OnValueChanged, true);
                
                // Binding - If View Changes...
                _inputField.onValueChanged.AddListener(
                    InputField_OnValueChanged);
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
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void Message_OnValueChanged(string previousValue, 
            string currentValue)
        {
            // Binding - If Model Changes...
            _inputField.text = currentValue;
        }
        
        private void InputField_OnValueChanged(string value)
        {
            // Binding - Setup
            DataBindingMiniModel dataBindingMiniModel =
                Context.ModelLocator.GetItem<DataBindingMiniModel>();
                
            // Binding - If View Changes...
            dataBindingMiniModel.Message.Value = value;
        }
    }
}