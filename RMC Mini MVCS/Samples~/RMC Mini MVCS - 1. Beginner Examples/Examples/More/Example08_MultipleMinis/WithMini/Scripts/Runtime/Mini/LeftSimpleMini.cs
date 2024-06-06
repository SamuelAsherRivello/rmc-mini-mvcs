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
    public class LeftSimpleMini: ISimpleMiniMvcs
    {
   
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        //  Fields ----------------------------------------
        private readonly LeftView _leftView;
        private MultipleMinisModel _multipleMinisModel;
        private LeftController _leftController;
        
        //  Initialization  -------------------------------
        public LeftSimpleMini(IContext context, LeftView leftView)
        {
            _leftView = leftView;
            Context = context;
        }
        
        public void Initialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                
                //Model
                //OPTION 1: Create it, pass it in
                //OPTION 2: Create it in the RightMini
                //OPTION 3: Create it in the LeftMini
                _multipleMinisModel = new MultipleMinisModel();
                _multipleMinisModel.Initialize(Context);
                
                //View
                _leftView.Initialize(Context);
                
                //Controller
                _leftController = new LeftController(_multipleMinisModel,
                    _leftView);
                _leftController.Initialize(Context);
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