using UnityEditor;
using UnityEngine.Events;

namespace RMC.Core.DesignPatterns.Creational.Singleton.CustomSingleton
{
    public interface ISingletonParent
    {
        //  Properties ------------------------------------
        
        //  General Methods -------------------------------
        void OnInstantiatedBase();
        void OnInstantiatedChild();
    }

    public abstract class SingletonParent : System.Object, ISingletonParent
    {
        //  Properties ------------------------------------
        public UnityEvent OnInstantiated = new UnityEvent();

        //  General Methods -------------------------------
        void ISingletonParent.OnInstantiatedBase()
        {
            (this as ISingletonParent).OnInstantiatedChild();
            OnInstantiated.Invoke();

        }

        void ISingletonParent.OnInstantiatedChild()
        {
            //Override 
        }

        public static void OnEnteredEditMode()
        {
            //Do nothing here
            //Static cannot be overridden per se
            //You can REDECLARE this method in a child 
            //REDECLARATION will be called
        }

        public static void  OnEnteredPlayMode()
        {
            //Do nothing here
            //Static cannot be overridden per se
            //You can REDECLARE this method in a child 
            //REDECLARATION will be called
        }
        
#if UNITY_EDITOR

        [InitializeOnLoadMethod]
        static void InitializeOnLoadMethod()
        {
            EditorApplication.playModeStateChanged += _OnPlayModeStateChanged;
        }

        /// <summary>
        /// This InitializeOnLoadMethod scheme is designed to
        /// Properly reset the IsShuttingDown each time play mode ends
        ///
        /// The IsShuttingDown helps prevent non-singletons in their OnDestroy()
        /// from accidentally calling singleton.Instantiate which causes issues
        /// 
        /// </summary>
        private static void _OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                OnEnteredEditMode();
            }
            else if (state == PlayModeStateChange.EnteredPlayMode)
            {
                OnEnteredPlayMode();
            }
        }
#endif


    }


    /// <summary>
    /// Custom Singleton Pattern: See <a href="https://en.wikipedia.org/wiki/Singleton_pattern">https://en.wikipedia.org/wiki/Singleton_pattern</a>
    ///
    /// This version is designed for situations: Non-Monobehaviour, Non-ScriptableObject
    /// </summary>
    public abstract class Singleton<T> : SingletonParent where T : SingletonParent, new()
    {
        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------
        private static T _instance = default(T);
        static readonly object _padlock = new object();
        
        
        //  Initialization Methods-------------------------
        public static bool IsInstantiated
        {
            get
            {
                return _instance != null;
            }
        }
        
        public static T Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                        (_instance as ISingletonParent).OnInstantiatedBase();
                    }
 
                    return _instance;
                }
            }
        }
        
        protected static void Uninstantiate()
        {
            _instance = null;
        }
        
        public static T Instantiate()
        {
            return Instance;
        }
   
        //  General Methods -------------------------------
		
        
        //  Event Handlers --------------------------------
    }
}