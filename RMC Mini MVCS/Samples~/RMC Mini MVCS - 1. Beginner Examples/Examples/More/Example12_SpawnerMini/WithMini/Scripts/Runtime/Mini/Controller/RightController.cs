using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class RightController: IController, IDisposable
    {
    
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        
        //  Fields ----------------------------------------
        private readonly SpawnerMiniModel _spawnerMiniModel;
        private readonly RightView _rightView;

        
        //  Initialization  -------------------------------
        public RightController(SpawnerMiniModel spawnerMiniModel, 
            RightView rightView)
        {
            _spawnerMiniModel = spawnerMiniModel;
            _rightView = rightView;
        }
        
        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                //Model
                _spawnerMiniModel.Counter.OnValueChanged.AddListener(
                    Counter_OnValueChanged);
                
                //Trigger refresh so this freshly spawned controller
                //captures any stale values from the model and updates the view
                _spawnerMiniModel.Counter.OnValueChangedRefresh();
            }
        }
        

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        public void Dispose()
        {
            RequireIsInitialized();
            
            _spawnerMiniModel.Counter.OnValueChanged.RemoveListener(
                Counter_OnValueChanged);
       
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