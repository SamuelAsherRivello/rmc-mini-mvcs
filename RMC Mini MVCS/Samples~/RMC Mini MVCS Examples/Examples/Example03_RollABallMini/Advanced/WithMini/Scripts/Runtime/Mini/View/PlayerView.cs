using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Components;
using RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

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
        
        
        //  Event Handlers --------------------------------
        private void OnInputCommand(InputCommand inputCommand)
        {
            RequireIsInitialized();
            
            _rigidBody.AddForce (inputCommand.Value * _speed);
        }
    }
}