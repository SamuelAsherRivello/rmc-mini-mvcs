using System.Collections.Generic;

namespace RMC.Core.Architectures.Mini.Controller.Commands
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
	public class CommandManager : ICommandManager
	{
		//  Properties ------------------------------------
		
		//  Fields ----------------------------------------
		private IContext _context;
		
		//Invoke
		public delegate void InvokeCommandDelegate<TCommand>(TCommand e) where TCommand : ICommand;
		private delegate void InvokeCommandDelegate(ICommand e);
		private Dictionary<System.Type, InvokeCommandDelegate> _invokeCommandDelegates = new Dictionary<System.Type, InvokeCommandDelegate>();
		private Dictionary<System.Delegate, InvokeCommandDelegate> _invokeCommandDelegatesLookup = new Dictionary<System.Delegate, InvokeCommandDelegate>();

		//Undo
		public delegate void UndoCommandDelegate<TExecutableCommand>(TExecutableCommand e) where TExecutableCommand : IExecutableCommand;
		private delegate void UndoCommandDelegate(IExecutableCommand e);
		private Dictionary<System.Type, UndoCommandDelegate> _undoCommandDelegates = new Dictionary<System.Type, UndoCommandDelegate>();
		private Dictionary<System.Delegate, UndoCommandDelegate> _undoCommandDelegatesLookup = new Dictionary<System.Delegate, UndoCommandDelegate>();


		//  Initialization  -------------------------------
		public CommandManager(IContext context)
		{
			_context = context;
		}
        
        
		//  Methods ---------------------------------------
		public void InvokeCommand(ICommand command)
		{
			InvokeCommandDelegate del;
			if (_invokeCommandDelegates.TryGetValue(command.GetType(), out del))
			{
				// Call the HANDLER and pass the COMMAND
				del.Invoke(command);
			}
		}
		
		public bool ExecuteCommand(IExecutableCommand executableCommand)
		{
			// FIRST: Calls NO handler, yet calls Execute on the COMMAND
			bool didExecute = executableCommand.Execute(_context);
			
			// ALSO, Call the HANDLER and pass the COMMAND
			// The command may or may not be observed, that is ok.
			InvokeCommand(executableCommand);

			return didExecute;
		}
		
		public bool UndoCommand(IExecutableCommand executableCommand)
		{
			bool didUndo = false;
			
			if (executableCommand.CanUndo)
			{
				didUndo = executableCommand.Undo(_context);
				
				//NOTE: We Invoke() here regardless of didUndo. Sounds good because we should
				// Invoke() when UndoCommand is called && CanUndo== true, regardless of consequence
				UndoCommandDelegate undoCommandDelegate;
				if (_undoCommandDelegates.TryGetValue(executableCommand.GetType(), out undoCommandDelegate))
				{
					// Call the HANDLER and pass the COMMAND
					undoCommandDelegate.Invoke(executableCommand);
				}
			}
			
			return didUndo;
		}
		
		public void AddCommandListener<TCommand>(CommandManager.InvokeCommandDelegate<TCommand> invokeDelegate) where TCommand : ICommand
		{
			//Invoke
			AddCommandListenerImpl<TCommand>(invokeDelegate);
		}
		
		public void AddCommandListener<TExecutableCommand>(
			CommandManager.InvokeCommandDelegate<TExecutableCommand> invokeDelegate,
			CommandManager.UndoCommandDelegate<TExecutableCommand> undoDelegate) where TExecutableCommand : IExecutableCommand
		{

			//Invoke
			AddCommandListenerImpl<TExecutableCommand>(invokeDelegate);
			
			//Undo
			AddUndoCommandListenerImpl<TExecutableCommand>(undoDelegate);
		}

		private void AddCommandListenerImpl<TCommand>(CommandManager.InvokeCommandDelegate<TCommand> del) where TCommand : ICommand
		{
			if (_invokeCommandDelegatesLookup.ContainsKey(del))
			{
				return;
			}

			InvokeCommandDelegate internalDelegate = (e) => del((TCommand)e);
			_invokeCommandDelegatesLookup[del] = internalDelegate;

			InvokeCommandDelegate tempDel;
			if (_invokeCommandDelegates.TryGetValue(typeof(TCommand), out tempDel))
			{
				_invokeCommandDelegates[typeof(TCommand)] = tempDel += internalDelegate;
			}
			else
			{
				_invokeCommandDelegates[typeof(TCommand)] = internalDelegate;
			}
		}
		
		private void AddUndoCommandListenerImpl<TExecutableCommand>(CommandManager.UndoCommandDelegate<TExecutableCommand> undoCommandDelegate) where TExecutableCommand : IExecutableCommand
		{
			if (_invokeCommandDelegatesLookup.ContainsKey(undoCommandDelegate))
			{
				return;
			}

			UndoCommandDelegate undoCommandDelegateResult = (e) => undoCommandDelegate((TExecutableCommand)e);
			_undoCommandDelegatesLookup[undoCommandDelegate] = undoCommandDelegateResult;

			UndoCommandDelegate tempDel;
			if (_undoCommandDelegates.TryGetValue(typeof(TExecutableCommand), out tempDel))
			{
				_undoCommandDelegates[typeof(TExecutableCommand)] = tempDel += undoCommandDelegateResult;
			}
			else
			{
				_undoCommandDelegates[typeof(TExecutableCommand)] = undoCommandDelegateResult;
			}
		}
		
		public void RemoveCommandListener<TCommand>(CommandManager.InvokeCommandDelegate<TCommand> del) where TCommand : ICommand
		{
			InvokeCommandDelegate internalDelegate;

			if (_invokeCommandDelegatesLookup.TryGetValue(del, out internalDelegate))
			{
				InvokeCommandDelegate tempDel;
				if (_invokeCommandDelegates.TryGetValue(typeof(TCommand), out tempDel))
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
	}
}