using System;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.View
{
    //  Enums ----------------------------------------
    public enum DeveloperConsoleMessage
    {
        Null,
        Deleting,
        Deleted,
    }
    
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="DeveloperConsoleController"/>
    /// 
    /// </summary>
    public class DeveloperConsoleView: MonoBehaviour, IView
    {
        
        //  Events ----------------------------------------
  
        [HideInInspector] 
        public readonly UnityEvent OnDeleteAllUserGeneratedContent = new UnityEvent();

        [HideInInspector] 
        public readonly UnityEvent OnDeleteAllCloudSaves = new UnityEvent();

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        public Label LeftTitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("LeftTitleLabel"); }}
        public Label RightTitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("RightTitleLabel"); }}
        
        public ListView ListView { get { return _uiDocument?.rootVisualElement.Q<ListView>("LeftListView"); }}
        
        public Button DeleteAllCloudSavesButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button01"); }}
        
        public Button DeleteAllUserGeneratedContentButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button02"); }}
        
        public Button Button03 { get { return _uiDocument?.rootVisualElement.Q<Button>("Button03"); }}

        public Button Button04 { get { return _uiDocument?.rootVisualElement.Q<Button>("Button04"); }}
        
        public Button Button05 { get { return _uiDocument?.rootVisualElement.Q<Button>("Button05"); }}

        public DeveloperConsoleMessage DeveloperConsoleMessage
        {
            get
            {
                return _developerConsoleMessage;
            }
            set
            {
                _developerConsoleMessage = value;
                RefreshUI();
            }
        }


        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;
        private DeveloperConsoleMessage _developerConsoleMessage = DeveloperConsoleMessage.Null;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                DeleteAllCloudSavesButton.clicked += DeleteAllCloudSavesButton_OnClicked;
                DeleteAllUserGeneratedContentButton.clicked += DeleteAllUserGeneratedContentButton_OnClicked;
                
                RefreshUI();
                
            }
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
            if (DeveloperConsoleMessage == DeveloperConsoleMessage.Null)
            {
                RightTitleLabel.text = $"Developer\nConsole";
            }
            else
            {
                RightTitleLabel.text = $"{DeveloperConsoleMessage.ToString()}\n";
            }
            
            //
            DeleteAllCloudSavesButton.text = "Delete <b>All</b> Cloud Saves";
            DeleteAllUserGeneratedContentButton.text = "Delete <b>All</b> UGC";
            DeleteAllCloudSavesButton.SetEnabled(ugsModel.IsSignedIn.Value);
            DeleteAllUserGeneratedContentButton.SetEnabled(ugsModel.IsSignedIn.Value);
            //
            ListView.visible = false;
            LeftTitleLabel.visible = false;
            Button03.visible = false;
            Button04.visible = false;
            Button05.visible = false;
      
        }
        
        //  Event Handlers --------------------------------
        private void DeleteAllCloudSavesButton_OnClicked()
        {
            OnDeleteAllCloudSaves.Invoke();
        }
        
        private void DeleteAllUserGeneratedContentButton_OnClicked()
        {
            OnDeleteAllUserGeneratedContent.Invoke();
        }
    }
}