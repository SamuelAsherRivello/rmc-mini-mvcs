using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithoutMini
{
    /// <summary>
    /// Data transfer object (DTO) containing all
    /// info needed to request a login.
    /// </summary>
    public class UserData
    {
        public string Username { get { return _username;} }
        public string Password { get { return _password;} }
        
        private readonly string _username;
        private readonly string _password;

        public UserData(string username, string password)
        {
            _username = username;
            _password = password;
        }
        public override string ToString()
        {
            return $"[UserData ({_username}, {_password})]";
        }
    }

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class LoginWithoutMiniExample: MonoBehaviour
    {
        //  Events ----------------------------------------
        
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
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

        //  Unity Methods ---------------------------------
        protected void Start()
        {
            _loginButton.onClick.AddListener(LoginButton_OnClicked);
            _logoutButton.onClick.AddListener(LogoutButton_OnClicked);
            _usernameInputField.onValueChanged.AddListener(AnyInputField_OnValueChanged);
            _passwordInputField.onValueChanged.AddListener(AnyInputField_OnValueChanged);
            
            //Refresh UI
            LogoutButton_OnClicked();
        }

        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        private void AnyInputField_OnValueChanged(string ignoreArg)
        {
            bool isValidInput = _usernameInputField.text.Length > 0 && 
                                _passwordInputField.text.Length > 0;

            _loginButton.interactable = isValidInput;
        }
        
        
        private async void LoginButton_OnClicked()
        {
            UserData userData = new UserData(_usernameInputField.text, _passwordInputField.text);
            
            _loginButton.interactable = false;
            _logoutButton.interactable = false;
            _usernameInputField.interactable = false;
            _passwordInputField.interactable = false;
            _statusText.text = $"Submitting Password of {userData.Password} ...";
            
            //For simplicity we check a local file to validate.
            //In production, call a server to validate instead
            TextAsset textAsset = Resources.Load<TextAsset>("Texts/LoginWithoutMiniText"); //txt file

            //Add cosmetic delay to simulate latency
            await Task.Delay(500);
            
            bool wasSuccess = false;
            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                string correctPassword = textAsset.ToString(); //contains "pass1234"
                wasSuccess = userData.Password == correctPassword;
            }
            
            string result = "";
            if (wasSuccess)
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
            
            _statusText.text = $"{result} for Password of {userData.Password}!";
        }
        
        
        private void LogoutButton_OnClicked()
        {
            _loginButton.interactable = false;
            _logoutButton.interactable = false;
            _usernameInputField.interactable = true;
            _passwordInputField.interactable = true;
            _usernameInputField.text = "";
            _passwordInputField.text = "";

            _titleText.text = "Login, Mini Example";
            _statusText.text = "Enter Username & Password (Hint: 'pass1234') ...";
        }
    }
}