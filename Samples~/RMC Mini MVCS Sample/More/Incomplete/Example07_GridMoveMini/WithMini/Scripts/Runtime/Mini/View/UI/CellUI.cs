using UnityEngine;
using UnityEngine.UI;

namespace RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.View.UI
{
    //  Namespace Properties ------------------------------
    
    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Add comment
    /// </summary>
    public class CellUI: MonoBehaviour
    {
        //  Events ----------------------------------------
        
        
        //  Properties ------------------------------------
        public Button Button { get { return _button;} }
        public Text Text { get { return _text;} }
        public bool IsInteractable 
        {
            get
            {
                return Button.IsInteractable();
            }
            set
            {
                Button.interactable = value;
            }
        }

        //  Fields ----------------------------------------
        [SerializeField] private Button _button;
        [SerializeField] private Text _text;
        //  Unity Methods ---------------------------------


        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
    }
}