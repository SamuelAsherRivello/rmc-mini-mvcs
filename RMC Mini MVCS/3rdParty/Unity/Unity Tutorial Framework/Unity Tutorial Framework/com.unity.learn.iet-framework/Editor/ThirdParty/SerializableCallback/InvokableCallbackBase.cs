namespace SerializableCallback
{
    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="TReturn"></typeparam>
    public abstract class InvokableCallbackBase<TReturn>
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public abstract TReturn Invoke(params object[] args);
    }
}
