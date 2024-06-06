using System;
using RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.View;
using RMC.Core.Architectures.Mini.Structure.Simple;

namespace RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The MiniMvcs is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    /// </summary>
    public class RightSimpleMini: ISimpleMiniMvcs
    {
   
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        //  Fields ----------------------------------------
        private readonly RightView _rightView;
        private RightController _rightController;
        private MultipleMinisModel _multipleMinisModel;

        //  Initialization  -------------------------------
        public RightSimpleMini(IContext context, RightView rightView)
        {
            _rightView = rightView;
            Context = context;
        }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                
                //Model
                _multipleMinisModel = 
                    Context.ModelLocator.GetItem<MultipleMinisModel>();
                
                //View
                _rightView.Initialize(Context);
                
                //Controller
                _rightController = new RightController(
                    _multipleMinisModel, _rightView);
                _rightController.Initialize(Context);
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