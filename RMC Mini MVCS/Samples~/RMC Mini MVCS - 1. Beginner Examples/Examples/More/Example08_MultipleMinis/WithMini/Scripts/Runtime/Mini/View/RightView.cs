using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.MultipleMinis.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class RightView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        
        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public IContext Context { get; private set; }
        
        public Text BodyText { get { return _bodyText;}}
        
        //  Fields ----------------------------------------

        [SerializeField] 
        private Text _bodyText;

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Context = context;
                
                //
                Context.CommandManager.AddCommandListener<CounterChangedCommand>(
                    OnCounterChangedCommand);
            }
        }

        
        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Unity Methods ---------------------------------
        protected void OnDestroy()
        {
            Context?.CommandManager?.RemoveCommandListener<CounterChangedCommand>(
                OnCounterChangedCommand);
        }

        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        private void OnCounterChangedCommand(CounterChangedCommand counterChangedCommand)
        {
            RequireIsInitialized();

            _bodyText.text = $"Counter = {counterChangedCommand.CurrentValue}";
        }
    }
}