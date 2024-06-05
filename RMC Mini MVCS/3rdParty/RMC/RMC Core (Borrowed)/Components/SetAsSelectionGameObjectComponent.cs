#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#pragma warning disable CS0414
namespace RMC.Core.Components
{
    /// <summary>
    /// Set the <see cref="Selection"/> to a
    /// given <see cref="GameObject"/>. 
    /// </summary>
    public class SetAsSelectionGameObjectComponent : MonoBehaviour
    {
        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------
        [SerializeField]
        private bool _isSelectionActiveGameObject = true;

        
        //  Unity Methods----------------------------------
        protected void Awake()
        {
            
            
#if UNITY_EDITOR
            if (_isSelectionActiveGameObject)
            {
                Selection.activeGameObject = gameObject;
            }
#endif
      
            
        }


        //  General Methods -------------------------------

		
        //  Event Handlers --------------------------------
    }
}