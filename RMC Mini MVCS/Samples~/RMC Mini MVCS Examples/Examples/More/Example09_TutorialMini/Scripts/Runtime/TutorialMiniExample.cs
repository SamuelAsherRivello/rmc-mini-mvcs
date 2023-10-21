using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Experimental;
using RMC.IntroToUnity.Samples.Tutorial.Mini.Controller;
using RMC.IntroToUnity.Samples.Tutorial.Mini.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RMC.IntroToUnity.Samples.Tutorial
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
        /// is experimental. Its a leading solution for
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
            //View
            _view.Initialize(loginModel.Context);
            
            //Controller
            TutorialController tutorialController = new TutorialController(loginModel, _view);
            tutorialController.Initialize(loginModel.Context);
        }

    }
}