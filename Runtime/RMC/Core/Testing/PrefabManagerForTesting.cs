using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            GameObject prefabInstance = GameObject.Instantiate<GameObject>(prefab, position, Quaternion.identity);
            
            _gameObjects.Add(prefabInstance);
            return prefabInstance.GetComponent<T>();
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
            for (int i = 0; i < 4; i++)
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