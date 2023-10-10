using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
			//Strict means *ONLY* allow 1 instance of T
			//But *DO* allow 2+ subclasses of T 
			//And *DO* allow 2+ sibling subclasses of T
			if (HasItem<IItem>(true) 
			    && GetItem<IItem>(true)?.GetType() == item.GetType())
			{
				// Allow MAX 0 or 1 instance of T
				throw new Exception("AddItem() failed. Item already added. Call HasItem<T>() first.");
			}

			Debug.Log($"1 GetItem<IItem>(true).GetType(): " + GetItem<IItem>(true)?.GetType());
			Debug.Log("2 item.GetType(): " + item.GetType());

			_items.Add(item);
			OnAddItemCompleted.Invoke(item);
		}
		
		public bool HasItem<SubType>(bool isStrict = false) where SubType :IItem 
		{
			return GetItem<SubType>(isStrict) != null;
		}
		
		
		public SubType GetItem<SubType>(bool isStrict = false) where SubType :IItem
		{
			SubType instance;

			if (isStrict)
			{
				instance = _items.OfType<SubType>().ToList().FirstOrDefault<SubType>();
				
				//Found something?
				if (instance != null)
				{
					//are both things an interface?
					if (instance.GetType().IsInterface && typeof(SubType).IsInterface)
					{
						Debug.Log("Yes interface");
						//Here we do NOT match if they are NOT the same type
						//regardless if they share an interface
						if (!(instance?.GetType() is SubType))
						{
							Debug.Log("clear it!!!");
							instance = default(SubType);
						}
					}
					else
					{
						Debug.Log("NOT not interface");
					}
				}
			}
			else
			{
				instance = _items.OfType<SubType>().ToList().FirstOrDefault<SubType>();
			}
		
			return instance;
		}
		
		
		public IItem GetItem(Type type)
		{
			return _items.FirstOrDefault(item => item.GetType() == type);
		}

		public void RemoveItem(IItem item)
		{
			//Strict means *ONLY* allow 1 instance of T
			//But *DO* allow 2+ subclasses of T 
			//And *DO* allow 2+ sibling subclasses of T
			if (HasItem<IItem>(true) 
			    && GetItem<IItem>(true).GetType() == item.GetType())
			{
				// Allow MAX 0 or 1 instance of T
				throw new Exception("RemoveItem() failed. Must call HasItem<T>() first.");
			}
			
			_items.Remove(item);
		}
		
		public void RemoveItem<SubType>(IItem item) where SubType : IItem 
		{
			//Strict means *ONLY* allow 1 instance of T
			//But *DO* allow 2+ subclasses of T 
			//And *DO* allow 2+ sibling subclasses of T
			if (!(HasItem<SubType>(true) 
			    && GetItem<SubType>(true).GetType() == item.GetType()))
			{
				// Allow MAX 0 or 1 instance of T
				throw new Exception("RemoveItem() failed. Must call HasItem<T>() first.");
			}
			
			_items.Remove(item);
		}
		
		public void RemoveItem<SubType>() where SubType : IItem 
		{
			//Strict means *ONLY* allow 1 instance of T
			//But *DO* allow 2+ subclasses of T 
			//And *DO* allow 2+ sibling subclasses of T
			if (!HasItem<SubType>(true))
			{
				// Allow MAX 0 or 1 instance of T
				throw new Exception("RemoveItem() failed. Must call HasItem<T>() first.");
			}
			
			_items.Remove(GetItem<SubType>(true));
		}
		
		//  Event Handlers --------------------------------
	}
}