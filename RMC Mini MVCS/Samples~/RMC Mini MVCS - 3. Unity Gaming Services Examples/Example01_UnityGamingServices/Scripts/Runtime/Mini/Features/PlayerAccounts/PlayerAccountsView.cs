using System;
using RMC.Mini.View;
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
    /// Relates to the <see cref="PlayerAccountsController"/>
    /// 
    /// </summary>
    public class PlayerAccountsView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
  
        [HideInInspector] 
        public readonly UnityEvent OnSignIn = new UnityEvent();

        [HideInInspector] 
        public readonly UnityEvent OnSignOut = new UnityEvent();

        
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
        public Button SignInButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button01"); }}
        public Button SignOutButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button02"); }}
        public Button OpenDashboardButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button03"); }}
        public Button OpenDocumentationButton { get { return _uiDocument?.rootVisualElement.Q<Button>("Button04"); }}
        public Button Button05 { get { return _uiDocument?.rootVisualElement.Q<Button>("Button05"); }}


        
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

                SignInButton.clicked += SignInButton_OnClicked;
                SignOutButton.clicked += SignOutButton_OnClicked;
                OpenDashboardButton.clicked += OpenDashboardButton_OnClicked;
                OpenDocumentationButton.clicked += OpenDocumentationButton_OnClicked;
                
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

            if (model.IsSignedIn.Value)
            {
                RightTitleLabel.text = $"Signed In"+
                                   $"<size='25'>"+
                                   $"\nId = {model.AuthenticationService.Value.PlayerInfo.Id}" +
                                   "</size>";
            }
            else
            {
                RightTitleLabel.text = $"Signed Out";
            }
            
            //
            SignInButton.text = "Sign In";
            SignOutButton.text = "Sign Out";
            OpenDashboardButton.text = "Open Dashboard";
            OpenDocumentationButton.text = "Open Documentation";
            //
            SignInButton.SetEnabled(!model.IsSignedIn.Value);
            SignOutButton.SetEnabled(model.IsSignedIn.Value);
            //
            Button05.visible = false;
            LeftTitleLabel.visible = false;
            ListView.visible = false;
        }
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void SignInButton_OnClicked()
        {
            OnSignIn.Invoke();
        }
        
        private void SignOutButton_OnClicked()
        {
            OnSignOut.Invoke();
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