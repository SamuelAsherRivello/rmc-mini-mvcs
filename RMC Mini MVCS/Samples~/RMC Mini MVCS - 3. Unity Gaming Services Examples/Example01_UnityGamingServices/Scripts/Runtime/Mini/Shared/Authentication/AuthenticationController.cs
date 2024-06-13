using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class AuthenticationController: BaseController // Extending 'base' is optional
        <UgsModel, DummyView, AuthenticationService> 
    {
        public AuthenticationController(
            UgsModel model, DummyView view, AuthenticationService service) 
            : base(model, view, service)
        {
        }

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Model
                _model.IsSignedIn.OnValueChanged.AddListener(IsLoggedIn_OnValueChanged);
                
                // Service
                _service.OnSignInComplete.AddListener(Service_OnSignInComplete);
                _service.OnSignInFailed.AddListener(Service_OnSignInFailed);
                _service.OnSignOutComplete.AddListener(Service_OnSignOutComplete);
                _service.OnExpire.AddListener(Service_OnExpire);
            }
        }


        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
        private void IsLoggedIn_OnValueChanged(bool previousValue, bool currentValue)
        {
        }
        
        
        private async void View_OnSignIn()
        {
            RequireIsInitialized();

            await _service.SignInAsync();
        }
        
        
        private void View_OnSignOut()
        {
            RequireIsInitialized();
            
            _service.SignOutAsync();
        }

        
        private void View_OnOpenDashboard()
        {
            RequireIsInitialized();
            
            _service.OpenDashboard();
            
        }
        
        
        private void View_OnOpenDocumentation()
        {
            RequireIsInitialized();
            
            _service.OpenDocumentation();
        }
        
        
        private void Service_OnSignInComplete(AuthenticationService authenticationService)
        {
            RequireIsInitialized();
        }
        
        
        private void Service_OnSignOutComplete(AuthenticationService authenticationService)
        {
            RequireIsInitialized();
        }
        
        
        private void Service_OnSignInFailed(AuthenticationService authenticationService)
        {
            RequireIsInitialized();
        }
        
        
        private void Service_OnExpire(AuthenticationService authenticationService)
        {
            RequireIsInitialized();
        }
    }
}