
namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Data transfer object (DTO) containing all
    /// info needed to request a login.
    /// </summary>
    public class UserData
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public string Username { get { return _username;} }
        public string Password { get { return _password;} }
        
        //  Fields ----------------------------------------
        private readonly string _username;
        private readonly string _password;

        //  Initialization  -------------------------------
        public UserData(string username, string password)
        {
            _username = username;
            _password = password;
        }
        
        //  Methods ---------------------------------------
        public override string ToString()
        {
            return $"[UserData ({_username}, {_password})]";
        }


        //  Event Handlers --------------------------------

    }
}