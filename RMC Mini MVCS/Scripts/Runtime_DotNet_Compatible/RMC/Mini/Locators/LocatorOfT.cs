using System;
using System.Collections.Generic;
using RMC.Core.Events;
using UnityEngine;

namespace RMC.Mini
{
    public class Locator<TBase> : Locator
    {
        public class LocatorItemUnityEvent : RmcEvent<TBase> { }
        public readonly LocatorItemUnityEvent OnItemAdded = new LocatorItemUnityEvent();
        public readonly LocatorItemUnityEvent OnItemRemoved = new LocatorItemUnityEvent();
        private readonly Dictionary<Type, Dictionary<string, TBase>> _items = new Dictionary<Type, Dictionary<string, TBase>>();

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
                Debug.Log($"{this.GetType().Name}.AddItem() Success for {type.Name} with optional key '{key}'.");
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

        public TItem GetItem<TItem>(string key = "") where TItem : TBase
        {
            Type type = Locator.GetLowestType(typeof(TItem));

            if (_items.ContainsKey(type) && _items[type].ContainsKey(key))
            {
                return (TItem)_items[type][key];
            }

            return default(TItem);
        }

        public bool HasItem<TItem>(string key = "") where TItem : TBase
        {
            return GetItem<TItem>(key) != null;
        }

        public void RemoveItem<TItem>(string key = "") where TItem : TBase
        {
            Type type = Locator.GetLowestType(typeof(TItem));

            Debug.Log($"Item1 of type '{type.Name}' with key '{key}' removed.");
            if (_items.ContainsKey(type) && _items[type].ContainsKey(key))
            {
                var item = _items[type][key];
                _items[type].Remove(key);
                OnItemRemoved.Invoke(item);
                Debug.Log($"Item2 of type '{type.Name}' with key '{key}' removed.");
            }
            else
            {
                throw new Exception($"Item of type '{type.Name}' with key '{key}' not found.");
            }
        }

        private Type GetLowestType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            while (type.BaseType != null && type.BaseType != typeof(TBase) && type.BaseType != typeof(object))
            {
                type = type.BaseType;
            }

            return type;
        }

        public override void Dispose()
        {
            Reset();
        }

        public void Reset()
        {
            _items.Clear();
        }

        public int GetItemCount()
        {
            return _items.Count;
        }

        // This method is used for testing only
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

        // New method to handle type checking and item retrieval logic
        public Locator<T> GetOrCreateLocator<T>() where T : class, TBase
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
                    Debug.LogWarning($"Type mismatch detected: {item.GetType()} cannot be cast to {typeof(T)}");
                    return null;
                }
            }

            return newLocator;
        }
    }
}
