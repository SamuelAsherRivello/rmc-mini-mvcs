using System;

namespace SerializableCallback
{
    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    public abstract class SerializableEventBase : SerializableCallbackBase
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public InvokableEventBase invokable;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public override void ClearCache()
        {
            base.ClearCache();
            invokable = null;
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <returns></returns>
        protected InvokableEventBase GetPersistentMethod()
        {
            Type[] types = new Type[ArgTypes.Length];
            Array.Copy(ArgTypes, types, ArgTypes.Length);

            Type genericType = null;
            switch (types.Length)
            {
                case 0:
                    genericType = typeof(InvokableEvent);
                    break;
                case 1:
                    genericType = typeof(InvokableEvent<>).MakeGenericType(types);
                    break;
                case 2:
                    genericType = typeof(InvokableEvent<,>).MakeGenericType(types);
                    break;
                case 3:
                    genericType = typeof(InvokableEvent<, ,>).MakeGenericType(types);
                    break;
                case 4:
                    genericType = typeof(InvokableEvent<, , ,>).MakeGenericType(types);
                    break;
                default:
                    throw new ArgumentException(types.Length + "args");
            }
            return Activator.CreateInstance(genericType, new object[] { target, methodName }) as InvokableEventBase;
        }
    }
}
