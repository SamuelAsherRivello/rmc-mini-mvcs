
namespace RMC.Core.Architectures.Mini.Controller.Commands
{
	/// <summary>
	/// This Command is a stand-alone object containing
	/// a value of data.
	/// </summary>
	public abstract class ValueCommand<TValue> : ICommand
	{
		//  Fields ----------------------------------------
		private readonly TValue _value;

		
		//  Properties ------------------------------------
		public TValue Value { get { return _value; } }


		//  Initialization  -------------------------------
		public ValueCommand(TValue value)
		{
			_value = value;
		}
		
		
		//  Methods  --------------------------------------
	}
}