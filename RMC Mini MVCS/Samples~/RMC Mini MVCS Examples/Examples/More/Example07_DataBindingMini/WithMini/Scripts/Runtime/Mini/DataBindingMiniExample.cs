using RMC.Core.Architectures.Mini.Samples.DataBindingMini.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.DataBindingMini.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class DataBindingMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        [SerializeField] 
        private LeftView _leftView;

        [SerializeField] 
        private RightView _rightView;

        private Context.Context _context;
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            
            // KEY CONCEPT: Create the Context here, OUTSIDE
            // and pass the SAME value to EACH Mini
            _context = new Context.Context();
            
            // Mini
            DataBindingMini dataBindingMini = new DataBindingMini(
                _context, 
                _leftView,
                _rightView);
            
            dataBindingMini.Initialize();
            
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}