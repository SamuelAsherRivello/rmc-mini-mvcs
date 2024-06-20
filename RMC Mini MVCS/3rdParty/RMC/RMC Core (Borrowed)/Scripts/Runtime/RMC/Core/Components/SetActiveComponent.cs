using UnityEngine;

namespace RMC.Core.Components
{
    /// <summary>
    /// Toggle the Active for a component.
    ///
    /// Useful if you want something invisible in the editor (for less clutter)
    /// but visible at runtime. Or vice-versa.
    /// </summary>
    public class SetActiveComponent : MonoBehaviour
    {
        //  Properties ------------------------------------
        public bool IsActiveOnAwake { get { return _isActiveOnAwake; } }

		
        //  Fields ----------------------------------------
        [SerializeField]
        private bool _isActiveOnAwake = true;

        
        //  Unity Methods----------------------------------
        protected void Awake()
        {
            gameObject.SetActive(_isActiveOnAwake);
        }


        //  General Methods -------------------------------

		
        //  Event Handlers --------------------------------
    }
}