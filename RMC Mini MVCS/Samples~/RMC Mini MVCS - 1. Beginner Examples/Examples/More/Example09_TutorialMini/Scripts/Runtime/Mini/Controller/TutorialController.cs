using System;
using RMC.Core.Architectures.Mini;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.IntroToUnity.Samples.Tutorial.Mini.View;

namespace RMC.IntroToUnity.Samples.Tutorial.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class TutorialController : IController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;
        private readonly LoginModel _loginModel;
        private readonly TutorialView _tutorialView;

        //  Initialization  -------------------------------
        public TutorialController(LoginModel loginModel, TutorialView tutorialView)
        {
            _loginModel = loginModel;
            _tutorialView = tutorialView;
        }

        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //
                _tutorialView.TitleText.text = "Tutorial";
                _loginModel.CanLogin.OnValueChanged.AddListener(LoginModel_CanLogin_OnValueChanged);
                
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
        
        //  Dispose Methods --------------------------------
        public virtual void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Methods ---------------------------------------
  

        //  Event Handlers --------------------------------
         private void OnLogoutCommand(LogoutCommand logoutCommand)
        {
            RequireIsInitialized();
            _tutorialView.BodyText.text = "Enter username and password.";

        }
         
         private void LoginModel_CanLogin_OnValueChanged(bool previousValue, bool currentValue)
         {
             if (currentValue)
             {
                 _tutorialView.BodyText.text = "Enter username and password. Click Login when ready.";
             }
             else
             {
                 _tutorialView.BodyText.text = "Enter username and password.";
             }
         }
        
        private void OnLoginSubmittedCommand(LoginSubmittedCommand loginSubmittedCommand)
        {
            RequireIsInitialized();
            _tutorialView.BodyText.text = "Wait for result.";
        }
        
        private void OnLoginCompletedCommand(LoginCompletedCommand loginCompletedCommand)
        {
            RequireIsInitialized();

            if (loginCompletedCommand.WasSuccess)
            {
                _tutorialView.BodyText.text = "You completed the tutorial.";
            }
            else
            {
                _tutorialView.BodyText.text = "You failed the tutorial. Try again.";
            }
        }
    }
}