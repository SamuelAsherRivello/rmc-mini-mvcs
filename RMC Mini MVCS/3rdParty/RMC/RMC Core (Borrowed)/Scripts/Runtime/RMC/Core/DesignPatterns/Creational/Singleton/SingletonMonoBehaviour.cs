using UnityEngine;

// ReSharper disable StaticMemberInGenericType
// ReSharper disable MemberCanBeProtected.Global

//Keep as "Borrowed" so it doesn't conflict with "RMC Core" (if user imports that, its optional)
namespace RMC.Core.Borrowed.DesignPatterns.Creational.Singletons
{
    //  Parent Class  --------------------------------------
    public abstract class SingletonMonoBehaviour : MonoBehaviour
    {
        //  Properties ------------------------------------
        public static bool IsShuttingDown { get; private set; } = false;

        //  Unity Methods   -------------------------------
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            IsShuttingDown = false;
        }

        private void OnApplicationQuit()
        {
            IsShuttingDown = true;
        }

        //  Other Methods ---------------------------------
        public virtual void OnInitialized()
        {
            // Override, if desired
        }
    }

    /// <summary>
    /// This class provides a generic singleton implementation for MonoBehaviour-based classes in Unity.
    /// It includes several common features for managing the singleton lifecycle, ensuring only one instance exists,
    /// and handling Unity-specific lifecycle events like play mode changes.
    /// </summary>
    public abstract class SingletonMonoBehaviour<T> : SingletonMonoBehaviour where T : MonoBehaviour
    {
        //  Logging ---------------------------------------
      
        private static readonly bool IsDebug = false;

        private static void DebugLog(string message)
        {
            if (!IsDebug)
            {
                return;
            }
            Debug.Log($"{DebugPrefix}{message}");
        }

        private static void DebugLogError(string message)
        {
            if (!IsDebug)
            {
                return;
            }
            Debug.LogError($"{DebugPrefix}{message}");
        }

        //  Properties ------------------------------------
        public static bool IsInstantiated => _Instance != null;

        public static T Instance
        {
            get
            {
                lock (_InstanceLock)
                {
                    // Avoid instance creation if the singleton is marked as shutting down
                    if (IsShuttingDown && Application.isPlaying)
                        return null;

                    // Return existing instance if found
                    if (_Instance != null)
                        return _Instance;

                    _IsInitializing = true;

                    // Search for any in-scene instance of T
                    var allInstances = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                    if (allInstances.Length == 1)
                    {
                        DebugLog($"Found exactly 1 {typeof(T)}");
                        _Instance = allInstances[0];
                    }
                    else if (allInstances.Length == 0)
                    {
                        DebugLog($"Found exactly no {typeof(T)}");
                        _Instance = new GameObject($"Singleton<{typeof(T)}>").AddComponent<T>();

                        // NOTE: Set the custom name in two spots on purpose
                        _Instance!.gameObject.name = CustomName;

                        DebugLog($"Create _Instance as {_Instance}");
                    }
                    else
                    {
                        DebugLog($"Found exactly {allInstances.Length} {typeof(T)}");
                        _Instance = allInstances[0];

                        // Destroy the duplicates
                        for (int i = 1; i < allInstances.Length; ++i)
                        {
                            DebugLogError($"Destroying duplicate {typeof(T)} on {allInstances[i].gameObject.name}");
                            Destroy(allInstances[i].gameObject);
                        }
                    }

                    _IsInitializing = false;
                    (_Instance as SingletonMonoBehaviour)?.OnInitialized();
                    return _Instance;
                }
            }
        }

        //  Fields ----------------------------------------
        private static T _Instance = null;
        private static readonly object _InstanceLock = new object();
        private static bool _IsInitializing = false;

        private static string DebugPrefix => $"[{CustomName}]: ";
        private static string CustomName => $"SingletonMonoBehaviour<{typeof(T).Name}>";

        //  Initialization  -------------------------------
        private static void ConstructIfNeeded(SingletonMonoBehaviour<T> inInstance)
        {
            lock (_InstanceLock)
            {
                if (_Instance == null && !_IsInitializing)
                {
                    DebugLog($"ConstructIfNeeded run for {typeof(T)}");
                    _Instance = inInstance as T;
                    (_Instance as SingletonMonoBehaviour)?.OnInitialized();
                }
                else if (_Instance != null && !_IsInitializing)
                {
                    DebugLogError($"Destroying duplicate {typeof(T)} on {inInstance.gameObject.name}");
                    Destroy(inInstance.gameObject);
                }
            }
        }

        //  Unity Methods   -------------------------------
        private void Awake()
        {
            ConstructIfNeeded(this);
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            // Unity requires DontDestroyOnLoad for root-level objects only
            gameObject.transform.parent = null;
            DontDestroyOnLoad(gameObject);

            // NOTE: Set the custom name in two spots on purpose
            if (_Instance != null)
            {
                _Instance!.gameObject.name = CustomName;
            }
        }

        public static void Dispose()
        {
            // Dispose only if the singleton is instantiated and not shutting down
            if (IsInstantiated && !IsShuttingDown)
            {
                // Find and destroy all instances of T in the scene (both active and inactive)
                //NOTE: This seems heavy-handed, but so far all tests pass, and it seems to work well at runtime
                var allInstances = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var instance in allInstances)
                {
                    DestroyImmediate(instance.gameObject);
                }

                _Instance = null; // Fully reset the singleton instance
            }
        }
    }
}
