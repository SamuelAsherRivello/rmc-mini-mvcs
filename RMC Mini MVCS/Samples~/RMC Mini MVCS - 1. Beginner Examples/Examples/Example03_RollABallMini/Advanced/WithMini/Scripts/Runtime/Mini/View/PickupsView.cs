using System;
using System.Collections.Generic;
using RMC.Mini.View;
using RMC.Mini.Samples.RollABall.WithMini.Components;
using UnityEngine;

namespace RMC.Mini.Samples.RollABall.WithMini.Mini.View
{
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class PickupsView: MonoBehaviour, IView
    {
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
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
                    foreach (var pickupsViewPickup in _pickups)
                    {
                        pickupsViewPickup.CanMove = false;
                    }
                }
 
            }
        }
        
            
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;
        private bool _canMove = true;

        [SerializeField] 
        private List<Pickup> _pickups;
        
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Dispose Methods --------------------------------
        public void Dispose()
        {
            // Optional: Handle any cleanup here...
        }
        
        
        //  Unity Methods ---------------------------------

        //  Event Handlers --------------------------------
    }
}