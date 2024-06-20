using RMC.Mini.Controller;
using RMC.Mini.Features.SceneSystem;
using RMC.Mini.Samples.UGS.Mini.Model;
using RMC.Mini.Samples.UGS.Mini.Service;
using RMC.Mini.Samples.UGS.Mini.View;
using RMC.Mini.Samples.UGS.Standard;
using UnityEngine;
using UnityEngine.SceneManagement;
using Service_AnalyticsService = RMC.Mini.Samples.UGS.Mini.Service.AnalyticsService;

namespace RMC.Mini.Samples.UGS.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class HudController: BaseController // Extending 'base' is optional
        <UgsModel, HudView, AuthenticationService>
    {

        private Service_AnalyticsService _analyticsService;
        
        public HudController(
            UgsModel model, HudView view, AuthenticationService authenticationService, 
            Service_AnalyticsService analyticsService)
            : base(model, view, authenticationService)
        {
            _analyticsService = analyticsService;
        }

        
        //  Initialization  -------------------------------
        public override async void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Model
                _model.IsSignedIn.OnValueChanged.AddListener(IsSignedIn_OnValueChanged);
                
                // View
                _view.OnBack.AddListener(View_OnBack);
                _view.OnDeveloperConsole.AddListener(View_OnDeveloperConsole);
                
                // Services - Local
                _service.OnSignInComplete.AddListener(Service_OnSignInComplete);
                _service.OnSignInFailed.AddListener(Service_OnSignInFailed);
                _service.OnSignOutComplete.AddListener(Service_OnSignOutComplete);
                _service.OnExpire.AddListener(Service_OnExpire);
                
                // Init
                if (_model.SceneHasAutoAuthentication.Value && !_model.IsSignedIn.Value)
                {
                    await _service.SignInAsync();
                }
                else
                {
                    _model.IsSignedIn.OnValueChangedRefresh();
                }

                await _analyticsService.RecordEvent(new SceneVisitEvent(SceneManager.GetActiveScene().name));
            }
        }
        
        

        
        public override void Dispose()
        {
            // Model
            _model.IsSignedIn.OnValueChanged.RemoveListener(IsSignedIn_OnValueChanged);
                
            // Service  
            _service.OnSignInComplete.RemoveListener(Service_OnSignInComplete);
            _service.OnSignInFailed.RemoveListener(Service_OnSignInFailed);
            _service.OnSignOutComplete.RemoveListener(Service_OnSignOutComplete);
            _service.OnExpire.RemoveListener(Service_OnExpire);
        }
        
        //  Methods ---------------------------------------
        private void IsSignedIn_OnValueChanged(bool previousValue, bool currentValue)
        {
            _view.RefreshUI();
        }
        
        
        //  Event Handlers --------------------------------
        private void Service_OnSignInComplete(AuthenticationService authenticationService)
        {
            RequireIsInitialized();
            
            Debug.Log("Service_OnSignInComplete");
            _model.AuthenticationService.Value = authenticationService;
            _model.IsSignedIn.Value = _model.AuthenticationService.Value.IsSignedIn;
        }
        
        private void Service_OnSignOutComplete(AuthenticationService authenticationService)
        {
            RequireIsInitialized();
            
            Debug.Log("Service_OnSignOutComplete");
            _model.AuthenticationService.Value = authenticationService;
            _model.IsSignedIn.Value = false;
        }
        
        private void Service_OnSignInFailed(AuthenticationService authenticationService)
        {
            RequireIsInitialized();
            
            Debug.Log("Service_OnSignInFailed");
            _model.AuthenticationService.Value = authenticationService;
            _model.IsSignedIn.Value = false;
        }
        
        private void Service_OnExpire(AuthenticationService authenticationService)
        {
            RequireIsInitialized();
            
            Debug.Log("Service_OnExpire");
            _model.AuthenticationService.Value = authenticationService;
            _model.IsSignedIn.Value = false;

        }

        
        private void View_OnBack()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(UgsConstants.Scene01_Ugs_Menu));
        }
        
        private void View_OnDeveloperConsole()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(UgsConstants.Scene05_Ugs_DeveloperConsole));
        }
    }
}