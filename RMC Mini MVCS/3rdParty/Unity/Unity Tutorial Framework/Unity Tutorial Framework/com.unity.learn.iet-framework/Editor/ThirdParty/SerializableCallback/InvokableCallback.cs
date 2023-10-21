using System;

namespace SerializableCallback
{
    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="TReturn"></typeparam>
    public class InvokableCallback<TReturn> : InvokableCallbackBase<TReturn>
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Func<TReturn> func;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <returns></returns>
        public TReturn Invoke()
        {
            return func();
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override TReturn Invoke(params object[] args)
        {
            return func();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                func = () => default(TReturn);
            }
            else
            {
                func = (System.Func<TReturn>)System.Delegate.CreateDelegate(typeof(System.Func<TReturn>), target, methodName);
            }
        }
    }

    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public class InvokableCallback<T0, TReturn> : InvokableCallbackBase<TReturn>
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Func<T0, TReturn> func;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public TReturn Invoke(T0 arg0)
        {
            return func(arg0);
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override TReturn Invoke(params object[] args)
        {
            return func((T0)args[0]);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                func = x => default(TReturn);
            }
            else
            {
                func = (System.Func<T0, TReturn>)System.Delegate.CreateDelegate(typeof(System.Func<T0, TReturn>), target, methodName);
            }
        }
    }

    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public class InvokableCallback<T0, T1, TReturn> : InvokableCallbackBase<TReturn>
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Func<T0, T1, TReturn> func;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public TReturn Invoke(T0 arg0, T1 arg1)
        {
            return func(arg0, arg1);
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override TReturn Invoke(params object[] args)
        {
            return func((T0)args[0], (T1)args[1]);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                func = (x, y) => default(TReturn);
            }
            else
            {
                func = (System.Func<T0, T1, TReturn>)System.Delegate.CreateDelegate(typeof(System.Func<T0, T1, TReturn>), target, methodName);
            }
        }
    }

    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public class InvokableCallback<T0, T1, T2, TReturn> : InvokableCallbackBase<TReturn>
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Func<T0, T1, T2, TReturn> func;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public TReturn Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            return func(arg0, arg1, arg2);
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override TReturn Invoke(params object[] args)
        {
            return func((T0)args[0], (T1)args[1], (T2)args[2]);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                func = (x, y, z) => default(TReturn);
            }
            else
            {
                func = (System.Func<T0, T1, T2, TReturn>)System.Delegate.CreateDelegate(typeof(System.Func<T0, T1, T2, TReturn>), target, methodName);
            }
        }
    }

    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public class InvokableCallback<T0, T1, T2, T3, TReturn> : InvokableCallbackBase<TReturn>
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Func<T0, T1, T2, T3, TReturn> func;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        public TReturn Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            return func(arg0, arg1, arg2, arg3);
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override TReturn Invoke(params object[] args)
        {
            return func((T0)args[0], (T1)args[1], (T2)args[2], (T3)args[3]);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableCallback(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                func = (x, y, z, w) => default(TReturn);
            }
            else
            {
                func = (System.Func<T0, T1, T2, T3, TReturn>)System.Delegate.CreateDelegate(typeof(System.Func<T0, T1, T2, T3, TReturn>), target, methodName);
            }
        }
    }
}
