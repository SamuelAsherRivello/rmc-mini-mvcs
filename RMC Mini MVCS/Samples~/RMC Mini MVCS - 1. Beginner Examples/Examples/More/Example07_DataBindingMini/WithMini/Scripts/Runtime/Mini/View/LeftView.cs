using System;
using RMC.Core.Architectures.Mini.Samples.DataBindingMini.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Core.Architectures.Mini.Samples.DataBindingMini.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class LeftView: MonoBehaviour, IView
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