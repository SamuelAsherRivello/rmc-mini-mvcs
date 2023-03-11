using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Context
{

	/// <summary>
	/// The Locator manages the storage, lookup,
	/// and retrieval of <see cref="IItem"/> objects
	/// of arbitrary type. 
	/// </summary>
	public class Locator<IItem>
	{
		public class AddItemCompletedUnityEvent : UnityEvent<IItem> {}

		//  Events ----------------------------------------
		public readonly AddItemCompletedUnityEvent OnAddItemCompleted = new AddItemCompletedUnityEvent();
		
		//  Properties ------------------------------------
        
		//  Fields ----------------------------------------
		private List<IItem> _items = new List<IItem>();
        
		//  Initialization  -------------------------------
		
		//  Methods ---------------------------------------
		public void AddItem (IItem item)
		{
			if (HasItem<IItem>())
			{
				// Allow MAX 0 or 1 instance of T
				throw new Exception("AddItem() failed. Item already added. Call HasItem<T>() first.");
			}
			_items.Add(item);
			OnAddItemCompleted.Invoke(item);
		}
		
		public bool HasItem<SubType>() where SubType :IItem 
		{
			return GetItem<SubType>() != null;
		}
		
		public SubType GetItem<SubType>() where SubType :IItem 
		{
			return _items.OfType<SubType>().ToList().FirstOrDefault<SubType>();
		}
		
		public IItem GetItem(Type type)
		{
			return _items.FirstOrDefault(item => item.GetType() == type);
		}

		public void RemoveItem(IItem item)
		{
			if (!HasItem<IItem>())
			{
				throw new Exception("RemoveItem() failed. Must call HasItem<T>() first.");
			}
			_items.Remove(item);
		}
		
		//  Event Handlers --------------------------------
	}
}