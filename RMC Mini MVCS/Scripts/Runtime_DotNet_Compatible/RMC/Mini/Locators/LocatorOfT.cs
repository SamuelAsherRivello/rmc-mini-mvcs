using System;
using System.Collections.Generic;
using RMC.Core.Events;
using UnityEngine;

namespace RMC.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Locator class for managing items of type TBase.
    /// This class provides methods to add, retrieve, check for, and remove items in a type-safe manner.
    /// It ensures that items are stored and retrieved based on their most specific type.
    /// 
    /// Generally, you can use the public API methods directly (e.g., AddItem, GetItem, HasItem, RemoveItem).
    /// However, in cases where you need to work with a specific derived type, you will need to call RecastLocatorAs
    /// inline with the method you want. For example:
    /// 
    /// <code>
    /// var specificLocator = locator.RecastLocatorAs<DerivedType>();
    /// specificLocator.AddItem(derivedItem);
    /// </code>
    /// </summary>
    public class Locator<TBase> : Locator
    {
        //  Events ----------------------------------------
        public class LocatorItemUnityEvent : RmcEvent<TBase> { }
        public readonly LocatorItemUnityEvent OnItemAdded = new LocatorItemUnityEvent();
        public readonly LocatorItemUnityEvent OnItemRemoved = new LocatorItemUnityEvent();

        //  Properties ------------------------------------

        //  Fields ----------------------------------------
        private readonly Dictionary<Type, Dictionary<string, TBase>> _items = new Dictionary<Type, Dictionary<string, TBase>>();

        //  Initialization  -------------------------------

        /// <summary>
        /// Adds an item to the locator.
        /// </summary>
        public void AddItem(TBase item, string key = "")
        {
            Type type = Locator.GetLowestType(item.GetType());
            if (!_items.ContainsKey(type))
            {
                _items[type] = new Dictionary<string, TBase>();
            }

            if (!_items[type].ContainsKey(key))
            {
                _items[type][key] = item;
                OnItemAdded.Invoke(item);
            }
            else
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new Exception($"{nameof(AddItem)} failed. Item of type '{type.Name}' already exists.");
                }
                else
                {
                    throw new Exception(
                        $"{nameof(AddItem)} failed. Item of type '{type.Name}' with key '{key}' already exists.");
                }
            }
        }

        //  Unity Methods   -------------------------------

        //  Other Methods ---------------------------------

        /// <summary>
        /// Retrieves an item from the locator by its type and optional key.
        /// </summary>
        public TItem GetItem<TItem>(string key = "") where TItem : TBase
        {
            Type type = Locator.GetLowestType(typeof(TItem));

            if (_items.ContainsKey(type) && _items[type].ContainsKey(key))
            {
                return (TItem)_items[type][key];
            }

            return default(TItem);
        }

        /// <summary>
        /// Checks if an item of a specific type and key exists in the locator.
        /// </summary>
        public bool HasItem<TItem>(string key = "") where TItem : TBase
        {
            return GetItem<TItem>(key) != null;
        }

        public void RemoveItem<TItem>(bool willDispose, string key = "") where TItem : TBase
        {
            Type type = Locator.GetLowestType(typeof(TItem));

            if (_items.ContainsKey(type) && _items[type].ContainsKey(key))
            {
                var item = _items[type][key];
                _items[type].Remove(key);
                OnItemRemoved.Invoke(item);

                if (willDispose && item is IDisposable disposableItem)
                {
                    disposableItem.Dispose();
                }
            }
            else
            {
                throw new Exception($"Item of type '{type.Name}' with key '{key}' not found.");
            }
        }

        // Overload for automatically disposing
        public void RemoveAndDisposeItem<TItem>(string key = "") where TItem : TBase, IDisposable
        {
            RemoveItem<TItem>(true, key);
        }
        
        
        /// <summary>
        /// Creates a type-safe locator for items of type T.
        /// 
        /// Generally, you call the API methods directly. For example:
        /// <code>
        /// locator.AddItem(item);
        /// var retrievedItem = locator.GetItem&lt;TItem&gt;(key);
        /// </code>
        /// 
        /// However, in cases where you need to work with a specific derived type, you will need to call RecastLocatorAs
        /// inline with the method you want. For example:
        /// <code>
        /// var specificLocator = locator.RecastLocatorAs&lt;DerivedType&gt;();
        /// specificLocator.AddItem(derivedItem);
        /// </code>
        /// </summary>
        public Locator<T> RecastLocatorAs<T>() where T : class, TBase
        {
            var newLocator = new Locator<T>();

            foreach (var item in GetAllItems())
            {
                if (item is T castedItem)
                {
                    newLocator.AddItem(castedItem);
                }
                else
                {
                    //Keep logging
                    Debug.LogWarning($"Type mismatch detected: {item.GetType()} cannot be cast to {typeof(T)}");
                    return null;
                }
            }

            return newLocator;
        }



        /// <summary>
        /// Gets the count of items in the locator.
        /// </summary>
        public int GetItemCount()
        {
            return _items.Count;
        }

        /// <summary>
        /// Gets all items in the locator.
        /// </summary>
        public List<TBase> GetAllItems()
        {
            List<TBase> items = new List<TBase>();
            foreach (var type in _items.Keys)
            {
                foreach (var key in _items[type].Keys)
                {
                    items.Add(_items[type][key]);
                }
            }
            return items;
        }
        
        
        /// <summary>
        /// Resets the locator, removing all items.
        /// </summary>
        public void Reset()
        {
            _items.Clear();
        }
        
        
        /// <summary>
        /// Disposes of the locator, clearing all items.
        /// </summary>
        public override void Dispose()
        {
            Reset();
        }


        //  Event Handlers --------------------------------
    }
}
