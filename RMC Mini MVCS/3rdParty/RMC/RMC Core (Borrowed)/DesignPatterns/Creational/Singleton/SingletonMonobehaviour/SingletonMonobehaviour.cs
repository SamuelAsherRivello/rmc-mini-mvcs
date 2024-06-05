﻿using UnityEditor;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace RMC.Core.DesignPatterns.Creational.Singleton.CustomSingletonMonobehaviour
{

    public abstract class SingletonMonobehaviour : MonoBehaviour
    {
        private static bool _IsShuttingDown = false;
        
        protected static bool _MustResetInstance = false;
        
        public bool HasDontDestroyOnLoad
        {
            get
            {
                return (gameObject.hideFlags & HideFlags.DontSave) != HideFlags.DontSave;
            }
        }
        
        public static bool IsShuttingDown
        {
            get
            {
                return _IsShuttingDown;
            }
            internal set
            {
                _IsShuttingDown = value;
            }
        }
        
#if UNITY_EDITOR

        [InitializeOnLoadMethod]
        static void InitializeOnLoadMethod()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// This InitializeOnLoadMethod scheme is designed to
        /// Properly reset the IsShuttingDown each time play mode ends
        ///
        /// The IsShuttingDown helps prevent non-singletons in their OnDestroy()
        /// from accidentally calling singleton.Instantiate which causes issues
        /// 
        /// </summary>
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                //Used for destruction
                IsShuttingDown = false;
                
                //Used for construction
                _MustResetInstance = true;
            }
            else if (state == PlayModeStateChange.EnteredPlayMode)
            {
             
            }
            //Debug.Log(state);
        }
        
        //  Static Cleanup ------------------------------------------

        /// <summary>
        /// When using the optional https://docs.unity3d.com/Manual/ConfigurableEnterPlayMode.html
        /// This code properly clears out the instance ONCE when entering play mode
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemRegistration()
        {
            _MustResetInstance = true;
        }

        
#endif
    }

    public abstract class SingletonMonobehaviour<T> : SingletonMonobehaviour  where T : MonoBehaviour
    {
        
        /// <summary>
        /// When using the optional https://docs.unity3d.com/Manual/ConfigurableEnterPlayMode.html
        /// This code properly clears out the instance ONCE when entering play mode
        /// </summary>
        public static void CheckResetInstance()
        {
            if (_MustResetInstance)
            {
                _Instance = null;
                _MustResetInstance = false;
            }

        }
        
        //  Properties ------------------------------------------
        
        public static bool IsInstantiated
        {
            get
            {
                return _Instance != null;
            }
        }

        public static T Instance
        {
            get
            {
                if (!IsInstantiated)
                {
                    Instantiate();
                }
                return _Instance;
            }
            set
            {
                _Instance = value;
            }

        }

        //  Fields -------------------------------------------------
        private static T _Instance;
       
        public delegate void OnInstantiateCompletedDelegate(T instance);
        public static OnInstantiateCompletedDelegate OnInstantiateCompleted;
        


        //  Instantiation ------------------------------------------

        
        protected virtual void Awake()
        {
            //NOTSURE
            if (_Instance != null && _Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instantiate();
            }
        }

        public static T Instantiate()
        {
            
            CheckResetInstance();
            
            var instances = GameObject.FindObjectsByType<T>(FindObjectsSortMode.InstanceID);

            //NOTSURE
            if (instances.Length > 1)
            {
                //Where 0 is the newest and length is the oldest
                //Destroy all except oldest
                for (int i = 0; i < instances.Length-1; i++)
                {
                    var next = instances[i];
                    Debug.Log("Destroy : " + next.gameObject.GetInstanceID() + " and " + next.gameObject.name);
                    Destroy(next.gameObject);
                }
            }
 
            
            if (IsShuttingDown || !Application.isPlaying)
            {
                Debug.LogError("Must check IsShuttingDown before calling Instantiate/Instance.");
            }
            
            if (!IsInstantiated)
            {
                _Instance = GameObject.FindObjectOfType<T>();
        
                if (_Instance == null)
                {
                    GameObject go = new GameObject();
                    _Instance = go.AddComponent<T>();
                    go.name = _Instance.GetType().FullName;
                    
                }

                // Notify child scope
                (_Instance as SingletonMonobehaviour<T>).InstantiateCompleted();

                // Notify observing scope(s)
                if (OnInstantiateCompleted != null)
                {
                    OnInstantiateCompleted(_Instance);
                }
            }
            
            _Instance.gameObject.transform.parent = null;
            DontDestroyOnLoad(_Instance.gameObject);
            
            return _Instance;
        }


        //  Unity Methods ------------------------------------------
        
        /// <summary>
        /// Detect and solve corner case
        /// </summary>
        protected void OnApplicationQuit()
        {
            Destroy();
        }
        
        /// <summary>
        /// Detect and solve corner case
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (HasDontDestroyOnLoad)
            {
                return;
            }
            Destroy();
        }
        
        /// <summary>
        /// Detect and solve corner case
        /// </summary>
        public static void Destroy()
        {
            
            IsShuttingDown = true;

            if (IsInstantiated)
            {
                if (Application.isPlaying)
                {
                    Destroy(_Instance.gameObject);
                }
                else
                {
                    DestroyImmediate(_Instance.gameObject);
                }

                _Instance = null;
            }
        }

        //  Other Methods ------------------------------------------
        public virtual void InstantiateCompleted()
        {
            // Override, if desired
        }

    }
}