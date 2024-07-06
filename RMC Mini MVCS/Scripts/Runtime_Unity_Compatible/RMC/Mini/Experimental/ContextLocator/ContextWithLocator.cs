using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace RMC.Mini.Experimental.ContextLocators
{
	/// <summary>
	/// EXPERIMENTAL: This is an alternative version of
	/// <see cref="IContext"/> that keeps a globally accessible
	/// reference to this context via <see cref="ContextLocator"/>.
	///
	/// USES: See <see cref="ContextLocator"/> for more info.
	/// 
	/// </summary>
	public class ContextWithLocator : BaseContext
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

		
		/// <summary>
		/// Some demos require this type. Centralize the warning logging here
		/// for consistency.
		/// </summary>
		/// <param name="context"></param>
		public static void AssertIsContextWithLocator(IContext context)
		{
			// NOTE: This demo is rare. It needs a special type of context,
			// so that the context can be found by the experimental ContextLocator
			// within. This is not a typical use case and is not generally recommended
			Assert.IsNotNull(context as ContextWithLocator, 
				$"This situation requires a context of type {nameof(ContextWithLocator)}.");
		}
		
		//  Initialization  -------------------------------
		
		/// <summary>
		/// Helper method to call with unique key
		/// </summary>
		/// <returns></returns>
		public static ContextWithLocator CreateNew()
		{
			var contextKey = Guid.NewGuid().ToString();
			return new ContextWithLocator(contextKey, false);
		}
		
		
		/// <summary>
		/// Create a new Context with a built-in <see cref="ContextLocator"/>
		/// </summary>
		/// <param name="contextKey">Use this key for registry to the <see cref="ContextLocator"/></param>
		/// <param name="willOverwriteByContextKey">Will delete any entry with conflicting contextKey (if exists)</param>
		public ContextWithLocator(string contextKey, bool willOverwriteByContextKey) : base()
		{
			// ContextLocator is Experimental: This allows any scope, including
			// non-mini classes, to access any Context via ContextLocator.Instance.GetItem<T>();
			_contextKey = contextKey;
			ContextLocatorAddItem(willOverwriteByContextKey);
		}

		public override void Dispose()
		{
			ContextLocatorRemoveItem();
		}
		
		
		//  Methods ---------------------------------------
		private void ContextLocatorAddItem(bool willOverwriteByContextKey)
		{
			
			
#if UNITY_5_3_OR_NEWER
			
			if (ContextLocator.Instance.HasItem<ContextWithLocator>(_contextKey))
			{
				string message = $"Context with key '{_contextKey}' already exists. So, DELETING existing Context with that key. " +
				                 $"Must pass in unique contextKey. Optional: You can use `Guid.NewGuid().ToString()` to generate a unique key.";

				if (willOverwriteByContextKey)
				{
					ContextLocator.Instance.RemoveAndDisposeItem<ContextWithLocator>( _contextKey);
					
					//KEEP LOG
					Debug.LogWarning(message);
				}
				else
				{
					throw new Exception(message);
				}
			}
			
			ContextLocator.Instance.AddItem(this, _contextKey);


#endif //if i'm in any version of unity (and thus NOT in Godot)
			
			
			
		}
		
		private void ContextLocatorRemoveItem()
		{
			
			
#if UNITY_5_3_OR_NEWER
			
			if (ContextLocator.Instance.HasItem<Context>(_contextKey))
			{
				ContextLocator.Instance.RemoveAndDisposeItem<Context>(_contextKey);
			}
			
#endif //if i'm in any version of unity (and thus NOT in Godot)
			
			
			
		}


	}
}