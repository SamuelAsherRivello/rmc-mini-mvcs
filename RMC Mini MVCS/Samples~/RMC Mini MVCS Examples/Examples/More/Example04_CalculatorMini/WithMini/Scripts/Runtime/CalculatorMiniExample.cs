using RMC.Core.Architectures.Mini.Samples.Calculator.Mini;
using RMC.Core.Architectures.Mini.Samples.Calculator.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Calculator
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class CalculatorMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------
        [SerializeField] private CalculatorView _calculatorView;
        
        //  Initialization  -------------------------------
        
        
        //  Unity Methods   -------------------------------
        protected void Start ()
        {
            
            CalculatorMini calculatorMini = new CalculatorMini(_calculatorView);
            calculatorMini.Initialize();
        }
        
        
        //  Other Methods ---------------------------------
        
        
        //  Event Handlers --------------------------------
    }
}