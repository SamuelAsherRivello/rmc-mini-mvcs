using System.Collections.Generic;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.View.UI
{
    //  Namespace Properties ------------------------------
    
    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Add comment
    /// </summary>
    public class GridUI: MonoBehaviour
    {
        //  Events ----------------------------------------
        
        //  Properties ------------------------------------
        public List<CellUI> CellUIs { get { return _cellUIs;}}
        
        //  Fields ----------------------------------------
        [SerializeField] private List<CellUI> _cellUIs;

        //  Unity Methods ---------------------------------

        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}