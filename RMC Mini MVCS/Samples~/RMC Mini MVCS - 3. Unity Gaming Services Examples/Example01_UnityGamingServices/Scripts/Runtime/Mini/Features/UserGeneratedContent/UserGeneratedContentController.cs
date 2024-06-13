using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.View;
using Unity.Services.CloudSave.Models;
using Unity.Services.Ugc;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.Controller
{
    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class UserGeneratedContentController: BaseController // Extending 'base' is optional
        <UserGeneratedContentModel, UserGeneratedContentView, UserGeneratedContentService> 
    {
        public UserGeneratedContentController(
            UserGeneratedContentModel model, UserGeneratedContentView view, UserGeneratedContentService service) 
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
                _model.UserGeneratedContentItems.OnValueChanged.AddListener(UserGeneratedContentItems_OnValueChanged);
                _model.SelectedListItem.OnValueChanged.AddListener(SelectedListItem_OnValueChanged);
                _view.OnLoad.AddListener(View_OnLoad);
                _view.OnSave.AddListener(View_OnSave);
                _view.OnDelete.AddListener(View_OnDelete);
                _view.OnListItemSelectionChange.AddListener(View_OnListItemSelectionChange);
                _view.OnOpenDashboard.AddListener(View_OnOpenDashboard);
                _view.OnOpenDocumentation.AddListener(View_OnOpenDocumentation);
                
                // Service
                _service.OnGetContentsListComplete.AddListener(Service_OnGetContentsListComplete);
                _service.OnSaveAPlayerDataComplete.AddListener(Service_OnSaveAPlayerDataComplete);
                _service.OnDeleteContentComplete.AddListener(Service_OnDeleteContentComplete);
            }
        }




        public override void Dispose()
        {
            // Model - Shared
            UgsModel ugsModel = Context.ModelLocator.GetItem<UgsModel>();
            ugsModel.IsSignedIn.OnValueChanged.RemoveListener(IsSignedIn_OnValueChanged);
                
            // Model - Local
            _model.UserGeneratedContentItems.OnValueChanged.RemoveListener(UserGeneratedContentItems_OnValueChanged);
                
            // Service
            _service.OnGetContentsListComplete.RemoveListener(Service_OnGetContentsListComplete);
            _service.OnSaveAPlayerDataComplete.RemoveListener(Service_OnSaveAPlayerDataComplete);
            _service.OnDeleteContentComplete.RemoveListener(Service_OnDeleteContentComplete);
        }


        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
        
        private async void View_OnLoad()
        {
            RequireIsInitialized();

            _view.UserGeneratedContentMessage = UserGeneratedContentMessage.Loading;
            await _service.GetContentsListAsync();
        }
        
        
        private async void View_OnSave()
        {
            RequireIsInitialized();

            Dictionary<string, SaveItem> playerData = new Dictionary<string, SaveItem>();
            playerData[Guid.NewGuid().ToString()] = 
                new SaveItem($"Player reached level {UnityEngine.Random.Range(1,99)}", "");
            
            _view.UserGeneratedContentMessage = UserGeneratedContentMessage.Saving;
            await _service.SaveAPlayerDataAsync(playerData);
            
        }

        private async void View_OnDelete()
        {
            RequireIsInitialized();

            if (!_model.HasSelectedListItem)
            {
                throw new Exception("View_OnDelete() Failed. Cannot delete without a selected item");
            }
            _view.UserGeneratedContentMessage = UserGeneratedContentMessage.Deleting;
            string contentId = _model.SelectedListItem.Value.Data;
            await _service.DeleteContentAsync(contentId);
        }
        
        private void Service_OnGetContentsListComplete(PagedResults<Content> pagedContents)
        {
            RequireIsInitialized();
            
            Dictionary<string, string> contentsDictionary = new Dictionary<string, string>();
            foreach (Content content in pagedContents.Results)
            {
                contentsDictionary.Add(content.Id, content.Name);
            }

            _model.UserGeneratedContentItems.Value = contentsDictionary;
            _view.UserGeneratedContentMessage = UserGeneratedContentMessage.Loaded;
        }
        
 

        private async void Service_OnSaveAPlayerDataComplete(Content content)
        {
            _view.UserGeneratedContentMessage = UserGeneratedContentMessage.Saved;
            
            //Wait x Seconds for server. Then Autoload again to refresh model with server data
            await Task.Delay(500);
            View_OnLoad();
        }
        
        private async void Service_OnDeleteContentComplete(string contentId)
        {
            _view.UserGeneratedContentMessage = UserGeneratedContentMessage.Deleted;
            
            //Wait x Seconds for server. Then Autoload again to refresh model with server data
            await Task.Delay(500);
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
            // can be null
            _model.SelectedListItem.Value = ugsListViewEntry;
        }
        
        private void SelectedListItem_OnValueChanged(UgsListViewEntry previousValue, UgsListViewEntry currentValue)
        {
            _view.RefreshUI();
        }
        
        private void IsSignedIn_OnValueChanged(bool previousValue, bool currentValue)
        {
            _view.RefreshUI();
        }
            
        private void UserGeneratedContentItems_OnValueChanged(Dictionary<string, string> previousValue, Dictionary<string, string> currentValue)
        {
            _view.RefreshUI();
            
            var list = new List<UgsListViewEntry>();
            foreach (var keyValuePair in _model.UserGeneratedContentItems.Value)
            {
                string title = $"{keyValuePair.Value}";
                string data = keyValuePair.Key;
                list.Add(new UgsListViewEntry(title, data));
            }

            _view.ListView.itemsSource = list;
        }
    }
}