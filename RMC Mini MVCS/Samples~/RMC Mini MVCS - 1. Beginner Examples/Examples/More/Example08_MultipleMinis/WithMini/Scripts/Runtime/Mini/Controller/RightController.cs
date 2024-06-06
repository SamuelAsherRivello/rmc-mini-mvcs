using System;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class RightController: IController 
    {
    
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        
        //  Fields ----------------------------------------
        private readonly MultipleMinisModel _multipleMinisModel;
        private readonly RightView _rightView;

        
        //  Initialization  -------------------------------
        public RightController(MultipleMinisModel multipleMinisModel, 
            RightView rightView)
        {
            _multipleMinisModel = multipleMinisModel;
            _rightView = rightView;
        }
        
        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                //Model
                _multipleMinisModel.Counter.OnValueChanged.AddListener(
                    Counter_OnValueChanged);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }

        //  Dispose Methods --------------------------------
        public virtual void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
        private void Counter_OnValueChanged(int previousValue, int currentValue)
        {
            RequireIsInitialized();
            
            //Option 1: Update the View directly. More coupled
            //_rightView.BodyText.text = $"Counter = {currentValue}";
            
            //Option 2: Update the View indirectly. Less coupled
            Context.CommandManager.InvokeCommand(
                new CounterChangedCommand(previousValue, currentValue));
            
        }
    }
}