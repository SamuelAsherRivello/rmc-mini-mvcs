using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.DataBindingMini.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.DataBindingMini.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.DataBindingMini.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.DataBindingMini.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The MiniMvcs is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    /// </summary>
    public class DataBindingMini: IMiniMvcs
    {
   
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        //  Fields ----------------------------------------
        private readonly LeftView _leftView;
        private readonly RightView _rightView;
        private DataBindingMiniModel _dataBindingMiniModel;
        private DataBindingController _dataBindingController;
        
        //  Initialization  -------------------------------
        public DataBindingMini(IContext context, 
            LeftView leftView,
            RightView rightView)
        {
            _leftView = leftView;
            _rightView = rightView;
            Context = context;
        }
        
        public void Initialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                
                //Model
                _dataBindingMiniModel = new DataBindingMiniModel();
                _dataBindingMiniModel.Initialize(Context);
                
                //View
                _leftView.Initialize(Context);
                _rightView.Initialize(Context);
                
                //Controller
                _dataBindingController = new DataBindingController(
                    _dataBindingMiniModel,
                    _leftView,
                    _rightView);
                
                _dataBindingController.Initialize(Context);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }

        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}