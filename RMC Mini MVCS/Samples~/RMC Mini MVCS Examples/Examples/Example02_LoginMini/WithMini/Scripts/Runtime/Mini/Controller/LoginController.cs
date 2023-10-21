using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class LoginController: BaseController // Extending 'base' is optional
        <LoginModel, LoginView, LoginService> 
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------

        public LoginController(LoginModel model, LoginView view, LoginService service) 
            : base(model, view, service)
        {
            
        }

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                //
                _model.LoggedInUserData.OnValueChanged.AddListener(Model_OnLoggedInUserDataChanged);
                _view.OnLogin.AddListener(View_OnLogin);
                _view.OnLogout.AddListener(View_OnLogout);
                _view.OnCanLoginChanged.AddListener(View_OnCanLoginChanged);
                _service.OnLoginCompleted.AddListener(Service_OnLoginCompleted);

                // Demo - Controller may update model DIRECTLY...
                _model.LoggedInUserData.Value = null;
                
                // Clear
                View_OnLogout();
            }
        }



        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
        private void View_OnCanLoginChanged(bool canLogin)
        {
            _model.CanLogin.Value = canLogin;
        }
        
        
        private void View_OnLogin(UserData userData)
        {
            RequireIsInitialized();

            Context.CommandManager.InvokeCommand(new LoginSubmittedCommand(
                userData));
            
            // Demo - Controller may update service DIRECTLY...
            _service.Login(userData);
        }
        
        private void View_OnLogout()
        {
            RequireIsInitialized();
            
            Context.CommandManager.InvokeCommand(new LogoutCommand());
        }
            
        private void Service_OnLoginCompleted(UserData userData, bool wasSuccess)
        {
            RequireIsInitialized();

            if (wasSuccess)
            {
                // Demo - Controller may update model DIRECTLY...
                _model.LoggedInUserData.Value = userData;
            }
            else
            {
                // Demo - Controller may update model DIRECTLY...
                _model.LoggedInUserData.Value = null;
            }
            
            Context.CommandManager.InvokeCommand(
                new LoginCompletedCommand(userData, wasSuccess));
        }
        
        
        private void Model_OnLoggedInUserDataChanged(UserData previousValue, UserData currentValue)
        {
            RequireIsInitialized();
            
            Context.CommandManager.InvokeCommand(
                new LoggedInUserDataChangedCommand(previousValue, currentValue));

        }
    }
}