
using System;
using RMC.Core.Architectures.Mini.Samples.Calculator.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Calculator.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Calculator.Mini.View;
using RMC.Core.Architectures.Mini.Structure.Simple;

namespace RMC.Core.Architectures.Mini.Samples.Calculator.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Calculator Mini performs math operations
    /// based on UI input.
    /// </summary>
    public class CalculatorSimpleMini : ISimpleMiniMvcs
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        
        //  Fields ----------------------------------------
        private IContext _context;
        private CalculatorView _calculatorView;

        //  Initialization  -------------------------------
        public CalculatorSimpleMini(CalculatorView calculatorView)
        {
            _calculatorView = calculatorView;
        }
        
        public void Initialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                
                //Context
                _context = new Context();
                
                //Model
                CalculatorModel calculatorModel = new CalculatorModel();

                //View

                //Controller
                CalculatorController calculatorController = 
                    new CalculatorController(calculatorModel, _calculatorView);

                //Initialize 
                calculatorModel.Initialize(_context);       //Do 1st. Registers ModelLocator
                _calculatorView.Initialize(_context);       //Do 2nd, Uses ModelLocator
                calculatorController.Initialize(_context);  //Do last
            }
            
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        //  Other Methods ---------------------------------
        
        
        //  Event Handlers --------------------------------
    }
}
