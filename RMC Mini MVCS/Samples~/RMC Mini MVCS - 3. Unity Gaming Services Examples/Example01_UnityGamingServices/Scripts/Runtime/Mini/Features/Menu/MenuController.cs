using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Features.SceneSystem;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.View;
using RMC.Core.Architectures.Mini.Samples.UGS.Standard;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class MenuController: BaseController // Extending 'base' is optional
        <UgsModel, MenuView, AuthenticationService> 
    {
        public MenuController(
            UgsModel model, MenuView view, AuthenticationService service) 
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
                _view.OnPlayerAccounts.AddListener(View_OnPlayerAccounts);
                _view.OnCloudSave.AddListener(View_OnCloudSave);
                _view.OnUserGeneratedContent.AddListener(View_OnUserGeneratedContent);
      
            }
        }


        //  Methods ---------------------------------------
        public override void Dispose()
        {
            // Model
            _model.IsSignedIn.OnValueChanged.RemoveListener(IsLoggedIn_OnValueChanged);
        }


        //  Event Handlers --------------------------------
        private void IsLoggedIn_OnValueChanged(bool previousValue, bool currentValue)
        {
            _view.RefreshUI();
        }

        
        private void View_OnPlayerAccounts()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(UgsConstants.Scene02_Ugs_PlayerAccounts));
        }
        
        private void View_OnCloudSave()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(UgsConstants.Scene03_Ugs_CloudSave));
        }
        
        private void View_OnUserGeneratedContent()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new LoadSceneRequestCommand(UgsConstants.Scene04_Ugs_UserGeneratedContent));
        }

        
    }
}