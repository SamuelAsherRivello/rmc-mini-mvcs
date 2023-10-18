using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace RMC.Core.Testing
{
    /// <summary>
    /// Load and destroy prefabs during Unit Testing.
    /// Doing this process error-free is non-trivial.
    /// </summary>
    public class PrefabManagerForTesting
    {
        private List<GameObject> _gameObjects = new List<GameObject>();

        public T LoadAndInstantiate<T>(string path) where T: Component
        {
            Vector3 position = Vector3.zero;
            GameObject prefab = Resources.Load<GameObject>(path);

           
            if (prefab == null)
            {
                throw new Exception($"LoadAndInstantiate() failed. " +
                                    $"Cannot find prefab at path {path}.");
            }
            
            GameObject prefabInstance = GameObject.Instantiate<GameObject>(prefab, position, Quaternion.identity);
            
            if (prefabInstance == null)
            {
                throw new Exception($"LoadAndInstantiate() failed. " +
                                    $"Cannot find prefabInstance at path {path}.");
            }
            
            _gameObjects.Add(prefabInstance);
            
            T instance = prefabInstance.GetComponent<T>();
            
            if (instance == null)
            {
#if UNITY_EDITOR
                path = AssetDatabase.GetAssetPath(prefab); //fuller path
#endif
                throw new Exception($"LoadAndInstantiate() failed. " +
                                    $"Cannot find instance of type {(typeof(T).Name)} at path {path}.");
            }

            return instance;
        }
        
        /// <summary>
        /// Wait enough for some common unity lifecycle events to occur
        /// </summary>
        /// <returns></returns>
        public IEnumerator WaitForUnityLifeCycle ()
        {
            // COMMENTS SHOW THE ORDER OF EXECUTION FROM FRAME 0
            
            // 1. Awake
            
            // 2. Wait
            yield return new WaitForEndOfFrame();
            
            // 3. Start, OnTriggerEnter, Etc...
            
            // 4. Wait
            yield return new WaitForEndOfFrame();
            
            // 5. Wait EXTRA - sometimes this is needed and sometimes not. Not sure why.
            for (int i = 0; i < 12; i++)
            {
                yield return new WaitForEndOfFrame(); //Keep 
            }
            
            // Done!
            
        }

        public void Clear()
        {
            foreach (GameObject go in _gameObjects)
            {
                GameObject.DestroyImmediate(go, true);
            }
            _gameObjects.Clear();
        }
    }
}