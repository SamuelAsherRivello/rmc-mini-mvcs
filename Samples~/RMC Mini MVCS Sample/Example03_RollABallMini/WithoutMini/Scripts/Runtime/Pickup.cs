using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithoutMini
{
	/// <summary>
	/// The Pickup is a spinning yellow cube which the player
	/// can collide with to collect to earn points
	/// </summary>
	public class Pickup : MonoBehaviour
	{
		//  Events ----------------------------------------
        
                
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        
        //  Unity Methods ---------------------------------

        
        //  Methods ---------------------------------------
        public void Rotate ()
        {
	        transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
        }
        
        //  Event Handlers --------------------------------
	}	
}
