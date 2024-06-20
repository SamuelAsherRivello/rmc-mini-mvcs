namespace RMC.Core.Events
{
    /// <summary>
    /// Interface for observable events.
    /// </summary>
    public interface IRmcEvent
    {
        void AddListener(RmcEventHandler call, bool willRefreshImmediately = false);
        void RemoveListener(RmcEventHandler call);
        void Invoke();
        void OnValueChangedRefresh();
    }

    /// <summary>
    /// Interface for observable events with values.
    /// </summary>
    public interface IRmcEvent<T>
    {
        void AddListener(RmcEventHandler<T> call, bool willRefreshImmediately = false, T value = default(T));
        void RemoveListener(RmcEventHandler<T> call);
        void Invoke(T value);
        void OnValueChangedRefresh(T value);
    }

    /// <summary>
    /// Interface for observable events with two values.
    /// </summary>
    public interface IEvent<T, U>
    {
        void AddListener(RmcEventHandler<T, U> call, bool willRefreshImmediately = false, T oldValue = default(T), U newValue = default(U));
        void RemoveListener(RmcEventHandler<T, U> call);
        void Invoke(T oldValue, U newValue);
        void OnValueChangedRefresh(T oldValue, U newValue);
    }

    /// <summary>
    /// Delegate for observable events.
    /// </summary>
    public delegate void RmcEventHandler();

    /// <summary>
    /// Delegate for observable events with values.
    /// </summary>
    public delegate void RmcEventHandler<T>(T value);

    /// <summary>
    /// Delegate for observable events with two values.
    /// </summary>
    public delegate void RmcEventHandler<T, U>(T oldValue, U newValue);

    /// <summary>
    /// Base class for observable events without generic parameters.
    /// </summary>
    public class RmcEvent : IRmcEvent
    {
        // Common functionality for both generic and non-generic events
        private event RmcEventHandler _eventHandler;

        public void AddListener(RmcEventHandler call, bool willRefreshImmediately = false)
        {
            _eventHandler += call;
            if (willRefreshImmediately)
            {
                Invoke();
            }
        }

        public void RemoveListener(RmcEventHandler call)
        {
            _eventHandler -= call;
        }

        public void Invoke()
        {
            _eventHandler?.Invoke();
        }

        public virtual void OnValueChangedRefresh()
        {
            // This method should be overridden in subclasses if needed
            Invoke();
        }
    }

    /// <summary>
    /// The custom event for observable values.
    /// </summary>
    public class RmcEvent<T> : IRmcEvent<T>
    {
        // Fields ----------------------------------------
        private event RmcEventHandler<T> _typedEventHandler;

        // Methods ---------------------------------------
        public void AddListener(RmcEventHandler<T> call, bool willRefreshImmediately = false, T value = default(T))
        {
            _typedEventHandler += call;
            if (willRefreshImmediately)
            {
                Invoke(value);
            }
        }

        public void RemoveListener(RmcEventHandler<T> call)
        {
            _typedEventHandler -= call;
        }

        public void Invoke(T value)
        {
            _typedEventHandler?.Invoke(value);
        }

        public void OnValueChangedRefresh(T value)
        {
            Invoke(value);
        }
    }

    /// <summary>
    /// The custom event for observable values with two parameters.
    /// </summary>
    public class RmcEvent<T, U> : IEvent<T, U>
    {
        // Fields ----------------------------------------
        private event RmcEventHandler<T, U> _typedEventHandler;

        // Methods ---------------------------------------
        public void AddListener(RmcEventHandler<T, U> call, bool willRefreshImmediately = false, T oldValue = default(T), U newValue = default(U))
        {
            _typedEventHandler += call;
            if (willRefreshImmediately)
            {
                Invoke(oldValue, newValue);
            }
        }

        public void RemoveListener(RmcEventHandler<T, U> call)
        {
            _typedEventHandler -= call;
        }

        public void Invoke(T oldValue, U newValue)
        {
            _typedEventHandler?.Invoke(oldValue, newValue);
        }

        public void OnValueChangedRefresh(T oldValue, U newValue)
        {
            Invoke(oldValue, newValue);
        }
    }
}
