using RMC.Core.Architectures.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Locators;
using RMC.Core.Architectures.Mini.Model;

//Keep as: RMC.Core.Architectures.Mini
namespace  RMC.Core.Architectures.Mini
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
			// Must override
			throw new System.NotImplementedException();
		}
		
		//  Methods ---------------------------------------

	}
}