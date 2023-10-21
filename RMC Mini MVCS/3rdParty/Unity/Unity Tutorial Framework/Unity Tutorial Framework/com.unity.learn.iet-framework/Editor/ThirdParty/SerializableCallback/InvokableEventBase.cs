namespace SerializableCallback
{
    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    public abstract class InvokableEventBase
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        public abstract void Invoke(params object[] args);
    }
}
