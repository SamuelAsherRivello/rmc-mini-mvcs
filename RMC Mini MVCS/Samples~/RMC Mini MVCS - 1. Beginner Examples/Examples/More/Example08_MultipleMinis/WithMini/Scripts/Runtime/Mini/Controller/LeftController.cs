using System;
using RMC.Core.Architectures.Mini.Controller;
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
    public class LeftController: IController 
    {
    
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        
        //  Fields ----------------------------------------
        private readonly LeftView _leftView;
        private readonly MultipleMinisModel _multipleMinisModel;

        
        //  Initialization  -------------------------------
        public LeftController(MultipleMinisModel multipleMinisModel, 
            LeftView leftView)
        {
            _multipleMinisModel = multipleMinisModel;
            _leftView = leftView;
        }
        
        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                //View
                _leftView.OnIncrementCounter.AddListener(LeftView_OnIncrementCounter);
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
        private void LeftView_OnIncrementCounter()
        {
            RequireIsInitialized();
            _multipleMinisModel.Counter.Value++;
        }
    }
}