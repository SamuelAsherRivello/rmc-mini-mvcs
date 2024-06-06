using System;
using RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class OnBounceUnityEvent : UnityEvent {}

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class BouncyBallView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        public readonly OnBounceUnityEvent OnBounced = new OnBounceUnityEvent();

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        public Text TitleText { get { return _titleText;} }
        public Text StatusText { get { return _statusText;} }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private Text _titleText;

        [SerializeField] 
        private Text _statusText;

        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                //
                Context.CommandManager.AddCommandListener<BounceCountChangedCommand>(
                    OnBounceCountChangedCommand);
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
        protected void OnCollisionEnter(Collision collision)
        {
            RequireIsInitialized();
            OnBounced.Invoke();
        }
        
        protected void OnDestroy()
        {
            Context?.CommandManager?.RemoveCommandListener<BounceCountChangedCommand>(
                OnBounceCountChangedCommand);
        }

        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        private void OnBounceCountChangedCommand(BounceCountChangedCommand bounceCountChangedCommand)
        {
            RequireIsInitialized();
            
            // Demo - View may READ model...
            int bounceCountMax = Context.ModelLocator.GetItem<BouncyBallModel>().BounceCountMax.Value;
           
            
            // Take Incoming Event Data
            _statusText.text = $"Bounce Count: {bounceCountChangedCommand.CurrentValue}/{bounceCountMax}";
        }

    }
}