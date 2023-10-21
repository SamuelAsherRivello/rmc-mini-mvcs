
using RMC.Core.Architectures.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller.Commands
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Command is a stand-alone object containing
    /// all arguments needed to perform a request
    /// </summary>
    public class LoginCompletedCommand : ICommand
    {

        //  Properties ------------------------------------
        public UserData UserData { get { return _userData;}}
        public bool WasSuccess { get { return _wasSuccess;}}
        
        //  Fields ----------------------------------------
        private readonly UserData _userData;
        private readonly bool _wasSuccess;
        
        //  Initialization  -------------------------------
        public LoginCompletedCommand(UserData userData, bool wasSuccess)
        {
            _userData = userData;
            _wasSuccess = wasSuccess;
        }
    }
}