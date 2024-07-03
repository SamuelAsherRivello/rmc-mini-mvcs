using RMC.Mini.Controller.Commands;
using RMC.Mini.Model;

//Keep As:RMC.Mini
namespace RMC.Mini
{
	/// <summary>
	/// See <see cref="Context"/>
	/// </summary>
	public class BaseContext : IContext
	{
		//  Properties ------------------------------------
		public ICommandManager CommandManager { get { return _commandManager; } }
		public Locator<IModel> ModelLocator { get { return _modelLocator; } }
		
		
		//  Fields ----------------------------------------
		private readonly ICommandManager _commandManager;
		private readonly Locator<IModel> _modelLocator;
		
		
		//  Initialization  -------------------------------
		public BaseContext()
		{
			_modelLocator = new Locator<IModel>();
			_commandManager = new CommandManager(this);
		}
		   
		public virtual void Dispose()
		{
			_modelLocator.Reset();
			_commandManager.Dispose();
		}
		
		//  Methods ---------------------------------------

	}
}