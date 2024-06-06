using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Here is a Singleton that can be used to access the Mini MVCS.
    /// Replace this with your own custom implementation.
    /// </summary>
    public class MultiSceneMiniSingleton: MonoBehaviour
    {
        //  Fields ----------------------------------------
        public static MultiSceneMiniSingleton Instance { get; private set; }
        public MultiSceneSimpleMini MultiSceneSimpleMini { get; set; }
        
        //  Initialization  -------------------------------

        
        //  Unity Methods ---------------------------------
        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        //  Event Handlers --------------------------------
    }
}