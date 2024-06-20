using UnityEngine.Events;


//////////////////////////////////////////////
///
///
///
/// This is a hack. its a copy of the Observable.cs so that this project
/// doesn't need to depend on the asdmdef where Observable lives.
///
///
//////////////////////////////////////////////
namespace RMC.Core.IO
{
    /// <summary>
    /// The <see cref="UnityEvent"/> for <see cref="Observable{T}"/>.
    /// </summary>
    public class LDSObservableUnityEvent<T, U> : UnityEvent<T, U>
    {
        //  Fields ----------------------------------------
        private LDSObservable<T> _observable;
        
        //  Constructor Methods ---------------------------
        public LDSObservableUnityEvent(LDSObservable<T> observable) : base()
        {
            _observable = observable;
        }
        
        
        //  Methods ---------------------------------------
        public new void AddListener(UnityAction<T, U> call)
        {
            AddListener(call, false);
        }
        
        /// <summary>
        /// Consuming scope can optionally 'catch up' to the existing
        /// value by immediately refreshing value to the observer
        /// </summary>
        /// <param name="call"></param>
        /// <param name="willRefreshImmediately"></param>
        public void AddListener(UnityAction<T, U> call, bool willRefreshImmediately )
        {
            base.AddListener(call);
            if (willRefreshImmediately)
            {
                _observable.OnValueChangedRefresh();
            }
        }
    }
    
    /// <summary>
    /// Wrapper where any changes to value of type
    /// <see cref="TValue"/> can be observable via events
    /// </summary>
    public class LDSObservable<TValue> 
    {
        //  Events ----------------------------------------
        public readonly LDSObservableUnityEvent<TValue, TValue> OnValueChanged;

        //  Properties ------------------------------------
        
        /// <summary>
        /// Keep this property name as "Value"
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

        //  Fields ----------------------------------------
        private TValue _currentValue;
        private TValue _previousValue;

        //  Constructor Methods ---------------------------

        public LDSObservable()
        {
            OnValueChanged = new LDSObservableUnityEvent<TValue,TValue>(this);
        }

        //  Methods ---------------------------------------
        
        /// <summary>
        /// Refresh the observers by recalling Invoke()
        /// with the latest values
        /// </summary>
        public void OnValueChangedRefresh()
        {
            OnValueChanged.Invoke(_previousValue, _currentValue);
        }
        
        protected virtual TValue OnValueChanging(TValue previousValue, TValue newValue)
        {
            //Optional: Override method to gate/police the value changes
            _previousValue = previousValue;
            return newValue;
        }
        
        

        //  Event Handlers --------------------------------
  
    }

}