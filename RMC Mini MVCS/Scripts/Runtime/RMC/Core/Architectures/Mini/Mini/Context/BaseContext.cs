using RMC.Core.Architectures.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Context
{
	/// <summary>
	/// See <see cref="Context"/>
	/// </summary>
	public class BaseContext : IContext
	{
		//  Properties ------------------------------------
		public ICommandManager CommandManager { get { return _commandManager; } }
		public ModelLocator ModelLocator { get { return _modelLocator; } }
		
		
		//  Fields ----------------------------------------
		private readonly ICommandManager _commandManager;
		private readonly ModelLocator _modelLocator;
		
		
		//  Initialization  -------------------------------
		public BaseContext()
		{
			_modelLocator = new ModelLocator();
			_commandManager = new CommandManager(this);
		}
		   
		public virtual void Dispose()
		{
			// Must override
			throw new System.NotImplementedException();
		}
		
		//  Methods ---------------------------------------

	}
}