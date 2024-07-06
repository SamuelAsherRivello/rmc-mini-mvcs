using System;
using RMC.Mini.View;
using RMC.Mini.Samples.RollABall.WithMini.Components;
using RMC.Mini.Samples.RollABall.WithMini.Mini.Controller.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Mini.Samples.RollABall.WithMini.Mini.View
{
    //  Class Attributes ----------------------------------
    public class PickupUnityEvent : UnityEvent<PickupComponent> {}

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class PlayerView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly PickupUnityEvent OnPickup = new PickupUnityEvent();

        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        public bool IsPaused { get; set; }


        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private Rigidbody _rigidBody;
        
        [SerializeField] 
        private float _speed = 500;
        
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //
                Context.CommandManager.AddCommandListener<InputCommand>(
                    OnInputCommand);
            }
        }

        
        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Unity Methods ---------------------------------
        protected void OnTriggerEnter(Collider myCollider) 
        {
            RequireIsInitialized();

            if (IsPaused)
            {
                return;
            }

            // Did I collide with the correct object?
            PickupComponent pickupComponent = myCollider.gameObject.GetComponent<PickupComponent>();
            if (pickupComponent != null)
            {
                OnPickup.Invoke(pickupComponent);
            }
        }
        
        protected void OnDestroy()
        {
            Context?.CommandManager?.RemoveCommandListener<InputCommand>(
                OnInputCommand);
        }

        //  Methods ---------------------------------------
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Event Handlers --------------------------------
        private void OnInputCommand(InputCommand inputCommand)
        {
            RequireIsInitialized();
            
            if (IsPaused)
            {
                return;
            }
            
            _rigidBody.AddForce (inputCommand.Value * _speed);
        }
    }
}