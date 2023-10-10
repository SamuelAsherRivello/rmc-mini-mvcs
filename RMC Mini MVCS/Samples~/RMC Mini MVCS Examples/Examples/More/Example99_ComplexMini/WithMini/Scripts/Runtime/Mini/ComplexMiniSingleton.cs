using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Here is a ComplexMiniSingleton that can be used to access the <see cref="ComplexMini"/>.
    /// Replace this with your own custom implementation.
    /// </summary>
    public class ComplexMiniSingleton: MonoBehaviour
    {
        //  Fields ----------------------------------------
        public static ComplexMiniSingleton Instance { get; private set; }
        public ComplexMini ComplexMini { get; set; }
        
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