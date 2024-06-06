using System;
using RMC.Core.Architectures.Mini;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.Core.Architectures.Mini.Samples.SOM.Mini
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="InventoryController"/>
    /// 
    /// </summary>
    public class InventoryView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnAction = new UnityEvent();
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                
                InventoryScriptableObjectModel inventoryScriptableObjectModel = Context.ModelLocator.GetItem<InventoryScriptableObjectModel>();
                inventoryScriptableObjectModel.InventoryCount.OnValueChanged.AddListener(InventoryCount_OnValueChanged);
                RefreshUI();
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
        
        protected void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnAction.Invoke();
            }
        }

        
        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }


        //  Methods ---------------------------------------
        private void RefreshUI()
        {
         
        }
        
        
        //  Event Handlers --------------------------------
        private void InventoryCount_OnValueChanged(int previousValue, int currentValue)
        {
            Debug.Log($"InventoryView. InventoryCount = {currentValue}");
        }
        
        private void PlayButton_OnClicked()
        {
            RefreshUI();
            OnAction.Invoke();
        }
    }
}