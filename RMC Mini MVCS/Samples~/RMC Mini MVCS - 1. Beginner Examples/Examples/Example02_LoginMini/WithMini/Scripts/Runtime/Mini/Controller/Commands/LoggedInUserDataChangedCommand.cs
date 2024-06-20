using RMC.Mini.Controller.Commands;
using RMC.Mini.Samples.Login.WithMini.Mini.Model;

namespace RMC.Mini.Samples.Login.WithMini.Mini.Controller.Commands
{
    /// <summary>
    /// This Command is a stand-alone object containing
    /// the before and after value during a data change
    /// </summary>
    public class LoggedInUserDataChangedCommand : ValueChangedCommand<UserData>
    {
        //  Initialization  -------------------------------
        public LoggedInUserDataChangedCommand(UserData previousValue, UserData currentValue) : 
            base(previousValue, currentValue)
        {
        }
    }
}