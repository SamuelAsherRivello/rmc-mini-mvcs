using System;

//Keep As:RMC.Mini
namespace RMC.Mini
{
	/// <summary>
	/// The context is the circumstances and setting for
	/// a specific reality.
	/// 
	/// <p>Common Use Case</p>
	/// 
	/// The <see cref="ISimpleMiniMvcs"/>
	/// shares one <see cref="Context"/> instance with each
	/// of its <see cref="IConcern"/>s so they can
	/// co-relate. The context provides object lookup
	/// functionality through the <see cref="ModelLocator"/>
	/// and communication functionality via <see cref="CommandManager"/>.
	///
	/// <p>Advanced Use Case</p>
	/// 
	/// Two or more <see cref="ISimpleMiniMvcs"/> instances
	/// which share one <see cref="Context"/> instance
	/// can co-relate.
	/// 
	/// </summary>
	public class Context : BaseContext
	{
		//  Properties ------------------------------------
		
		/// <summary>
		/// FIXME: The lifespan of the <see cref="Singleton{T}"/> is too long due to a bug.
		/// So, this can optionally be used to help limit collisions of Contexts.
		///
		/// Its likely never needed in a typical game project, but the sample project
		/// contains dozens of samples each with a mini and when running unit testing
		/// they are not always cleaned up properly automatically (yet) during testing.
		/// </summary>
		protected readonly string _contextKey = "";
		
		//  Fields ----------------------------------------

		
		//  Initialization  -------------------------------
		public Context(string contextKey = "") : base()
		{
			// ContextLocator is Experimental: This allows any scope, including
			// non-mini classes, to access any Context via ContextLocator.Instance.GetItem<T>();
			_contextKey = contextKey;
			ContextLocatorAddItem();
		}

		public override void Dispose()
		{
			ContextLocatorRemoveItem();

		}
		
		
		//  Methods ---------------------------------------
		private void ContextLocatorAddItem()
		{
			
			
#if UNITY_5_3_OR_NEWER
			
			if (Mini.ContextLocator.Instance.HasItem<Context>(_contextKey))
			{
				throw new Exception($"Context with key '{_contextKey}' already exists. " +
				                    $"Must pass in unique contextKey. Optional: You can use `Guid.NewGuid().ToString()` to generate a unique key.");
			}
			else
			{
				Mini.ContextLocator.Instance.AddItem(this, _contextKey);
			}
			
			
#endif //if i'm in any version of unity (and thus NOT in Godot)
			
			
			
		}
		
		private void ContextLocatorRemoveItem()
		{
			
			
#if UNITY_5_3_OR_NEWER
			
			if (Mini.ContextLocator.Instance.HasItem<Context>(_contextKey))
			{
				Mini.ContextLocator.Instance.RemoveItem<Context>(_contextKey);
			}
			
#endif //if i'm in any version of unity (and thus NOT in Godot)
			
			
			
		}
	}
}