using RMC.Mini.Experimental.ContextLocators;
using RMC.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Mini.Samples.Tutorial.Mini.Controller;
using RMC.Mini.Samples.Tutorial.Mini.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RMC.Mini.Samples.Tutorial
{
    /// <summary>
    /// The Example is the main entry point to the demo.
    ///
    /// A tutorial does not need to load a separate scene,
    /// that is done here only to demonstrate that this
    /// completely unrelated code can properly decorate
    /// a user tutorial experience 'on top' of a system
    /// without the system knowing.
    /// </summary>
    public class TutorialMiniExample: MonoBehaviour
    {

        //Mini does NOT require any MonoBehaviours ...
        //But here is an optional one to ease UI setup ...
        [SerializeField] 
        private TutorialView _view;
        
        protected void Awake ()
        {
            // Prepare to reference the MVCS
            MakeBridgeToMini();
            
            // Load an unrelated scene with the MVCS to reference
            SceneManager.LoadScene("LoginWithMiniExample", LoadSceneMode.Additive);
        }
        
        /// <summary>
        /// EXPERIMENTAL: This and any use of <see cref="ContextLocator"/>
        /// is experimental. It's a leading solution for
        /// any scope 'outside' of MVCS to reference 'inside'
        /// the MVCS.
        /// </summary>
        private void MakeBridgeToMini()
        {
            // When any context is added ...
            ContextLocator.Instance.OnItemAdded.AddListener(context =>
            {
                // ... and any model is added ...
                context.ModelLocator.OnItemAdded.AddListener(model =>
                {
                    // ... check for matching model ...
                    LoginModel loginModel = (LoginModel)model;
                    if (loginModel != null)
                    {
                        // ... listen to changes in the appropriate model
                        OnLoginModelLocated(loginModel);
                    }
                });
            });
        }

        private void OnLoginModelLocated(LoginModel loginModel)
        {
            
            // NOTE: Special type check> See method comments.
            ContextWithLocator.AssertIsContextWithLocator(loginModel.Context);

            //View
            _view.Initialize(loginModel.Context);

            //Controller
            TutorialController tutorialController = new TutorialController(loginModel, _view);
            tutorialController.Initialize(loginModel.Context);
            

        }

    }
}