using System;
using RMC.Core.Experimental;
using RMC.Core.Architectures.Mini.Controller.Commands;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Context
{
	/// <summary>
	/// The context is the circumstances and setting for
	/// a specific reality.
	/// 
	/// <p>Common Use Case</p>
	/// 
	/// The <see cref="IMiniMvcs"/>
	/// shares one <see cref="Context"/> instance with each
	/// of its <see cref="IConcern"/>s so they can
	/// co-relate. The context provides object lookup
	/// functionality through the <see cref="ModelLocator"/>
	/// and communication functionality via <see cref="CommandManager"/>.
	///
	/// <p>Advanced Use Case</p>
	/// 
	/// Two or more <see cref="IMiniMvcs"/> instances
	/// which share one <see cref="Context"/> instance
	/// can co-relate.
	/// 
	/// </summary>
	public class Context : BaseContext
	{
		//  Properties ------------------------------------
		protected readonly string _contextKey = "";
		
		//  Fields ----------------------------------------

		
		//  Initialization  -------------------------------
		public Context(string contextKey = "") : base()
		{
			// ContextLocator is Experimental: This allows any scope, including
			// non-mini classes, to access any Context via ContextLocator.Instance.GetItem<T>();
			_contextKey = contextKey;
			if (_contextKey == "")
			{
				//Experimental
				_contextKey = Guid.NewGuid().ToString();
			}
			if (ContextLocator.Instance.HasItem<Context>(_contextKey))
			{
				throw new Exception($"Context with key '{_contextKey}' already exists. Must pass in unique contextKey.");
			}
			else
			{
				ContextLocator.Instance.AddItem(this, _contextKey);
			}

		}
		
		public override void Dispose()
		{
			if (ContextLocator.Instance.HasItem<Context>(_contextKey))
			{
				ContextLocator.Instance.RemoveItem<Context>(_contextKey);
			}
		}
		   
		//  Methods ---------------------------------------

	}
}