namespace RMC.Core.Architectures.Mini.Controller.Commands
{
    /// <summary>
    /// The CommandManager allows to observe and invoke
    /// <see cref="ICommand"/> instances
    /// </summary>
    public interface ICommandManager
    {
        //  Properties ------------------------------------
        
        //  Methods  --------------------------------------
        void AddCommandListener<TCommand>(
            CommandManager.InvokeCommandDelegate<TCommand> del) where TCommand : ICommand;
        
        void RemoveCommandListener<TCommand>(
            CommandManager.InvokeCommandDelegate<TCommand> del) where TCommand : ICommand;
        
        void InvokeCommand(ICommand command);
    
    }
}