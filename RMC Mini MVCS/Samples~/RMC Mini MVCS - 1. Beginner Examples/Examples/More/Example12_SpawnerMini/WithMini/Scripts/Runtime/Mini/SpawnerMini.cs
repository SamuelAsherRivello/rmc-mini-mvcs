using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The MiniMvcs is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    /// </summary>
    public class SpawnerMini: IMiniMvcs
    {
   
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        //  Fields ----------------------------------------
        private readonly LeftView _leftView;
        private SpawnerMiniModel _spawnerMiniModel;
        private LeftController _leftController;
        private RightController _rightController;
        private RightView _rightViewPrefab;

        
        //  Initialization  -------------------------------
        public SpawnerMini(IContext context, LeftView leftView, RightView rightViewPrefab)
        {
            _leftView = leftView;
            _rightViewPrefab = rightViewPrefab;
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
                _spawnerMiniModel = new SpawnerMiniModel();
                _spawnerMiniModel.Initialize(Context);
                
                //View
                _leftView.Initialize(Context);
                
                //Controller
                _leftController = new LeftController(_spawnerMiniModel,
                    _leftView, _rightViewPrefab);
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