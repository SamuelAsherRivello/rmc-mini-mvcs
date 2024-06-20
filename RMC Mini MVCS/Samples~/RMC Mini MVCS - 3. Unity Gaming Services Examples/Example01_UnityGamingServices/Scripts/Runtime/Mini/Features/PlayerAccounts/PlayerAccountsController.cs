using RMC.Mini.Controller;
using RMC.Mini.Samples.UGS.Mini.Model;
using RMC.Mini.Samples.UGS.Mini.Service;
using RMC.Mini.Samples.UGS.Mini.View;

namespace RMC.Mini.Samples.UGS.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class PlayerAccountsController: BaseController // Extending 'base' is optional
        <UgsModel, PlayerAccountsView, PlayerAccountsService> 
    {
        public PlayerAccountsController(
            UgsModel model, PlayerAccountsView view, PlayerAccountsService service) 
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
                
                // View
                _view.OnSignIn.AddListener(View_OnSignIn);
                _view.OnSignOut.AddListener(View_OnSignOut);
                _view.OnOpenDashboard.AddListener(View_OnOpenDashboard);
                _view.OnOpenDocumentation.AddListener(View_OnOpenDocumentation);
                
                // Service
                _service.OnSignInComplete.AddListener(Service_OnSignInComplete);
            }
        }

        public override void Dispose()
        {
            // Model
            _model.IsSignedIn.OnValueChanged.RemoveListener(IsLoggedIn_OnValueChanged);
                
            // Service
            _service.OnSignInComplete.RemoveListener(Service_OnSignInComplete);
        }
        
        
        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
        private void IsLoggedIn_OnValueChanged(bool previousValue, bool currentValue)
        {
            _view.RefreshUI();
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