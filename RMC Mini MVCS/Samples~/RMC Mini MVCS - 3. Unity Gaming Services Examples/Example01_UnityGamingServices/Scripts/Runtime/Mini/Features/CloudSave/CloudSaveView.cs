using System;
using System.Collections.Generic;
using RMC.Mini.View;
using RMC.Mini.Samples.UGS.Mini.Controller;
using RMC.Mini.Samples.UGS.Mini.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Mini.Samples.UGS.Mini.View
{
    //  Enums ----------------------------------------
    public enum CloudSaveMessage
    {
        Null,
        Loading,
        Saving,
        Deleting,
        Loaded,
        Deleted,
        Saved
    }

    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="CloudSaveController"/>
    /// 
    /// </summary>
    public class CloudSaveView: MonoBehaviour, IView
    {
        
        //  Events ----------------------------------------
  
        [HideInInspector] 
        public readonly UnityEvent OnLoad = new UnityEvent();

        [HideInInspector] 
        public readonly UnityEvent OnSave = new UnityEvent();

        [HideInInspector] 
        public readonly UnityEvent OnDeleteAll = new UnityEvent();
        
        [HideInInspector] 
        public readonly UgsListViewEntryUnityEvent OnListItemSelectionChange = new UgsListViewEntryUnityEvent();

        [HideInInspector] 
        public readonly UnityEvent OnOpenDocumentation = new UnityEvent();
        
        [HideInInspector] 
        public readonly UnityEvent OnOpenDashboard = new UnityEvent();

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        public Label LeftTitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("LeftTitleLabel"); }}
        public Label RightTitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("RightTitleLabel"); }}
        
        public ListView ListView { get { return _uiDocument?.rootVisualElement.Q<ListView>("LeftListView"); }}
        
        public Button LoadButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button01"); }}
        
        public Button SaveButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button02"); }}
        
        public Button DeleteAllButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button03"); }}

        public Button OpenDashboardButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button04"); }}
        
        public Button OpenDocumentationButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button05"); }}

        public CloudSaveMessage CloudSaveMessage
        {
            get
            {
                return _cloudSaveMessage;
            }
            set
            {
                _cloudSaveMessage = value;
                RefreshUI();
            }
        }


        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        private CloudSaveMessage _cloudSaveMessage = CloudSaveMessage.Null;
        private UgsListView _listView;
        
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                LoadButton.clicked += LoadButton_OnClicked;
                SaveButton.clicked += SaveButton_OnClicked;
                DeleteAllButton.clicked += DeleteAllButtonOnClicked;
                OpenDashboardButton.clicked += OpenDashboardButton_OnClicked;
                OpenDocumentationButton.clicked += OpenDocumentationButton_OnClicked;

                _listView = new UgsListView(ListView);
                _listView.OnSelectionChange.AddListener(ListView_OnSelectionChange);
                _listView.ItemsSource = null; //Refresh cleanly
            }
        }

        private void ListView_OnSelectionChange(UgsListViewEntry ugsListViewEntry)
        {
            OnListItemSelectionChange.Invoke(ugsListViewEntry);

        }


        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Unity Methods ---------------------------------
        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }


        //  Methods ---------------------------------------
        public void RefreshUI()
        {
            // Model - Shared
            UgsModel ugsModel = Context.ModelLocator.GetItem<UgsModel>();
                
            // Model - Local
            CloudSaveModel cloudSaveModel = Context.ModelLocator.GetItem<CloudSaveModel>();
            
            // Message
            if (CloudSaveMessage == CloudSaveMessage.Null)
            {
                RightTitleLabel.text = $"Cloud Save";
            }
            else
            {
                RightTitleLabel.text = $"{CloudSaveMessage.ToString()}\n";
            }
            
            var list = new List<UgsListViewEntry>();
            foreach (var keyValuePair in cloudSaveModel.CloudSaveItems.Value)
            {
                string title = $"{keyValuePair.Value.Value.GetAsString()}";
                string data = keyValuePair.Key;
                list.Add(new UgsListViewEntry(title, data));
            }
            _listView.ItemsSource = list;

            //
            LoadButton.text = "Load";
            SaveButton.text = "Save";
            DeleteAllButton.text = "Delete All";
            OpenDashboardButton.text = "Open Dashboard";
            OpenDocumentationButton.text = "Open Documentation";
            //
            LoadButton.SetEnabled(ugsModel.IsSignedIn.Value);
            SaveButton.SetEnabled(ugsModel.IsSignedIn.Value);
            DeleteAllButton.SetEnabled(ugsModel.IsSignedIn.Value);
            //
            LeftTitleLabel.text = "Items";
        }
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void LoadButton_OnClicked()
        {
            OnLoad.Invoke();
        }
        
        private void SaveButton_OnClicked()
        {
            OnSave.Invoke();
        }
        
        private void DeleteAllButtonOnClicked()
        {
            OnDeleteAll.Invoke();
        }
        
        
        private void OpenDashboardButton_OnClicked()
        {
            OnOpenDashboard.Invoke();
        }
        
        private void OpenDocumentationButton_OnClicked()
        {
            OnOpenDocumentation.Invoke();
        }
    }
}