using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Clock.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class ClockView: IView
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //
                Context.CommandManager.AddCommandListener<TimeChangedCommand>(
                    OnTimeChangedCommand);
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

        private void OnTimeChangedCommand(TimeChangedCommand timeChangedCommand)
        {
            RequireIsInitialized();
            
            // For simplicity, the 'rendering' of our view is just a debug log.
            Debug.Log($"Elapsed Time: {timeChangedCommand.CurrentValue} seconds");
        }

    }
}