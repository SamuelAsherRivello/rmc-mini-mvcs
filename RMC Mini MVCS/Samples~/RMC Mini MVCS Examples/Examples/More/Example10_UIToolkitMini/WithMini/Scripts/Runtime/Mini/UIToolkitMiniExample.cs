using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class UIToolkitMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        // Mini does NOT require any MonoBehaviours ...
        // But here is an optional one to ease UI setup ...
        [SerializeField] 
        private UIToolkitView _view;

        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            UIToolkitMini uiToolkitMini = new UIToolkitMini(_view);
            uiToolkitMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}