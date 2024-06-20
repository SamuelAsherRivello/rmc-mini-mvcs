using System;
using RMC.Mini.Controller;
using RMC.Mini.Samples.DataBindingMini.WithMini.Mini.Model;
using RMC.Mini.Samples.DataBindingMini.WithMini.Mini.View;

namespace RMC.Mini.Samples.DataBindingMini.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class DataBindingController: IController 
    {
    
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        
        //  Fields ----------------------------------------
        private readonly LeftView _leftView;
        private readonly RightView _rightView;
        private readonly DataBindingMiniModel _dataBindingMiniModel;

        
        //  Initialization  -------------------------------
        public DataBindingController(DataBindingMiniModel dataBindingMiniModel, 
            LeftView leftView,
            RightView rightView)
        {
            _dataBindingMiniModel = dataBindingMiniModel;
            _leftView = leftView;
            _rightView = rightView;
        }
        
        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
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
    }
}