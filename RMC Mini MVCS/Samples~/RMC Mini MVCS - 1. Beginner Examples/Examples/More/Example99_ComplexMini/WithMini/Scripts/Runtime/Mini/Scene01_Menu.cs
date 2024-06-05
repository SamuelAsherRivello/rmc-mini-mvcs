using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class Scene01_Menu : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        //Mini does NOT require any MonoBehaviours ...
        //But here is an optional one to ease UI setup ...
        [SerializeField] 
        private HudView _hudView;
        
        //Mini does NOT require any MonoBehaviours ...
        //But here is an optional one to ease UI setup ...
        [SerializeField] 
        private MenuView _menuView;

        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log("Scene01_Menu.Start()");
            AddFeaturesForMenuScene();

        }

        //  Methods ---------------------------------------
        private void AddFeaturesForMenuScene()
        {
            if (ComplexMiniSingleton.Instance == null || 
                ComplexMiniSingleton.Instance.ComplexMini == null)
            {
                // Create the mini as you typically do  
                ComplexMiniSingleton.Instance.ComplexMini = new ComplexMini();
            
                // Store the mini on the ComplexMiniSingleton 
                ComplexMiniSingleton.Instance.ComplexMini.Initialize();
                
                Debug.Log("Create New Mini");
            }
            else
            {
                Debug.Log("Found Existing Mini");
            }
            
            ComplexMini mini = ComplexMiniSingleton.Instance.ComplexMini;
            mini.AddFeaturesForMenuScene(_hudView, _menuView);
        }


        //  Event Handlers --------------------------------
    }
}