using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class Scene02_Game : MonoBehaviour
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
        private GameView _gameView;

        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log("Scene02_Game.Start()");
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
            mini.AddFeaturesForGameScene(_hudView, _gameView);
        }

        //  Event Handlers --------------------------------
    }
}