using RMC.Mini.Controller;
using RMC.Mini.Service;
using RMC.Mini.Samples.UGS.Mini.Model;
using RMC.Mini.Samples.UGS.Mini.Service;
using RMC.Mini.Samples.UGS.Mini.View;

namespace RMC.Mini.Samples.UGS.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class DeveloperConsoleController: BaseController // Extending 'base' is optional
        <UgsModel, DeveloperConsoleView, DummyService> 
    {
        public DeveloperConsoleController(
            UgsModel model, DeveloperConsoleView view) 
            : base(model, view, new DummyService())
        {
        }

        private CloudSaveService _cloudSaveService;
        private UserGeneratedContentService _userGeneratedContentService;
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Model - Shared
                UgsModel ugsModel = Context.ModelLocator.GetItem<UgsModel>();
                ugsModel.IsSignedIn.OnValueChanged.AddListener(IsSignedIn_OnValueChanged);

                // View
                _view.OnDeleteAllCloudSaves.AddListener(View_OnDeleteAllCloudSaves);
                _view.OnDeleteAllUserGeneratedContent.AddListener(View_OnDeleteAllUserGeneratedContent);
                
            }
        }
        

        public override void Dispose()
        {
            // Model - Shared
            UgsModel ugsModel = Context.ModelLocator.GetItem<UgsModel>();
            ugsModel.IsSignedIn.OnValueChanged.RemoveListener(IsSignedIn_OnValueChanged);

        }
        
        //  Methods ---------------------------------------
        public void AddServices(
            CloudSaveService cloudSaveService, 
            UserGeneratedContentService userGeneratedContentService)
        {
            _cloudSaveService = cloudSaveService;
            _userGeneratedContentService = userGeneratedContentService;
            
        }
        
        //  Event Handlers --------------------------------
        private void IsSignedIn_OnValueChanged(bool previousValue, bool currentValue)
        {
            RequireIsInitialized();
            _view.RefreshUI();
        }
        
        private async void View_OnDeleteAllCloudSaves()
        {
            RequireIsInitialized();

            _view.DeveloperConsoleMessage = DeveloperConsoleMessage.Deleting;
            await _cloudSaveService.DeleteAllPlayerDataAsync();
            _view.DeveloperConsoleMessage = DeveloperConsoleMessage.Deleted;

        }

        private async void View_OnDeleteAllUserGeneratedContent()
        {
            RequireIsInitialized();
            
            _view.DeveloperConsoleMessage = DeveloperConsoleMessage.Deleting;
            await _userGeneratedContentService.DeleteAllContentAsync();
            _view.DeveloperConsoleMessage = DeveloperConsoleMessage.Deleted;
        }
    }
}