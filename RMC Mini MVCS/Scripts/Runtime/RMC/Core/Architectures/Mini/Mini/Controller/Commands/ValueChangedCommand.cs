
namespace RMC.Core.Architectures.Mini.Controller.Commands
{
	/// <summary>
	/// This Command is a stand-alone object containing
	/// the before and after value during a data change
	/// </summary>
	public abstract class ValueChangedCommand<TValue> : ICommand
	{
		//  Properties ------------------------------------
		
		//  Fields ----------------------------------------
        
		//  Initialization  -------------------------------

		//  Methods  --------------------------------------
		
		public TValue PreviousValue { get { return _previousValue; } }
		public TValue CurrentValue { get { return _currentValue; } }

		private readonly TValue _previousValue;
		private readonly TValue _currentValue;

		public ValueChangedCommand(TValue previousValue, TValue currentValue)
		{
			_previousValue = previousValue;
			_currentValue = currentValue;
		}

	}
}