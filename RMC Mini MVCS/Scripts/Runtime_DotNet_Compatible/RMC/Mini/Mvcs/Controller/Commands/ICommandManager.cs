using System;

namespace RMC.Mini.Controller.Commands
{
    /// <summary>
    /// The CommandManager allows to observe and invoke
    /// <see cref="ICommand"/> instances
    /// </summary>
    public interface ICommandManager : IDisposable
    {
        //  Properties ------------------------------------
        
        //  Methods  --------------------------------------
        void AddCommandListener<TCommand>(
            CommandManager.InvokeCommandDelegate<TCommand> del) where TCommand : ICommand;
        
        int GetCommandListenerCount();
        
        void InvokeCommand(ICommand command);
        
        void RemoveCommandListener<TCommand>(
            CommandManager.InvokeCommandDelegate<TCommand> del) where TCommand : ICommand;
    
    }
}