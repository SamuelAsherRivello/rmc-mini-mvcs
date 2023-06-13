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
    public class PickupUnityEvent : UnityEvent<Pickup> {}

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
        
        //  Properties ------------------------------------
        public bool CanMove
        {
            get
            {
                return _canMove;
            }
            set
            {
                if (_canMove != value)
                {
                    _canMove = value;
                    if (!_canMove)
                    {
                        _lastVelocityBeforePause = _rigidBody.velocity;
                        _lastAngularVelocityBeforePause = _rigidBody.angularVelocity;
                        _rigidBody.velocity = Vector3.zero;
                        _rigidBody.angularVelocity = Vector3.zero;
                    }
                    else
                    {
                        _rigidBody.velocity = _lastVelocityBeforePause;
                        _rigidBody.angularVelocity = _lastAngularVelocityBeforePause;
                  
                    }
                }
 
            }
        }

        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;
        private bool _canMove = true;
        private Vector3 _lastVelocityBeforePause = Vector3.zero;
        private Vector3 _lastAngularVelocityBeforePause = Vector3.zero;
        
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
            Pickup pickup = myCollider.gameObject.GetComponent<Pickup>();
            if (pickup != null)
            {
                OnPickup.Invoke(pickup);
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

            if (CanMove)
            {
                _rigidBody.AddForce (inputCommand.Value * _speed);
            }
        }
    }
}