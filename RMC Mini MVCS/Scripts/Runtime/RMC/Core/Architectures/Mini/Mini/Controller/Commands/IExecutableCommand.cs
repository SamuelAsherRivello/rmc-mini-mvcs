using RMC.Core.Architectures.Mini.Context;

namespace RMC.Core.Architectures.Mini.Controller.Commands
{
	/// <summary>
	/// Complex type of <see cref="ICommand"/>.
	///
	/// This type can be used with the <see cref="CommandManager"/> related to
	/// operations of Invoke(), AddCommandListener(), Execute(), Undo()
	/// 
	/// </summary>
	public interface IExecutableCommand : ICommand
	{
		/// <summary>
		/// </summary>
        
		//  Properties ----------------------------------------
		bool CanUndo { get; }
		
		//  Methods  --------------------------------------
		bool Execute(IContext context);
		bool Undo(IContext context);
	}
}
