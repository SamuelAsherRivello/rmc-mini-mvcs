using System;
using System.Collections.Generic;
using RMC.Mini.Controller;
using RMC.Mini.Samples.UGS.Mini.Model;
using RMC.Mini.Samples.UGS.Mini.Service;
using RMC.Mini.Samples.UGS.Mini.View;
using Unity.Services.CloudSave.Models;

namespace RMC.Mini.Samples.UGS.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class CloudSaveController: BaseController // Extending 'base' is optional
        <CloudSaveModel, CloudSaveView, CloudSaveService> 
    {
        public CloudSaveController(
            CloudSaveModel model, CloudSaveView view, CloudSaveService service) 
            : base(model, view, service)
        {
        }

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Model - Shared
                UgsModel ugsModel = Context.ModelLocator.GetItem<UgsModel>();
                ugsModel.IsSignedIn.OnValueChanged.AddListener(IsSignedIn_OnValueChanged);
                
                // Model - Local
                _model.CloudSaveItems.OnValueChanged.AddListener(CloudSaveItems_OnValueChanged);
                
                // View
                _view.OnLoad.AddListener(View_OnLoad);
                _view.OnSave.AddListener(View_OnSave);
                _view.OnDeleteAll.AddListener(View_OnDeleteAll);
                _view.OnListItemSelectionChange.AddListener(View_OnListItemSelectionChange);
                _view.OnOpenDashboard.AddListener(View_OnOpenDashboard);
                _view.OnOpenDocumentation.AddListener(View_OnOpenDocumentation);
                _view.RefreshUI();
                
                // Service
                _service.OnLoadAllPlayerDataComplete.AddListener(Service_OnLoadPlayerDataAllComplete);
                _service.OnSaveAPlayerDataComplete.AddListener(Service_OnSaveAPlayerDataComplete);
                _service.OnDeleteAllPlayerDataComplete.AddListener(Service_OnDeleteAllPlayerDataComplete);
                
       
            }
        }


        public override void Dispose()
        {
            // Model - Shared
            UgsModel ugsModel = Context.ModelLocator.GetItem<UgsModel>();
            ugsModel.IsSignedIn.OnValueChanged.RemoveListener(IsSignedIn_OnValueChanged);
                
            // Model - Local
            _model.CloudSaveItems.OnValueChanged.RemoveListener(CloudSaveItems_OnValueChanged);
                
            // Service
            _service.OnLoadAllPlayerDataComplete.RemoveListener(Service_OnLoadPlayerDataAllComplete);
            _service.OnSaveAPlayerDataComplete.RemoveListener(Service_OnSaveAPlayerDataComplete);
            _service.OnDeleteAllPlayerDataComplete.RemoveListener(Service_OnDeleteAllPlayerDataComplete);
        }


        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
        
        private async void View_OnLoad()
        {
            RequireIsInitialized();

            _view.CloudSaveMessage = CloudSaveMessage.Loading;
            await _service.LoadAllPlayerDataAsync();
        }
        
        
        private async void View_OnSave()
        {
            RequireIsInitialized();

            Dictionary<string, SaveItem> playerData = new Dictionary<string, SaveItem>();
            playerData[Guid.NewGuid().ToString()] = new SaveItem($"Player reached level {UnityEngine.Random.Range(1,99)}", "");
            _view.CloudSaveMessage = CloudSaveMessage.Saving;
            await _service.SaveAPlayerDataAsync(playerData );
            
        }

        private async void View_OnDeleteAll()
        {
            RequireIsInitialized();
            
            _view.CloudSaveMessage = CloudSaveMessage.Deleting;
            await _service.DeleteAllPlayerDataAsync();
        }
        
        private void Service_OnLoadPlayerDataAllComplete(Dictionary<string, Item> items)
        {
            RequireIsInitialized();
            
            _model.CloudSaveItems.Value = items;
            _view.CloudSaveMessage = CloudSaveMessage.Saved;
        }
        
 

        private void Service_OnSaveAPlayerDataComplete(Dictionary<string, string> arg0)
        {
            _view.CloudSaveMessage = CloudSaveMessage.Saved;
            
            //Autoload again to refresh model with server data
            View_OnLoad();
        }
        
        private void Service_OnDeleteAllPlayerDataComplete()
        {
            _view.CloudSaveMessage = CloudSaveMessage.Deleted;
            
            //Autoload again to refresh model with server data
            View_OnLoad();
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
        
        private void View_OnListItemSelectionChange(UgsListViewEntry ugsListViewEntry)
        {
            if (ugsListViewEntry == null)
            {
                return;
            }
            _model.SelectedListItem.Value = ugsListViewEntry;
        }

        
        private void IsSignedIn_OnValueChanged(bool previousValue, bool currentValue)
        {
            _view.RefreshUI();
        }
            
        private void CloudSaveItems_OnValueChanged(Dictionary<string, Item> previousValue, Dictionary<string, Item> currentValue)
        {
            _view.RefreshUI();
        }
    }
}