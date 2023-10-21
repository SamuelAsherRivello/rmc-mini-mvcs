using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Abstract base class for CollectionWrapper implementations.
    /// </summary>
    public abstract class CollectionWrapper
    {
    }

    /// <summary>
    /// Represents an abstract collection of items. The items are stored internally as List.
    /// Implement your own concrete collections by inheriting from this class.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public abstract class CollectionWrapper<T> : CollectionWrapper, IEnumerable<T>
    {
        [SerializeField]
        List<T> m_Items = new List<T>();

        /// <summary>
        /// Default-constructs an empty collection.
        /// </summary>
        public CollectionWrapper() {}
        /// <summary>
        /// Constructs with items.
        /// </summary>
        /// <param name="items"></param>
        public CollectionWrapper(IList<T> items) { SetItems(items); }

        /// <summary>
        /// Returns an item at a specific index.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T this[int i]
        {
            get { return m_Items[i]; }
            set
            {
                if (
                    (m_Items[i] == null && value != null) ||
                    (m_Items[i] != null && !m_Items[i].Equals(value))
                )
                {
                    m_Items[i] = value;
                }
            }
        }

        /// <summary>
        /// The count of items.
        /// </summary>
        public int Count { get { return m_Items.Count; } }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        /// <summary>
        /// Returns an Enumerator to the items.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        /// <summary>
        /// Returns the items of the collection as a list.
        /// </summary>
        /// <param name="items"></param>
        public void GetItems(List<T> items)
        {
            if (items.Capacity < m_Items.Count)
            {
                items.Capacity = m_Items.Count;
            }
            items.Clear();
            items.AddRange(m_Items);
        }

        /// <summary>
        /// Sets the items of the collection.
        /// </summary>
        /// <param name="items"></param>
        public void SetItems(IEnumerable<T> items)
        {
            m_Items.Clear();
            m_Items.AddRange(items);
        }

        /// <summary>
        /// Adds an item to the collection
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(T item)
        {
            m_Items.Add(item);
        }
    }
}
