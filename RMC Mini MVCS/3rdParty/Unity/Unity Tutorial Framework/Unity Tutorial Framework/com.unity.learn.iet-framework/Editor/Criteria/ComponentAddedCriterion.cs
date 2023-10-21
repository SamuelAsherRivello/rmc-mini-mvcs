using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that specific Components are added to a GameObject.
    /// </summary>
    public class ComponentAddedCriterion : Criterion
    {
        [SerializeField, FormerlySerializedAs("targetGameObject")]
        ObjectReference m_TargetGameObject;

        [SerializeField, FormerlySerializedAs("requiredComponents")]
        SerializedTypeCollection m_RequiredComponents = new SerializedTypeCollection();

        /// <summary>
        /// Returns the target GameObject.
        /// </summary>
        public GameObject TargetGameObject
        {
            get
            {
                if (m_TargetGameObject == null)
                    return null;
                return m_TargetGameObject.SceneObjectReference.ReferencedObjectAsGameObject;
            }
            set
            {
                if (m_TargetGameObject == null)
                    m_TargetGameObject = new ObjectReference();
                m_TargetGameObject.SceneObjectReference.Update(value);
            }
        }

        /// <summary>
        /// Returns the required Components.
        /// </summary>
        public IList<Type> RequiredComponents
        {
            get
            {
                return m_RequiredComponents.Select(typeAndFutureReference => typeAndFutureReference.SerializedType.Type)
                    .ToList();
            }
            set
            {
                var items = value.Select(type => new TypeAndFutureReference(new SerializedType(type))).ToList();
                m_RequiredComponents.SetItems(items);
                OnValidate();
            }
        }

        /// <summary>
        /// Starts testing of the criterion.
        /// </summary>
        public override void StartTesting()
        {
            base.StartTesting();
            UpdateCompletion();

            EditorApplication.update += UpdateCompletion;
        }

        /// <summary>
        /// Stops testing of the criterion.
        /// </summary>
        public override void StopTesting()
        {
            base.StopTesting();
            EditorApplication.update -= UpdateCompletion;
        }

        /// <summary>
        /// Destroys unused future reference assets and updates future references.
        /// </summary>
        protected override void OnValidate()
        {
            // Destroy unused future reference assets
            base.OnValidate();

            // Update future references
            var requiredComponentsIndex = 0;
            foreach (var typeAndFutureReference in m_RequiredComponents)
            {
                requiredComponentsIndex++;

                var type = typeAndFutureReference.SerializedType.Type;
                if (type == null)
                    continue;

                if (typeAndFutureReference.FutureReference == null)
                    typeAndFutureReference.FutureReference = CreateFutureObjectReference();

                typeAndFutureReference.FutureReference.ReferenceName = string.Format("{0}: {1}",
                    requiredComponentsIndex, type.FullName);
            }

            if (requiredComponentsIndex != 0)
                UpdateFutureObjectReferenceNames();
        }

        /// <summary>
        /// Evaluates if the criterion is completed.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateCompletion()
        {
            var gameObject = TargetGameObject;
            if (gameObject == null)
                return false;

            if (RequiredComponents.Count() == 0)
                return true;

            foreach (var type in RequiredComponents)
            {
                if (type == null)
                {
                    Debug.LogWarning("Testing for a null component type will always fail.");
                    return false;
                }

                if (gameObject.GetComponent(type) == null)
                    return false;
            }

            // Update future references
            foreach (var requiredType in m_RequiredComponents)
            {
                var component = gameObject.GetComponent(requiredType.SerializedType.Type);
                requiredType.FutureReference.SceneObjectReference.Update(component);
            }

            return true;
        }

        /// <summary>
        /// Returns FutureObjectReference for this Criterion.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<FutureObjectReference> GetFutureObjectReferences()
        {
            return m_RequiredComponents
                .Where(c => c.SerializedType.Type != null && c.FutureReference != null)
                .Select(c => c.FutureReference);
        }

        /// <summary>
        /// Auto-completes the criterion.
        /// </summary>
        /// <returns>True if the auto-completion succeeded.</returns>
        public override bool AutoComplete()
        {
            var gameObject = TargetGameObject;
            if (gameObject == null)
                return false;

            foreach (var type in RequiredComponents)
            {
                var component = gameObject.AddComponent(type);
                if (component == null)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Wrapper class for serialization purposes.
        /// </summary>
        [Serializable]
        public class SerializedTypeCollection : CollectionWrapper<TypeAndFutureReference> {}

        /// <summary>
        /// A SerializedType-FutureObjectReference pair.
        /// </summary>
        [Serializable]
        public class TypeAndFutureReference : ICloneable
        {
            /// <summary>
            /// The SerializedType.
            /// </summary>
            [SerializedTypeFilter(typeof(Component), false), FormerlySerializedAs("serializedType")]
            public SerializedType SerializedType;

            /// <summary>
            /// The FutureReference.
            /// </summary>
            [FormerlySerializedAs("futureReference")]
            public FutureObjectReference FutureReference;

            /// <summary>
            /// Constructs from a SerializedType.
            /// </summary>
            /// <param name="serializedType"></param>
            public TypeAndFutureReference(SerializedType serializedType)
            {
                this.SerializedType = serializedType;
            }

            /// <summary>
            /// Creates a clone of this instance.
            /// </summary>
            /// <returns></returns>
            public object Clone()
            {
                return new TypeAndFutureReference(SerializedType);
            }
        }
    }
}
