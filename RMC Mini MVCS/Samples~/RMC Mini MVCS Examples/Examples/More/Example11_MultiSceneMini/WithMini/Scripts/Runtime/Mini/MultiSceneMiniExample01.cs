using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class MultiSceneMiniExample01 : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        //Mini does NOT require any MonoBehaviours ...
        //But here is an optional one to ease UI setup ...
        [SerializeField] 
        private MultiSceneView01 _view;

        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log("MultiSceneMiniExample01.Start()");

            MultiSceneMini mini;
            
            if (MultiSceneMiniSingleton.Instance == null || MultiSceneMiniSingleton.Instance.MultiSceneMini == null)
            {
                // COMMON: Create the mini as you typically do  
                mini = new MultiSceneMini();
                
                // UNIQUE: Store the mini on the Singleton so you
                // can detect "has this scene been loaded before?"
                MultiSceneMiniSingleton.Instance.MultiSceneMini = mini;
                MultiSceneMiniSingleton.Instance.MultiSceneMini.Initialize();
                
                Debug.Log("Create New Mini");
                MultiSceneMiniSingleton.Instance.MultiSceneMini.AddFeaturesForScene01(_view);
            }
            else
            {
                Debug.Log("Found Existing Mini");
                MultiSceneMiniSingleton.Instance.MultiSceneMini.AddFeaturesForScene01(_view);
            }
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}