using RMC.Mini.Controller.Commands;
using RMC.Mini.Samples.Login.WithMini.Mini.Model;

namespace RMC.Mini.Samples.Login.WithMini.Mini.Controller.Commands
{
    /// <summary>
    /// The Command is a stand-alone object containing
    /// all arguments needed to perform a request
    /// </summary>
    public class LoginSubmittedCommand : ICommand
    {

        //  Properties ------------------------------------
        public UserData UserData { get { return _userData;}}
        
        
        //  Fields ----------------------------------------
        private readonly UserData _userData;
        
        
        //  Initialization  -------------------------------
        public LoginSubmittedCommand(UserData userData)
        {
            _userData = userData;
        }
    }
}