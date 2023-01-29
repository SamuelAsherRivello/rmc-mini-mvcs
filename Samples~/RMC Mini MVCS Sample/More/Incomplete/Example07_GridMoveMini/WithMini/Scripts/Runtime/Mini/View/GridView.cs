using System;
using System.Collections.Generic;
using System.Numerics;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.View.UI;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------
    public class CellClickUnityEvent : UnityEvent<int> {}
    
    //  Class Attributes ----------------------------------

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class GridView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly CellClickUnityEvent OnCellClick = new CellClickUnityEvent();

        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;
        
        [SerializeField] private GridUI _gridUI;
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                for (int index = 0; index < _gridUI.CellUIs.Count; index++)
                {
                    int localIndex = index;
                    CellUI cellUI = _gridUI.CellUIs[index];
                    cellUI.Text.text = $"{localIndex + 1}"; //1,2,3,4
                    cellUI.Button.onClick.AddListener(delegate
                    {
                        Cell_OnClicked(localIndex);
                    });
                }
                
                //
                GridMoveModel gridMoveModel = Context.ModelLocator.GetItem<GridMoveModel>();
                gridMoveModel.GridCellIndex.OnValueChanged.AddListener(
                    (p, c) => GridMoveModel_GridCellIndex_OnValueChanged(p, c));
                gridMoveModel.GridCellIndexIsPending.OnValueChanged.AddListener(
                    (p, c) => GridMoveModel_GridCellIndexIsPending_OnValueChanged(p, c));

                
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


        //  Methods ---------------------------------------
        public Vector3 GetCellUIPositionByIndex(int index)
        { 
            
            //TODO: How do I get the world position from a given cell?
            //CellUI cellUI = _gridUI.CellUIs[index];
            //Canvas canvas = cellUI.GetComponentInParent<Canvas>();
            //RectTransform rectTransform = cellUI.GetComponent<RectTransform>();
            //Vector3 viewportPoint = Camera.main.WorldToViewportPoint(rectTransform.position);
            
            //HACK: put some temp hardcoded values in a list
            float y = 0;
            float z = -1.37f;
            
            List<Vector3> positions = new List<Vector3>();
            positions.Add(new Vector3(-3, y, z));
            positions.Add(new Vector3(-2, y, z));
            positions.Add(new Vector3(-1, y, z));
            positions.Add(new Vector3(-0, y, z));
            positions.Add(new Vector3(1, y, z));


            return positions[index];
        }
        
        //  Event Handlers --------------------------------
        private void Cell_OnClicked(int cellIndex)
        {
            OnCellClick.Invoke(cellIndex);
        }
        
        
        
        private void GridMoveModel_GridCellIndexIsPending_OnValueChanged(
            bool previousValue, bool currentValue)
        {
            RequireIsInitialized();

            //Toggle clickability of all cells when the 
            //player is in motion
            foreach (CellUI cellUI in _gridUI.CellUIs)
            {
                cellUI.IsInteractable = !currentValue;
            }
            


        }
        private void GridMoveModel_GridCellIndex_OnValueChanged(
            int previousValue, int currentValue)
        {
            RequireIsInitialized();
            
            //Sometimes the model sets GridCellIndex without 
            //the user clicking. Good. Be sure to make it look clicked.
            CellUI cellUI = _gridUI.CellUIs[currentValue];
            cellUI.Button.Select();
        }
        
    }
}