using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithoutMini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class PickupUnityEvent : UnityEvent<Pickup> {}

    /// <summary>
    /// The Player is the users avatar, a 3D sphere which
    /// uses Unity Physics to move and collide with other
    /// world elements
    /// </summary>
    public class Player: MonoBehaviour
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly PickupUnityEvent OnPickup = new PickupUnityEvent();

        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        [SerializeField] 
        private Rigidbody _rigidBody;
        
        [SerializeField] 
        private float _speed = 500;
        
        //  Initialization  -------------------------------
        
        
        //  Unity Methods ---------------------------------
        protected void Start()
        {
        }

        //  Unity Methods ---------------------------------
        protected void OnTriggerEnter(Collider myCollider) 
        {
            // Did I collide with the correct object?
            Pickup pickup = myCollider.gameObject.GetComponent<Pickup>();
            if (pickup != null)
            {
                OnPickup.Invoke(pickup);
            }
        }
        
        //  Methods ---------------------------------------
        public void Move(Vector3 movement)
        {
            _rigidBody.AddForce (movement * _speed);
        }
        
        //  Event Handlers --------------------------------

    }
}