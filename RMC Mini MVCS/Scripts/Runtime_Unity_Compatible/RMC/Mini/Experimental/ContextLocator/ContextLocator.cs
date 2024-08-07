﻿
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RMC.Mini.Experimental.ContextLocators
{
	/// <summary>
	/// This is experimental.
	/// 
	/// This Locator manages the storage, lookup,
	/// and retrieval of <see cref="IContext"/> objects.
	///
	/// The primary use case within games
	/// is to allow any arbitrary scope (whether
	/// inside the MVCS or not) to access any
	/// <see cref="IContext"/>. Essentially this bridges
	/// the non-mvcs world to the mvcs world.
	///
	/// NOTE: Its generally recommended NOT to use ContextLocator.
	///
	/// 
	/// </summary>
	public class ContextLocator : Locator<IContext>
	{
		private static ContextLocator _instance;

		public static ContextLocator Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ContextLocator();
					ObserveOnPlayModeStateChanged();
				}

				return _instance;
			}
		}

		public ContextLocator() : base()
		{
		}
		
		/// <summary>
		/// During Unit Testing and More. Clear this instance
		/// </summary>
		public static void Destroy ()
		{
			//Debug.Log("ContextLocator.Destroy ");
			
			_instance = null;
		}

#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		public static void  OnInitializeOnLoadMethod()
		{
			Destroy();
		}
#endif //UNITY_EDITOR

		private static void ObserveOnPlayModeStateChanged()
		{
#if UNITY_EDITOR
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif //UNITY_EDITOR
		}
		
#if UNITY_EDITOR
		/// <summary>
		/// When the scene ends, clear this instance
		/// </summary>
		private static void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
		{
			if (playModeStateChange == PlayModeStateChange.ExitingPlayMode)
			{
				Destroy();
			}
		}
#endif //UNITY_EDITOR

	}
}