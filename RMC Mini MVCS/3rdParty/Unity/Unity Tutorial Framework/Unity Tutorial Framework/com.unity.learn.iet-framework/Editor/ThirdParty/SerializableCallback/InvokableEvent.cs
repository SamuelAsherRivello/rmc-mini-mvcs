using System;

namespace SerializableCallback
{
    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    public class InvokableEvent : InvokableEventBase
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public System.Action action;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public void Invoke()
        {
            action();
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        public override void Invoke(params object[] args)
        {
            action();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                action = () => {};
            }
            else
            {
                action = (System.Action)System.Delegate.CreateDelegate(typeof(System.Action), target, methodName);
            }
        }
    }

    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    public class InvokableEvent<T0> : InvokableEventBase
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Action<T0> action;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="arg0"></param>
        public void Invoke(T0 arg0)
        {
            action(arg0);
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        public override void Invoke(params object[] args)
        {
            action((T0)args[0]);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                action = x => {};
            }
            else
            {
                action = (System.Action<T0>)System.Delegate.CreateDelegate(typeof(System.Action<T0>), target, methodName);
            }
        }
    }

    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public class InvokableEvent<T0, T1> : InvokableEventBase
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Action<T0, T1> action;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        public void Invoke(T0 arg0, T1 arg1)
        {
            action(arg0, arg1);
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        public override void Invoke(params object[] args)
        {
            action((T0)args[0], (T1)args[1]);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                action = (x, y) => {};
            }
            else
            {
                action = (System.Action<T0, T1>)System.Delegate.CreateDelegate(typeof(System.Action<T0, T1>), target, methodName);
            }
        }
    }

    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class InvokableEvent<T0, T1, T2> : InvokableEventBase
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Action<T0, T1, T2> action;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            action(arg0, arg1, arg2);
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        public override void Invoke(params object[] args)
        {
            action((T0)args[0], (T1)args[1], (T2)args[2]);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                action = (x, y, z) => {};
            }
            else
            {
                action = (System.Action<T0, T1, T2>)System.Delegate.CreateDelegate(typeof(System.Action<T0, T1, T2>), target, methodName);
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
    public class InvokableEvent<T0, T1, T2, T3> : InvokableEventBase
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Action<T0, T1, T2, T3> action;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            action(arg0, arg1, arg2, arg3);
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="args"></param>
        public override void Invoke(params object[] args)
        {
            action((T0)args[0], (T1)args[1], (T2)args[2], (T3)args[3]);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        public InvokableEvent(object target, string methodName)
        {
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                action = (x, y, z, w) => {};
            }
            else
            {
                action = (System.Action<T0, T1, T2, T3>)System.Delegate.CreateDelegate(typeof(System.Action<T0, T1, T2, T3>), target, methodName);
            }
        }
    }
}
