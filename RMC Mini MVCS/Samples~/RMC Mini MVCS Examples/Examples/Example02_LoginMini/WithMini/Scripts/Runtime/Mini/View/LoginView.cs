using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class LoginUnityEvent : UnityEvent<UserData> {}
    public class CanLoginChangedUnityEvent : UnityEvent<bool> {}
    public class LogoutUnityEvent : UnityEvent {}
    
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class LoginView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly LoginUnityEvent OnLogin = new LoginUnityEvent();
        
        [HideInInspector] 
        public readonly CanLoginChangedUnityEvent OnCanLoginChanged = new CanLoginChangedUnityEvent();
        
        [HideInInspector] 
        public readonly LogoutUnityEvent OnLogout = new LogoutUnityEvent();
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private Text _titleText;

        [SerializeField] 
        private Text _statusText;

        [SerializeField] 
        private InputField _usernameInputField;

        [SerializeField] 
        private InputField _passwordInputField;

        [SerializeField] 
        private Button _loginButton;
        
        [SerializeField] 
        private Button _logoutButton;

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //
                _loginButton?.onClick.AddListener(LoginButton_OnClicked);
                _logoutButton?.onClick.AddListener(LogoutButton_OnClicked);
                _usernameInputField?.onValueChanged.AddListener(AnyInputField_OnValueChanged);
                _passwordInputField?.onValueChanged.AddListener(AnyInputField_OnValueChanged);
                AnyInputField_OnValueChanged("");
                    
                //
                Context.CommandManager.AddCommandListener<LogoutCommand>(
                    OnLogoutCommand);
                Context.CommandManager.AddCommandListener<LoginSubmittedCommand>(
                    OnLoginSubmittedCommand);
                Context.CommandManager.AddCommandListener<LoginCompletedCommand>(
                    OnLoginCompletedCommand);

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
        private void AnyInputField_OnValueChanged(string _)
        {
            bool isValidInput = _usernameInputField.text.Length > 0 && 
                                _passwordInputField.text.Length > 0;

            _loginButton.interactable = isValidInput;
            OnCanLoginChanged.Invoke(_loginButton.interactable);
        }
        
        
        private void LoginButton_OnClicked()
        {
            OnLogin.Invoke(new UserData(_usernameInputField.text, _passwordInputField.text));
        }
        
        
        private void LogoutButton_OnClicked()
        {
            OnLogout.Invoke();
        }
        
        
        protected void OnDestroy()
        {
            Context?.CommandManager?.RemoveCommandListener<LogoutCommand>(
                OnLogoutCommand);
            Context?.CommandManager?.RemoveCommandListener<LoginSubmittedCommand>(
                OnLoginSubmittedCommand);
            Context?.CommandManager?.RemoveCommandListener<LoginCompletedCommand>(
                OnLoginCompletedCommand);
        }

        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        private void OnLogoutCommand(LogoutCommand logoutCommand)
        {
            RequireIsInitialized();

            if (_loginButton != null) _loginButton.interactable = false;
            if (_logoutButton != null) _logoutButton.interactable = false;
            if (_usernameInputField != null) _usernameInputField.interactable = true;
            if (_passwordInputField != null) _passwordInputField.interactable = true;
            if (_usernameInputField != null) _usernameInputField.text = "";
            if (_passwordInputField != null) _passwordInputField.text = "";

            // Demo - Controller may update view DIRECTLY...
            if (_titleText != null) _titleText.text = "Login, Mini Example";
            if (_statusText != null) _statusText.text = "Enter Username & Password (Hint: 'pass1234') ...";

        }
        
        private void OnLoginSubmittedCommand(LoginSubmittedCommand loginSubmittedCommand)
        {
            RequireIsInitialized();

            _loginButton.interactable = false;
            _logoutButton.interactable = false;
            _usernameInputField.interactable = false;
            _passwordInputField.interactable = false;
            
            _statusText.text = $"Submitting Password of {loginSubmittedCommand.UserData.Password} ...";
        }
        
        private void OnLoginCompletedCommand(LoginCompletedCommand loginCompletedCommand)
        {
            RequireIsInitialized();

            string result = "";
            if (loginCompletedCommand.WasSuccess)
            {
                result = "Success";
            }
            else
            {
                result = "Failed";
            }
            
            _loginButton.interactable = false;
            _logoutButton.interactable = true;
            _usernameInputField.interactable = false;
            _passwordInputField.interactable = false;
            
            _statusText.text = $"{result} for Password of {loginCompletedCommand.UserData.Password}!";
            
            //NOTE...
            if (loginCompletedCommand.WasSuccess)
            {
                //The view used only Commands to observe state above (good).
                //  Another option is to reference the model per below (bad),
                //  or observe the model events (good)
                LoginModel loginModel = Context.ModelLocator.GetItem<LoginModel>();
                Debug.Log($"View Sees Model with {loginModel.LoggedInUserData.Value}");
            }
        }
    }
}