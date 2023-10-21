
using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Calculator.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Calculator.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.Calculator.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class CalculatorController : IController
    {

        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        
        //  Fields ----------------------------------------
        private CalculatorModel _calculatorModel;
        private CalculatorView _calculatorView;
        
        
        //  Initialization  -------------------------------
        public CalculatorController(CalculatorModel calculatorModel, CalculatorView calculatorView)
        {
            _calculatorModel = calculatorModel;
            _calculatorView = calculatorView;
        }
        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                //
                _calculatorView.OnAdd.AddListener(View_OnAdd);
                _calculatorView.OnReset.AddListener(View_OnReset);
                View_OnReset();

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
        private void View_OnReset()
        {
            RequireIsInitialized();
            
            _calculatorModel.A.Value = 0;
            _calculatorModel.B.Value = 0;
            _calculatorModel.Result.Value = 0;
            
            //Alternative Solution (Not Needed)
            //Context.CommandManager.InvokeCommand(new ResetCommand());
        }

        
        private void View_OnAdd()
        {
            RequireIsInitialized();

            _calculatorModel.A.Value = Int32.Parse(_calculatorView.AInputField.text);
            _calculatorModel.B.Value = Int32.Parse(_calculatorView.BInputField.text);
            _calculatorModel.Result.Value = _calculatorModel.A.Value + _calculatorModel.B.Value;
        }
    }
}







