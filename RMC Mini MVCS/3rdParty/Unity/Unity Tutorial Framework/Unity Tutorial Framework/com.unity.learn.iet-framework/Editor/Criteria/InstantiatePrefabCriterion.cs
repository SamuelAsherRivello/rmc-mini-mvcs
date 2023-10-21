using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that a specific prefab is instantiated.
    /// </summary>
    public class InstantiatePrefabCriterion : Criterion
    {
        [SerializeField]
        GameObject m_PrefabParent;

        [SerializeField]
        FuturePrefabInstanceCollection m_FuturePrefabInstances = new FuturePrefabInstanceCollection();

        // InstanceID's of existing GameObject prefab instance roots we want to ignore
        HashSet<int> m_ExistingPrefabInstances = new HashSet<int>();

        // InstanceID of GameObject prefab instance root that initially completed this criterion
        int m_PrefabInstance;

        /// <summary>
        /// Prefab parent.
        /// </summary>
        public GameObject PrefabParent
        {
            get => m_PrefabParent;
            set
            {
                m_PrefabParent = value;
                OnValidate();
            }
        }

        /// <summary>
        /// Sets future prefab instances.
        /// </summary>
        /// <param name="prefabParents"></param>
        public void SetFuturePrefabInstances(IList<UnityObject> prefabParents)
        {
            var futurePrefabInstances = prefabParents.Select(prefabParent => new FuturePrefabInstance(prefabParent));
            m_FuturePrefabInstances.SetItems(futurePrefabInstances.ToList());
            OnValidate();
        }

        /// <summary>
        /// Runs validation logic.
        /// </summary>
        protected override void OnValidate()
        {
            base.OnValidate();

            if (m_PrefabParent == null)
                return;

            // Ensure prefab parent is infact a prefab parent
            if (PrefabUtility.GetPrefabAssetType(m_PrefabParent) != PrefabAssetType.NotAPrefab)
            {
                // Ensure prefab parent is the prefab root
                var prefabRoot = m_PrefabParent.transform.root.gameObject;
                if (m_PrefabParent != prefabRoot)
                    m_PrefabParent = prefabRoot;
            }
            else
            {
                Debug.LogWarning("Prefab parent must either be a prefab parent or a prefab instance.");
                m_PrefabParent = null;
            }

            // Prevent aliasing of future reference whenever the last item is copied
            var count = m_FuturePrefabInstances.Count;
            if (count >= 2)
            {
                var last = m_FuturePrefabInstances[count - 1];
                var secondLast = m_FuturePrefabInstances[count - 2];
                if (last.FutureReference == secondLast.FutureReference)
                    last.FutureReference = null;
            }

            var updateFutureReferenceNames = false;
            var futurePrefabInstanceIndex = -1;

            foreach (var futurePrefabInstance in m_FuturePrefabInstances)
            {
                futurePrefabInstanceIndex++;

                // Destroy future reference if prefab parent is null or it changed
                var prefabParent = futurePrefabInstance.PrefabParent;
                var previousPrefabParent = futurePrefabInstance.PreviousPrefabParent;
                futurePrefabInstance.PreviousPrefabParent = prefabParent;
                if (prefabParent == null || (previousPrefabParent != null && prefabParent != previousPrefabParent))
                {
                    if (futurePrefabInstance.FutureReference != null)
                    {
                        DestroyImmediate(futurePrefabInstance.FutureReference, true);
                        futurePrefabInstance.FutureReference = null;
                    }
                }

                if (prefabParent == null)
                    continue;

                // Ensure future prefab parent is infact a prefab parent
                if (PrefabUtility.GetPrefabAssetType(prefabParent) != PrefabAssetType.NotAPrefab)
                {
                    // Find root game object of future prefab parent
                    GameObject futurePrefabParentRoot = null;
                    if (prefabParent is GameObject)
                    {
                        var gameObject = (GameObject)prefabParent;
                        futurePrefabParentRoot = gameObject.transform.root.gameObject;
                    }
                    else if (prefabParent is Component)
                    {
                        var component = (Component)prefabParent;
                        futurePrefabParentRoot = component.transform.root.gameObject;
                    }

                    // Ensure prefab parent and future prefab parent belong to the same prefab
                    if (futurePrefabParentRoot == m_PrefabParent)
                    {
                        // Create new future reference if it doesn't exist yet
                        if (futurePrefabInstance.FutureReference == null)
                        {
                            var referenceName = string.Format("{0}: {1} ({2})", futurePrefabInstanceIndex + 1,
                                prefabParent.name, prefabParent.GetType().Name);
                            futurePrefabInstance.FutureReference = CreateFutureObjectReference(referenceName);
                            updateFutureReferenceNames = true;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Prefab parent and future prefab parent have different prefab objects.");
                        futurePrefabInstance.PrefabParent = null;
                    }
                }
                else
                {
                    Debug.LogWarning("Future prefab parent must be either a prefab parent or a prefab instance.");
                    futurePrefabInstance.PrefabParent = null;
                }
            }

            if (updateFutureReferenceNames)
                UpdateFutureObjectReferenceNames();
        }

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public override void StartTesting()
        {
            base.StartTesting();
            // Record existing prefab instances
            m_ExistingPrefabInstances.Clear();
            foreach (var gameObject in UnityObject.FindObjectsOfType<GameObject>())
            {
                if (PrefabUtilityShim.GetCorrespondingObjectFromSource(gameObject) != null)
                {
                    var prefabInstanceRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
                    m_ExistingPrefabInstances.Add(prefabInstanceRoot.GetInstanceID());
                }
            }

            Selection.selectionChanged += OnSelectionChanged;

            if (IsCompleted)
                EditorApplication.update += OnUpdateWhenCompleted;

            UpdateCompletion();
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
            m_ExistingPrefabInstances.Clear();

            Selection.selectionChanged -= OnSelectionChanged;
            EditorApplication.update -= OnUpdateWhenCompleted;
        }

        void OnSelectionChanged()
        {
            if (IsCompleted)
                return;

            foreach (var gameObject in Selection.gameObjects)
            {
                if (PrefabUtilityShim.GetCorrespondingObjectFromSource(gameObject) != null)
                {
                    var prefabInstanceRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
                    if (prefabInstanceRoot == gameObject && m_ExistingPrefabInstances.Add(prefabInstanceRoot.GetInstanceID()))
                        OnPrefabInstantiated(prefabInstanceRoot);
                }
            }
        }

        void OnPrefabInstantiated(GameObject prefabInstanceRoot)
        {
            if (m_PrefabParent == null)
                return;

            if (PrefabUtilityShim.GetCorrespondingObjectFromSource(prefabInstanceRoot) == m_PrefabParent)
            {
                foreach (var component in prefabInstanceRoot.GetComponentsInChildren<Component>())
                {
                    UpdateFutureReferences(component);

                    if (component is Transform)
                        UpdateFutureReferences(component.gameObject);
                }

                m_PrefabInstance = prefabInstanceRoot.GetInstanceID();

                UpdateCompletion();
            }
        }

        void OnUpdateWhenCompleted()
        {
            if (!IsCompleted)
            {
                EditorApplication.update -= OnUpdateWhenCompleted;
                return;
            }

            UpdateCompletion();
        }

        bool EvaluateCompletionInternal()
        {
            if (m_PrefabInstance == 0)
                return false;

            var prefabObject = EditorUtility.InstanceIDToObject(m_PrefabInstance);
            if (prefabObject == null)
            {
                m_ExistingPrefabInstances.Remove(m_PrefabInstance);
                m_PrefabInstance = 0;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            var willBeCompleted = EvaluateCompletionInternal();
            if (!IsCompleted && willBeCompleted)
                EditorApplication.update += OnUpdateWhenCompleted;

            return willBeCompleted;
        }

        void UpdateFutureReferences(UnityObject prefabInstance)
        {
            UnityObject prefabParent = PrefabUtilityShim.GetCorrespondingObjectFromSource(prefabInstance);
            foreach (var futurePrefabInstance in m_FuturePrefabInstances)
            {
                if (futurePrefabInstance.PrefabParent == prefabParent)
                    futurePrefabInstance.FutureReference.SceneObjectReference.Update(prefabInstance);
            }
        }

        /// <summary>
        /// Returns FutureObjectReference for this Criterion.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<FutureObjectReference> GetFutureObjectReferences()
        {
            return m_FuturePrefabInstances
                .Select(futurePrefabInstance => futurePrefabInstance.FutureReference)
                .Where(futurePrefabInstance => futurePrefabInstance != null);
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            if (m_PrefabParent == null)
                return false;

            Selection.activeObject = PrefabUtility.InstantiatePrefab(m_PrefabParent);

            return true;
        }

        /// <summary>
        /// Future prefab instance.
        /// </summary>
        [Serializable]
        public class FuturePrefabInstance
        {
            [SerializeField]
            UnityObject m_PrefabParent;

            UnityObject m_PreviousPrefabParent;

            [SerializeField, HideInInspector]
            FutureObjectReference m_FutureReference;

            /// <summary>
            /// Prefab parent.
            /// </summary>
            public UnityObject PrefabParent { get => m_PrefabParent; set => m_PrefabParent = value; }

            /// <summary>
            /// Previous prefab parent.
            /// </summary>
            public UnityObject PreviousPrefabParent { get => m_PreviousPrefabParent; set => m_PreviousPrefabParent = value; }

            /// <summary>
            /// Future reference.
            /// </summary>
            public FutureObjectReference FutureReference { get => m_FutureReference; set => m_FutureReference = value; }

            /// <summary>
            /// Construcs with a specific prefab parent.
            /// </summary>
            /// <param name="prefabParent"></param>
            public FuturePrefabInstance(UnityObject prefabParent)
            {
                m_PrefabParent = prefabParent;
            }
        }

        [Serializable]
        class FuturePrefabInstanceCollection : CollectionWrapper<FuturePrefabInstance>
        {
        }
    }
}
