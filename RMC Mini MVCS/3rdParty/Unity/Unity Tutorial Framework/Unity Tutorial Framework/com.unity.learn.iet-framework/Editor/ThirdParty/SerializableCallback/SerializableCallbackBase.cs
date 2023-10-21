using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SerializableCallback
{
    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    /// <typeparam name="TReturn"></typeparam>
    public abstract class SerializableCallbackBase<TReturn> : SerializableCallbackBase
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public InvokableCallbackBase<TReturn> func;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public override void ClearCache()
        {
            base.ClearCache();
            func = null;
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <returns></returns>
        protected InvokableCallbackBase<TReturn> GetPersistentMethod()
        {
            Type[] types = new Type[ArgTypes.Length + 1];
            Array.Copy(ArgTypes, types, ArgTypes.Length);
            types[types.Length - 1] = typeof(TReturn);

            Type genericType = null;
            switch (types.Length)
            {
                case 1:
                    genericType = typeof(InvokableCallback<>).MakeGenericType(types);
                    break;
                case 2:
                    genericType = typeof(InvokableCallback<,>).MakeGenericType(types);
                    break;
                case 3:
                    genericType = typeof(InvokableCallback<, ,>).MakeGenericType(types);
                    break;
                case 4:
                    genericType = typeof(InvokableCallback<, , ,>).MakeGenericType(types);
                    break;
                case 5:
                    genericType = typeof(InvokableCallback<, , , ,>).MakeGenericType(types);
                    break;
                default:
                    throw new ArgumentException(types.Length + "args");
            }
            return Activator.CreateInstance(genericType, new object[] { target, methodName }) as InvokableCallbackBase<TReturn>;
        }
    }

    /// <summary> An inspector-friendly serializable function </summary>
    [System.Serializable]
    public abstract class SerializableCallbackBase : ISerializationCallbackReceiver
    {
        /// <summary> Target object </summary>
        public Object target { get { return _target; } set { _target = value; ClearCache(); } }
        /// <summary> Target method name </summary>
        public string methodName { get { return _methodName; } set { _methodName = value; ClearCache(); } }
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public object[] Args { get { return args != null ? args : args = _args.Select(x => x.GetValue()).ToArray(); } }
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public object[] args;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Type[] ArgTypes { get { return argTypes != null ? argTypes : argTypes = _args.Select(x => Arg.RealType(x.argType)).ToArray(); } }
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Type[] argTypes;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public bool dynamic { get { return _dynamic; } set { _dynamic = value; ClearCache(); } }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        [SerializeField] protected Object _target;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        [SerializeField] protected string _methodName;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        [SerializeField] protected Arg[] _args;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        [SerializeField] protected bool _dynamic;
#pragma warning disable 0414
        [SerializeField] private string _typeName;
#pragma warning restore 0414

        [SerializeField] private bool dirty;

#if UNITY_EDITOR
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        protected SerializableCallbackBase()
        {
            _typeName = base.GetType().AssemblyQualifiedName;
        }

#endif

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public virtual void ClearCache()
        {
            argTypes = null;
            args = null;
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        /// <param name="dynamic"></param>
        /// <param name="args"></param>
        public void SetMethod(Object target, string methodName, bool dynamic, params Arg[] args)
        {
            _target = target;
            _methodName = methodName;
            _dynamic = dynamic;
            _args = args;
            ClearCache();
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        protected abstract void Cache();

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (dirty) { ClearCache(); dirty = false; }
#endif
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            _typeName = base.GetType().AssemblyQualifiedName;
#endif
        }
    }

    /// <summary>
    /// https://github.com/Siccity/SerializableCallback
    /// </summary>
    [System.Serializable]
    public struct Arg
    {
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public enum ArgType
        {
            /// <summary>
            /// https://github.com/Siccity/SerializableCallback
            /// </summary>
            Unsupported,
            /// <summary>
            /// https://github.com/Siccity/SerializableCallback
            /// </summary>
            Bool,
            /// <summary>
            /// https://github.com/Siccity/SerializableCallback
            /// </summary>
            Int,
            /// <summary>
            /// https://github.com/Siccity/SerializableCallback
            /// </summary>
            Float,
            /// <summary>
            /// https://github.com/Siccity/SerializableCallback
            /// </summary>
            String,
            /// <summary>
            /// https://github.com/Siccity/SerializableCallback
            /// </summary>
            Object
        }
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public bool boolValue;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public int intValue;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public float floatValue;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public string stringValue;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public Object objectValue;
        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        public ArgType argType;

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return GetValue(argType);
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetValue(ArgType type)
        {
            switch (type)
            {
                case ArgType.Bool:
                    return boolValue;
                case ArgType.Int:
                    return intValue;
                case ArgType.Float:
                    return floatValue;
                case ArgType.String:
                    return stringValue;
                case ArgType.Object:
                    return objectValue;
                default:
                    return null;
            }
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type RealType(ArgType type)
        {
            switch (type)
            {
                case ArgType.Bool:
                    return typeof(bool);
                case ArgType.Int:
                    return typeof(int);
                case ArgType.Float:
                    return typeof(float);
                case ArgType.String:
                    return typeof(string);
                case ArgType.Object:
                    return typeof(Object);
                default:
                    return null;
            }
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ArgType FromRealType(Type type)
        {
            if (type == typeof(bool)) return ArgType.Bool;
            else if (type == typeof(int)) return ArgType.Int;
            else if (type == typeof(float)) return ArgType.Float;
            else if (type == typeof(String)) return ArgType.String;
            else if (type == typeof(Object)) return ArgType.Object;
            else  return ArgType.Unsupported;
        }

        /// <summary>
        /// https://github.com/Siccity/SerializableCallback
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSupported(Type type)
        {
            return FromRealType(type) != ArgType.Unsupported;
        }
    }
}
