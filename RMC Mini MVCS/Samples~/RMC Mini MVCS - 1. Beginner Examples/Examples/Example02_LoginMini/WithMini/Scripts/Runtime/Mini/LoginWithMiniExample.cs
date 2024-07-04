using RMC.Mini.Experimental.ContextLocators;
using RMC.Mini.Samples.Login.WithMini.Mini.View;
using UnityEngine;
using UnityEngine.Assertions;

namespace RMC.Mini.Samples.Login.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class LoginWithMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        //Mini does NOT require any MonoBehaviours ...
        //But here is an optional one to ease UI setup ...
        [SerializeField] 
        private LoginView _view;

        //  Unity Methods  --------------------------------
        protected void Start()
        {
            LoginMini loginMini = new LoginMini(_view);

            ContextWithLocator t;
            
            // This demo is rare. It needs a special type of context.
            Assert.IsNotNull(loginMini.Context as ContextWithLocator, 
                "This demo requires a ContextWithLocator.");
            
            loginMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}