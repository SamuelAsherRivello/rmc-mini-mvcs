using RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class MultipleMinisExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        [SerializeField] 
        private LeftView _leftView;

        [SerializeField] 
        private RightView _rightView;

        private Context _context;
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            
            // KEY CONCEPT: Create the Context here, OUTSIDE
            // and pass the SAME value to EACH Mini
            _context = new Context();
            
            // LEFT
            LeftSimpleMini leftSimpleMini = new LeftSimpleMini(_context, _leftView);
            leftSimpleMini.Initialize();
            
            // LEFT
            RightSimpleMini rightSimpleMini = new RightSimpleMini(_context, _rightView);
            rightSimpleMini.Initialize();
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}