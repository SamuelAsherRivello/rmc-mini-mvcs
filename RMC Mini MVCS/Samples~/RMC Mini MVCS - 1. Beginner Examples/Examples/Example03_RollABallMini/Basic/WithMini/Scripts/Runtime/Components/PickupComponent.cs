﻿using RMC.Mini.Experimental.ContextLocators;
using RMC.Mini.Samples.RollABall.WithMini.Mini.Model;
using UnityEngine;

namespace RMC.Mini.Samples.RollABall.WithMini.Components
{
	/// <summary>
	/// The Pickup is a spinning yellow cube which the player
	/// can collide with to collect to earn points
	/// </summary>
	public class PickupComponent : MonoBehaviour
	{
        //  Fields ----------------------------------------
        private bool _isGameOver = false;
        private bool _isGamePaused = false;
        
        
        //  Unity Methods ---------------------------------
        protected void Awake ()
        {
	        MakeBridgeToMini();
        }


        protected void Update ()
        {
	        if (_isGameOver || _isGamePaused)
	        {
		        return;
	        }
	        
	        transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
        }
        
        
        //  Methods ---------------------------------------
        /// <summary>
        /// EXPERIMENTAL: This and any use of <see cref="ContextLocator"/>
        /// is experimental. It's a leading solution for
        /// any scope 'outside' of MVCS to reference 'inside'
        /// the MVCS assuming it has NO direct reference to the MVCS.
        /// </summary>
        private void MakeBridgeToMini()
        {
	         ContextLocator.Instance.OnItemAdded.AddListener(context =>
	         {
		         // ... and any model is added...
		         context.ModelLocator.OnItemAdded.AddListener(model =>
		         {
			         // ... listen to changes in the appropriate model
			         RollABallModel rollABallModel = (RollABallModel)model;
			         if (rollABallModel != null)
			         {
				         rollABallModel.IsGameOver.OnValueChanged.AddListener(IsGameOver_OnValueChanged);
				         rollABallModel.IsGamePaused.OnValueChanged.AddListener(IsGamePaused_OnValueChanged);
			         }
		         });
	         });
        }
        
        //  Event Handlers --------------------------------
		private void IsGameOver_OnValueChanged(bool previousValue, bool currentValue)
		{
			_isGameOver = currentValue;
		}
		
		private void IsGamePaused_OnValueChanged(bool previousValue, bool currentValue)
		{
			_isGamePaused = currentValue;
		}
	}	
}
