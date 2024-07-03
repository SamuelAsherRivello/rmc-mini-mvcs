using System;
using System.Collections.Generic;

namespace RMC.Mini.Controller.Commands
{
    /// <summary>
    /// The CommandManager allows to observe and invoke
    /// <see cref="ICommand"/> instances.
    ///
    /// Summary
    ///		* Call AddCommandListener() to observe a command.
    ///		* Call Invoke(). This executes nothing, but notifies any listeners.
    ///		* Call Execute(). This executes and notifies any listeners.
    ///		* Call Undo(). This undoes something and notifies any listeners.
    ///
    /// Related Types
    ///		* ICommand           relates to Invoke(), AddCommandListener()
    ///		* IExecutableCommand relates Invoke(), AddCommandListener(), Execute(), Undo()
    /// 
    /// </summary>
    public class CommandManager : ICommandManager, IDisposable
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        private IContext _context;
        
        //Invoke
        public delegate void InvokeCommandDelegate<TCommand>(TCommand e) where TCommand : ICommand;
        private delegate void InvokeCommandDelegate(ICommand e);
        private Dictionary<Type, InvokeCommandDelegate> _invokeCommandDelegates = new Dictionary<Type, InvokeCommandDelegate>();
        private Dictionary<Delegate, InvokeCommandDelegate> _invokeCommandDelegatesLookup = new Dictionary<Delegate, InvokeCommandDelegate>();

        //Undo
        public delegate void UndoCommandDelegate<TExecutableCommand>(TExecutableCommand e) where TExecutableCommand : IExecutableCommand;
        private delegate void UndoCommandDelegate(IExecutableCommand e);
        private Dictionary<Type, UndoCommandDelegate> _undoCommandDelegates = new Dictionary<Type, UndoCommandDelegate>();
        private Dictionary<Delegate, UndoCommandDelegate> _undoCommandDelegatesLookup = new Dictionary<Delegate, UndoCommandDelegate>();

        //  Initialization  -------------------------------
        public CommandManager(IContext context)
        {
            _context = context;
        }
        
        //  Methods ---------------------------------------
        
        /// <summary>
        /// Invokes the specified command.
        /// </summary>
        public void InvokeCommand(ICommand command)
        {
            if (_invokeCommandDelegates.TryGetValue(command.GetType(), out InvokeCommandDelegate del))
            {
                del.Invoke(command);
            }
        }

        public int GetCommandListenerCount()
        {
            return _invokeCommandDelegates.Count;
        }

        /// <summary>
        /// Executes the specified executable command.
        /// </summary>
        public bool ExecuteCommand(IExecutableCommand executableCommand)
        {
            bool didExecute = executableCommand.Execute(_context);
            InvokeCommand(executableCommand);
            return didExecute;
        }

        /// <summary>
        /// Undoes the specified executable command.
        /// </summary>
        public bool UndoCommand(IExecutableCommand executableCommand)
        {
            bool didUndo = false;

            if (executableCommand.CanUndo)
            {
                didUndo = executableCommand.Undo(_context);

                if (_undoCommandDelegates.TryGetValue(executableCommand.GetType(), out UndoCommandDelegate undoCommandDelegate))
                {
                    undoCommandDelegate.Invoke(executableCommand);
                }
            }

            return didUndo;
        }

        /// <summary>
        /// Adds a command listener for the specified command type.
        /// </summary>
        public void AddCommandListener<TCommand>(InvokeCommandDelegate<TCommand> invokeDelegate) where TCommand : ICommand
        {
            AddCommandListenerImpl(invokeDelegate);
        }

        /// <summary>
        /// Adds command and undo listeners for the specified executable command type.
        /// </summary>
        public void AddCommandListener<TExecutableCommand>(
            InvokeCommandDelegate<TExecutableCommand> invokeDelegate,
            UndoCommandDelegate<TExecutableCommand> undoDelegate) where TExecutableCommand : IExecutableCommand
        {
            AddCommandListenerImpl(invokeDelegate);
            AddUndoCommandListenerImpl(undoDelegate);
        }

        private void AddCommandListenerImpl<TCommand>(InvokeCommandDelegate<TCommand> del) where TCommand : ICommand
        {
            if (_invokeCommandDelegatesLookup.ContainsKey(del))
            {
                return;
            }

            InvokeCommandDelegate internalDelegate = (e) => del((TCommand)e);
            _invokeCommandDelegatesLookup[del] = internalDelegate;

            if (_invokeCommandDelegates.TryGetValue(typeof(TCommand), out InvokeCommandDelegate tempDel))
            {
                _invokeCommandDelegates[typeof(TCommand)] = tempDel += internalDelegate;
            }
            else
            {
                _invokeCommandDelegates[typeof(TCommand)] = internalDelegate;
            }
        }
        
        private void AddUndoCommandListenerImpl<TExecutableCommand>(UndoCommandDelegate<TExecutableCommand> undoCommandDelegate) where TExecutableCommand : IExecutableCommand
        {
            if (_invokeCommandDelegatesLookup.ContainsKey(undoCommandDelegate))
            {
                return;
            }

            UndoCommandDelegate undoCommandDelegateResult = (e) => undoCommandDelegate((TExecutableCommand)e);
            _undoCommandDelegatesLookup[undoCommandDelegate] = undoCommandDelegateResult;

            if (_undoCommandDelegates.TryGetValue(typeof(TExecutableCommand), out UndoCommandDelegate tempDel))
            {
                _undoCommandDelegates[typeof(TExecutableCommand)] = tempDel += undoCommandDelegateResult;
            }
            else
            {
                _undoCommandDelegates[typeof(TExecutableCommand)] = undoCommandDelegateResult;
            }
        }

        /// <summary>
        /// Removes a command listener for the specified command type.
        /// </summary>
        public void RemoveCommandListener<TCommand>(InvokeCommandDelegate<TCommand> del) where TCommand : ICommand
        {
            if (_invokeCommandDelegatesLookup.TryGetValue(del, out InvokeCommandDelegate internalDelegate))
            {
                if (_invokeCommandDelegates.TryGetValue(typeof(TCommand), out InvokeCommandDelegate tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        _invokeCommandDelegates.Remove(typeof(TCommand));
                    }
                    else
                    {
                        _invokeCommandDelegates[typeof(TCommand)] = tempDel;
                    }
                }

                _invokeCommandDelegatesLookup.Remove(del);
            }
        }

        /// <summary>
        /// Disposes the CommandManager and cleans up resources.
        /// </summary>
        public void Dispose()
        {
            _invokeCommandDelegates.Clear();
            _invokeCommandDelegatesLookup.Clear();
            _undoCommandDelegates.Clear();
            _undoCommandDelegatesLookup.Clear();
        }
    }
}
