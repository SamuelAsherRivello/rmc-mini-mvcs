using System;
using RMC.Core.Utilities.Testing;
using RMC.Mini.View;
using RMC.Mini.Samples.Clock.WithMini.Mini.Controller.Commands;
using UnityEngine;

namespace RMC.Mini.Samples.Clock.WithMini.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class ClockView: IView
    {
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
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void OnTimeChangedCommand(TimeChangedCommand timeChangedCommand)
        {
            RequireIsInitialized();
            
            if (TestRunnerUtilities.IsRunningUnitTests())
            {
                //Show no console logging when unit testing
                //So there is less noise and distraction
                return;
            }
            
            // For simplicity, the 'rendering' of our view is just a debug log.
            Debug.Log($"Elapsed Time: {timeChangedCommand.CurrentValue} seconds");
        }
    }
}
