using System;
using System.Collections.Generic;
using System.Linq;
using RMC.Core.Events;

//Keep As:RMC.Mini
namespace RMC.Mini
{
    public class Locator
    {
        public static Type GetLowestType(Type type)
        {
            return GetTypeHierarchy(type).LastOrDefault();
        }
        
        private static Type[] GetTypeHierarchy(Type type)
        {
            var hierarchy = new System.Collections.Generic.List<Type>();

            while (type != null)
            {
                hierarchy.Insert(0, type);  // Insert at the beginning to maintain order
                type = type.BaseType;
            }

            return hierarchy.ToArray();
        }
    }

    public class Locator<TBase> : Locator
    {
        public class LocatorItemUnityEvent : RmcEvent<TBase> {}
        public readonly LocatorItemUnityEvent OnItemAdded = new LocatorItemUnityEvent();

        private Dictionary<Type, Dictionary<string, TBase>> _items = new Dictionary<Type, Dictionary<string, TBase>>();

        // AddItem method to add items based on their actual type
        public void AddItem(TBase item, string key = "")
        {
            Type type = GetLowestType(item.GetType());
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
                    throw new Exception($"{nameof(AddItem)}() failed. " +
                                        $"Item of type '{type.Name}' already exists in Locator<{typeof(TBase).Name}>.");
                }
                else
                {
                    throw new Exception($"{nameof(AddItem)}() failed. " +
                                        $"Item of type '{type.Name}' with key '{key}' already exists in Locator<{typeof(TBase).Name}>.");;
                }
            }
        }

        // GetItem method to retrieve items based on their type
        public TItem GetItem<TItem>(string key = "") where TItem : TBase
        {
            Type type = GetLowestType(typeof(TItem));

            if (type.IsInterface)
            {
                throw new Exception ($"Cannot get item of type {type.Name}. " +
                                     $"Interfaces are not allowed for this particular method.");
            }
            
            if (_items.ContainsKey(type) && _items[type].ContainsKey(key))
            {
                return (TItem)_items[type][key];
            }
            return default(TItem);
        }

        // HasItem method to check if an item exists based on its type
        public bool HasItem<TItem>(string key = "") where TItem : TBase
        {
            return GetItem<TItem>(key) != null;
        }

        // RemoveItem method to remove items based on their type
        public void RemoveItem<TItem>(string key = "") where TItem : TBase
        {
            Type type = GetLowestType(typeof(TItem));

            if (type.IsInterface)
            {
                throw new Exception ($"Cannot get item of type {type.Name}. " +
                                     $"Interfaces are not allowed for this particular method.");
            }
            
            if (_items.ContainsKey(type) && _items[type].ContainsKey(key))
            {
                _items[type].Remove(key);
            }
            else
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new Exception($"{nameof(GetItem)}() failed. " +
                                        $"Item of type '{type.Name}' does not exist in Locator<{typeof(TBase).Name}>.");
                }
                else
                {
                    throw new Exception($"{nameof(GetItem)}() failed. " +
                                        $"Item of type '{type.Name}' with key '{key}' does not exist in Locator<{typeof(TBase).Name}>.");;
                }
            }
        }

        // Reset method to clear all items
        public void Reset()
        {
            _items.Clear();
        }
    }
}