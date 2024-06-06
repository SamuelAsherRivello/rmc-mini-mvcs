using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class MultiSceneMiniExample02 : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        //Mini does NOT require any MonoBehaviours ...
        //But here is an optional one to ease UI setup ...
        [SerializeField] 
        private MultiSceneView02 _view;

        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log("MultiSceneMiniExample02.Start()");
            
            MultiSceneSimpleMini simpleMini;
            
            if (MultiSceneMiniSingleton.Instance == null || MultiSceneMiniSingleton.Instance.MultiSceneSimpleMini == null)
            {
                // COMMON: Create the mini as you typically do  
                simpleMini = new MultiSceneSimpleMini();
                
                // UNIQUE: Store the mini on the Singleton so you
                // can detect "has this scene been loaded before?"
                MultiSceneMiniSingleton.Instance.MultiSceneSimpleMini = simpleMini;
                MultiSceneMiniSingleton.Instance.MultiSceneSimpleMini.Initialize();
                
                Debug.Log("Create New Mini");
                MultiSceneMiniSingleton.Instance.MultiSceneSimpleMini.AddFeaturesForScene02(_view);
            }
            else
            {
                Debug.Log("Found Existing Mini");
                MultiSceneMiniSingleton.Instance.MultiSceneSimpleMini.AddFeaturesForScene02(_view);
            }
           

        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}