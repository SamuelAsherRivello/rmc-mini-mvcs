using RMC.Mini.Samples.Configurator.Mini.Model.Data;
using UnityEngine;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace RMC.Mini.Samples.Configurator.Standard.Gameplay
{
    /// <summary>
    /// Represents the 3D Graphics in front of the <see cref="Environment"/>
    /// </summary>
    public class Player : MonoBehaviour
    {
        //  Properties ------------------------------------

        public CharacterData CharacterData
        {
            get { return _characterData; }
            set
            {
                _characterData = value;

                if (_head == null)
                {
                    return;
                }
                CustomColorUtility.SetColorAsync(_head.material, _characterData.HeadColor, CustomColorUtility.DefaultDuration);
                CustomColorUtility.SetColorAsync(_chest.material, _characterData.ChestColor, CustomColorUtility.DefaultDuration);
                CustomColorUtility.SetColorAsync(_legs.material, _characterData.LegsColor, CustomColorUtility.DefaultDuration);
            }
        }

        
        //  Fields ----------------------------------------

        public bool IsPlayerEnabled { get; set; } = false;

        [SerializeField]
        private Renderer _head;

        [SerializeField]
        private Renderer _chest;

        [SerializeField]
        private Renderer _legs;

        [SerializeField]
        private float _angularSpeed = 100f;

        [SerializeField]
        private float _linearSpeed = 5f;

        private CharacterData _characterData;

        //  Unity Methods ---------------------------------

        private void Update()
        {
            if (!IsPlayerEnabled)
            {
                return;
            }
            
            HandleMovement();
        }
        
        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }
    
        //  Methods ---------------------------------
        private void HandleMovement()
        {
            float rotation = Input.GetAxis("Horizontal") * _angularSpeed * Time.deltaTime;
            float movement = Input.GetAxis("Vertical") * _linearSpeed *  Time.deltaTime;
            
            transform.Rotate(0, rotation, 0);
            transform.Translate(0, 0, -movement);
        }
    }
}
