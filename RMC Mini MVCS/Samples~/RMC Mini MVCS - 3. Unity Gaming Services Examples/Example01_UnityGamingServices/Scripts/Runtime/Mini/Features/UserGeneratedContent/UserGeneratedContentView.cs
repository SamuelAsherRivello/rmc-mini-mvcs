using System;
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
    public enum UserGeneratedContentMessage
    {
        Null,
        Loading,
        Loaded,
        Saving,
        Saved,
        Deleting,
        Deleted,
    }
    
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="UserGeneratedContentController"/>
    /// 
    /// </summary>
    public class UserGeneratedContentView: MonoBehaviour, IView
    {
        
        //  Events ----------------------------------------
  
        [HideInInspector] 
        public readonly UnityEvent OnLoad = new UnityEvent();

        [HideInInspector] 
        public readonly UnityEvent OnSave = new UnityEvent();

        [HideInInspector] 
        public readonly UnityEvent OnDelete = new UnityEvent();

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
        
        public Button DeleteButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button03"); }}

        public Button OpenDashboardButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button04"); }}
        
        public Button OpenDocumentationButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button05"); }}

        public UserGeneratedContentMessage UserGeneratedContentMessage
        {
            get
            {
                return _userGeneratedContentMessage;
            }
            set
            {
                _userGeneratedContentMessage = value;
                RefreshUI();
            }
        }


        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        private UserGeneratedContentMessage _userGeneratedContentMessage = UserGeneratedContentMessage.Null;
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
                DeleteButton.clicked += DeleteButton_OnClicked;
                OpenDashboardButton.clicked += OpenDashboardButton_OnClicked;
                OpenDocumentationButton.clicked += OpenDocumentationButton_OnClicked;

                _listView = new UgsListView(ListView);
                _listView.OnSelectionChange.AddListener(ListView_OnSelectionChange);
                _listView.ItemsSource = null; //Refresh cleanly
                
                RefreshUI();
                
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
            UserGeneratedContentModel userGeneratedContentModel = 
                Context.ModelLocator.GetItem<UserGeneratedContentModel>();

            if (UserGeneratedContentMessage == UserGeneratedContentMessage.Null)
            {
                RightTitleLabel.text = $"UGC";
            }
            else
            {
                RightTitleLabel.text = $"{UserGeneratedContentMessage.ToString()}\n";
            }
            
            //
            LoadButton.text = "Load";
            SaveButton.text = "Save";
            DeleteButton.text = "Delete"; 
            OpenDashboardButton.text = "Open Dashboard";
            OpenDocumentationButton.text = "Open Documentation";
            //
            LoadButton.SetEnabled(ugsModel.IsSignedIn.Value);
            SaveButton.SetEnabled(ugsModel.IsSignedIn.Value);
            DeleteButton.SetEnabled(ugsModel.IsSignedIn.Value && 
                                    userGeneratedContentModel.HasSelectedListItem);
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
        
        private void DeleteButton_OnClicked()
        {
            OnDelete.Invoke();
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