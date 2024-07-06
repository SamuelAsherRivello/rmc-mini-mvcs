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
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="MenuController"/>
    /// 
    /// </summary>
    public class MenuView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnPlayerAccounts = new UnityEvent();
        
        [HideInInspector] 
        public readonly UnityEvent OnCloudSave = new UnityEvent();
        
        [HideInInspector] 
        public readonly UnityEvent OnUserGeneratedContent = new UnityEvent();

        
        
        public Label LeftTitleLabel { get { return _uiDocument?.rootVisualElement.Q<Label>("LeftTitleLabel"); }}
        public Label RightTitleLabel { get { return _uiDocument?.rootVisualElement?.Q<Label>("RightTitleLabel"); }}
        public ListView ListView { get { return _uiDocument?.rootVisualElement.Q<ListView>("LeftListView"); }}

        public Button PlayerAccountsButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button01"); }}
        public Button CloudSaveButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button02"); }}
        public Button UserGeneratedContentButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button03"); }}
        public Button Button04 { get { return _uiDocument?.rootVisualElement.Q<Button>("Button04"); }}
        public Button Button05 { get { return _uiDocument?.rootVisualElement.Q<Button>("Button05"); }}

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

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

                //
                PlayerAccountsButton.clicked += PlayerAccountsButtonOnClicked;
                CloudSaveButton.clicked += CloudSaveButton_OnClicked;
                UserGeneratedContentButton.clicked += UserGeneratedContentButton_OnClicked;
  
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
            UgsModel model = Context.ModelLocator.GetItem<UgsModel>();
            
            //
            RightTitleLabel.text = $"Play or\nCustomize";
            PlayerAccountsButton.text = "Player Accounts";
            CloudSaveButton.text = "Cloud Save";
            UserGeneratedContentButton.text = "User Generated Content";
            //
            LeftTitleLabel.visible = false;
            Button04.visible = false;
            Button05.visible = false;
            ListView.visible = false;

        }
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void PlayerAccountsButtonOnClicked()
        {
            OnPlayerAccounts.Invoke();
        }
        
        private void CloudSaveButton_OnClicked()
        {
            OnCloudSave.Invoke();
        }
        
        private void UserGeneratedContentButton_OnClicked()
        {
            OnUserGeneratedContent.Invoke();
        }
    }
}