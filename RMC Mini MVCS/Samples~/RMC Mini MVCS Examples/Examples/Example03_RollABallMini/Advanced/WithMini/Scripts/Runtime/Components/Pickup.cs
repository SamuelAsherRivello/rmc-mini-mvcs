using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Components
{
	/// <summary>
	/// The Pickup is a spinning yellow cube which the player
	/// can collide with to collect to earn points
	/// </summary>
	public class Pickup : MonoBehaviour
	{
        
                
        //  Properties ------------------------------------
        public bool CanMove { get { return _canMove;} set { _canMove = value;} }

        //  Fields ----------------------------------------
        private bool _canMove = true;
        
        //  Unity Methods ---------------------------------
        protected void Update ()
        {
	        if (CanMove)
	        {
		        transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	        }
        }
        
        //  Methods ---------------------------------------
        
        //  Event Handlers --------------------------------
	}	
}
