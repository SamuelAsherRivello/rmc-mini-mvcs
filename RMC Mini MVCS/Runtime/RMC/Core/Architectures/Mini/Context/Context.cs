using RMC.Core.Architectures.Mini.Context.Experimental;
using RMC.Core.Architectures.Mini.Controller.Commands;

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
        
		
		//  Fields ----------------------------------------

		
		//  Initialization  -------------------------------
		public Context() : base()
		{
			// Experimental: This allows any scope, including
			// non-mini classes, to access any IContext
			// instance through ContextLocator
			if (!ContextLocator.Instance.HasItem<IContext>())
			{
				ContextLocator.Instance.AddItem(this);
			}
		}
		
		   
		//  Methods ---------------------------------------
	}
}