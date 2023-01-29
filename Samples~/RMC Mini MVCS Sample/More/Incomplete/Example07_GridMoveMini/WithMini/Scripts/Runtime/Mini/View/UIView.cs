using System;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class UIView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }

        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

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
                GridMoveModel gridMoveModel = Context.ModelLocator.GetItem<GridMoveModel>();
                gridMoveModel.StatusText.OnValueChanged.AddListener(
                    (p, c) => GridMoveModel_StatusText_OnValueChanged(p, c));
            }
        }

        
        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        
        private void GridMoveModel_StatusText_OnValueChanged(
            string previousValue, string currentValue)
        {
            RequireIsInitialized();

            if (_statusText != null)
            {
                _statusText.text = $"{currentValue}";
            }
        }

    }
}