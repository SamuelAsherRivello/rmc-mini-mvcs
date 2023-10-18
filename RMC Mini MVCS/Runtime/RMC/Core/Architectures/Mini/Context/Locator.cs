using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Context
{
	/// <summary>
	/// The Locator manages the storage, lookup,
	/// and retrieval of <see cref="IItem"/> objects
	/// of arbitrary type. 
	/// </summary>

	public class Locator<TBase>
	{
		public class AddItemCompletedUnityEvent : UnityEvent<TBase> {}

		//  Events ----------------------------------------
		public readonly AddItemCompletedUnityEvent OnAddItemCompleted = new AddItemCompletedUnityEvent();
		
		//  Properties ------------------------------------
        
		
		//  Fields ----------------------------------------
		private Dictionary<Type, Dictionary<string, TBase>> _services = new Dictionary<Type, Dictionary<string, TBase>>();

		
		//  Initialization  -------------------------------
		
		
		//  Methods ---------------------------------------
	
		
		// Add a service of type TItem
		public void AddItem<TItem>(TItem service, string key = "") where TItem : TBase
		{
			var type = typeof(TItem);
			if (!_services.ContainsKey(type))
			{
				_services[type] = new Dictionary<string, TBase>();
			}

			if (!_services[type].ContainsKey(key))
			{
				_services[type][key] = service;
			}
			else
			{
				throw new Exception($"Service of type {type.Name} with key '{key}' already exists.");
			}
		}

		// Remove a service of type TItem
		public void RemoveItem<TItem>(string key = "") where TItem : TBase
		{
			var type = typeof(TItem);
			if (_services.ContainsKey(type) && _services[type].ContainsKey(key))
			{
				_services[type].Remove(key);
			}
			else
			{
				throw new Exception($"Service of type {type.Name} with key '{key}' not found.");
			}
		}

		// Get a service of the specified type and key
		public TItem GetItem<TItem>(string key = "") where TItem : TBase
		{
			var type = typeof(TItem);
			if (_services.ContainsKey(type) && _services[type].ContainsKey(key))
			{
				return (TItem)_services[type][key];
			}
			else
			{
				return default(TItem);
			}
		}

		public bool HasItem<TItem>(string key = "") where TItem : TBase
		{
			try
			{
				return GetItem<TItem>(key) != null;
			}
			catch
			{
				return false;
			}
		}

		// Reset or clear all services
		public void Reset()
		{
			_services.Clear();
		}
		
		//  Event Handlers --------------------------------
	}
}