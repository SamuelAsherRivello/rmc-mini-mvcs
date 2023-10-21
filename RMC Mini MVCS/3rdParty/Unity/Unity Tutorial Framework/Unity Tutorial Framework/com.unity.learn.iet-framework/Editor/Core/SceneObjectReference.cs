using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Used to reference different Unity Objects.
    /// </summary>
    [Serializable]
    public class SceneObjectReference
    {
        [SerializeField]
        string m_SceneGuid;
        [SerializeField]
        string m_GameObjectGuid;
        [SerializeField]
        SerializedType m_SerializedComponentType = new SerializedType(null);
        [SerializeField]
        int m_ComponentIndex;
        [SerializeField]
        Object m_AssetObject;
        [SerializeField]
        GameObject m_Prefab;

        [NonSerialized]
        bool m_Initialized;
        [NonSerialized]
        Object m_ReferencedObject;

        SerializedProperty m_SceneGuidProperty;
        SerializedProperty m_GameObjectGuidProperty;
        //SerializedProperty m_ComponentTypeProperty;
        SerializedProperty m_SerializedComponentTypeProperty;
        SerializedProperty m_ComponentIndexProperty;
        SerializedProperty m_AssetObjectProperty;
        SerializedProperty m_PrefabProperty;

        /// <summary>
        /// The referenced Object.
        /// </summary>
        public Object ReferencedObject
        {
            get
            {
                if (!m_Initialized)
                {
                    Init();
                }
                return m_ReferencedObject;
            }
        }

        /// <summary>
        /// The referenced Object as GameObject.
        /// </summary>
        public GameObject ReferencedObjectAsGameObject => ReferencedObject as GameObject;

        /// <summary>
        /// The referenced Object as Component.
        /// </summary>
        public Component ReferencedObjectAsComponent => ReferencedObject as Component;

        /// <summary>
        /// Is this a GameObject reference.
        /// </summary>
        public bool IsGameObjectReference =>
            !string.IsNullOrEmpty(m_GameObjectGuid) && m_Prefab == null && m_SerializedComponentType.Type == null && m_AssetObject == null;

        /// <summary>
        /// Is this a Component reference.
        /// </summary>
        public bool IsComponentReference =>
            !string.IsNullOrEmpty(m_GameObjectGuid) && m_Prefab == null && m_SerializedComponentType.Type != null && m_AssetObject == null;

        /// <summary>
        /// Is this an Asset reference.
        /// </summary>
        public bool IsAssetReference =>
            string.IsNullOrEmpty(m_GameObjectGuid) && m_Prefab == null && m_SerializedComponentType.Type == null && m_AssetObject != null;

        /// <summary>
        /// Is this a Prefab reference.
        /// </summary>
        public bool IsPrefabReference =>
            string.IsNullOrEmpty(m_GameObjectGuid) && m_Prefab != null && m_SerializedComponentType.Type == null && m_AssetObject == null;

        /// <summary>
        /// Is the reference resolved, that is, pointing to a valid object.
        /// </summary>
        public bool ReferenceResolved =>
            m_AssetObject != null || ReferencedObject != null || string.IsNullOrEmpty(m_GameObjectGuid);

        /// <summary>
        /// The Scene the referenced GameObject/Component belongs to, if this is a GameObject/Component reference.
        /// </summary>
        public SceneAsset ReferenceScene =>
            AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(m_SceneGuid));

        /// <summary>
        /// Default constructs this class to uninitialized state.
        /// </summary>
        public SceneObjectReference()
        {
        }

        /// <summary>
        /// Constructs from a SerializedProperty.
        /// </summary>
        /// <param name="property"></param>
        public SceneObjectReference(SerializedProperty property)
        {
            m_SceneGuidProperty = property.FindPropertyRelative("m_SceneGuid");
            m_GameObjectGuidProperty = property.FindPropertyRelative("m_GameObjectGuid");
            m_SerializedComponentTypeProperty = property.FindPropertyRelative("m_SerializedComponentType.m_TypeName");
            m_ComponentIndexProperty = property.FindPropertyRelative("m_ComponentIndex");
            m_AssetObjectProperty = property.FindPropertyRelative("m_AssetObject");
            m_PrefabProperty = property.FindPropertyRelative("m_Prefab");

            m_SceneGuid = m_SceneGuidProperty.stringValue;
            m_GameObjectGuid = m_GameObjectGuidProperty.stringValue;
            m_SerializedComponentType = new SerializedType(Type.GetType(m_SerializedComponentTypeProperty.stringValue));
            m_ComponentIndex = m_ComponentIndexProperty.intValue;
            m_AssetObject = m_AssetObjectProperty.objectReferenceValue;
            m_Prefab = m_PrefabProperty.objectReferenceValue as GameObject;

            Init();
        }

        void Init()
        {
            m_Initialized = true;
            m_ReferencedObject = null;

            EditorSceneManager.sceneOpened += OnSceneOpened;
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (m_AssetObject != null)
            {
                m_ReferencedObject = m_AssetObject;
                return;
            }

            if (m_Prefab != null)
            {
                m_ReferencedObject = m_Prefab;
                return;
            }

            if (string.IsNullOrEmpty(m_GameObjectGuid))
            {
                return;
            }

            var guidComponent = SceneObjectGuidManager.Instance.GetComponent(m_GameObjectGuid);
            if (guidComponent == null)
            {
                return;
            }
            GameObject go;
            m_ReferencedObject = go = guidComponent.gameObject;
            if (m_SerializedComponentType.Type == null)
                return;
            var componentType = m_SerializedComponentType.Type;
            if (componentType == null)
                return;
            var components = go.GetComponents(componentType);

            if (components.Length == 0)
            {
                Debug.LogWarning("Component " + componentType + " not found.");
                ResetReference();
                SaveProperties();
                return;
            }

            if (m_ComponentIndex + 1 > components.Length)
            {
                Debug.LogWarning("Component with given index " + m_ComponentIndex + " did not exist");
                m_ComponentIndexProperty.intValue = m_ComponentIndex = 0;
            }
            m_ReferencedObject = components[m_ComponentIndex];
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ResetInitialization();
        }

        void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            ResetInitialization();
        }

        /// <summary>
        /// Resets/updates this reference to a new object.
        /// </summary>
        /// <param name="newObject"></param>
        public void Update(Object newObject)
        {
            ResetInitialization();
            ResetReference();
            if (newObject == null)
            {
                SaveProperties();
                return;
            }

            SceneObjectGuid guidComponent;

            GameObject go;
            if (newObject is Component)
            {
                Component component = newObject as Component;
                go = component.gameObject;
                guidComponent = go.GetComponent<SceneObjectGuid>();
                m_SerializedComponentType = new SerializedType(component.GetType());
                m_ComponentIndex = Array.IndexOf(go.GetComponents(component.GetType()), component);
            }
            else if (newObject is GameObject)
            {
                go = newObject as GameObject;
                if (PrefabUtility.IsPartOfPrefabAsset(go))
                {
                    m_Prefab = go;
                    SaveProperties();
                    return;
                }
                guidComponent = go.GetComponent<SceneObjectGuid>();
            }
            else
            {
                m_ReferencedObject = m_AssetObject = newObject;
                SaveProperties();
                return;
            }

            if (guidComponent == null)
            {
                guidComponent = go.AddComponent<SceneObjectGuid>();
                Undo.RegisterCreatedObjectUndo(guidComponent, "Created GUID component");
            }

            m_GameObjectGuid = guidComponent.Id;
            m_SceneGuid = GetSceneId(go);
            if (string.IsNullOrEmpty(m_SceneGuid))
            {
                Debug.LogError("The scene needs to be saved");
                return;
            }

            SaveProperties();
        }

        void ResetReference()
        {
            m_SceneGuid = m_GameObjectGuid = null;
            m_SerializedComponentType = new SerializedType(null);
            m_ComponentIndex = 0;
            m_AssetObject = null;
            m_ReferencedObject = null;
            m_Prefab = null;
        }

        void ResetInitialization()
        {
            m_Initialized = false;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void SaveProperties()
        {
            if (m_GameObjectGuidProperty == null)
            {
                return;
            }
            m_GameObjectGuidProperty.stringValue = m_GameObjectGuid;
            m_SceneGuidProperty.stringValue = m_SceneGuid;
            m_SerializedComponentTypeProperty.stringValue = m_SerializedComponentType.Type == null
                ? ""
                : m_SerializedComponentType.Type.AssemblyQualifiedName;
            m_ComponentIndexProperty.intValue = m_ComponentIndex;
            m_AssetObjectProperty.objectReferenceValue = m_AssetObject;
            m_PrefabProperty.objectReferenceValue = m_Prefab;
        }

        string GetSceneId(GameObject gameObject) => AssetDatabase.AssetPathToGUID(gameObject.scene.path);
    }
}
