using RMC.Core.Events;

namespace RMC.Core.Observables
{
    /// <summary>
    /// Wrapper where any changes to value of type
    /// <see cref="TValue"/> can be observable via events.
    /// </summary>
    public class Observable<TValue>
    {
        // Events ----------------------------------------
        public readonly IEvent<TValue, TValue> OnValueChanged;

        // Properties ------------------------------------
        /// <summary>
        /// Keep this property name as "Value".
        /// </summary>
        public TValue Value
        {
            set
            {
                _currentValue = OnValueChanging(_currentValue, value);
                OnValueChanged.Invoke(_previousValue, _currentValue);
            }
            get
            {
                return _currentValue;
            }
        }

        // Fields ----------------------------------------
        private TValue _currentValue;
        private TValue _previousValue;

        // Constructor Methods ---------------------------
        public Observable()
        {
            OnValueChanged = new RmcEvent<TValue, TValue>();
        }

        // Methods ---------------------------------------
        /// <summary>
        /// Refresh the observers by recalling Invoke()
        /// with the latest values.
        /// </summary>
        public void OnValueChangedRefresh()
        {
            OnValueChanged.OnValueChangedRefresh(_previousValue, _currentValue);
        }

        protected virtual TValue OnValueChanging(TValue previousValue, TValue newValue)
        {
            // Optional: Override method to gate/police the value changes
            _previousValue = previousValue;
            return newValue;
        }

        // Event Handlers --------------------------------
    }
}
