using RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class SpawnerMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        [SerializeField] 
        private LeftView _leftView;

        [SerializeField] 
        private RightView _rightViewPrefab;

        private Context.Context _context;
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            _context = new Context.Context();
            
            // LEFT
            SpawnerMini spawnerMini = new SpawnerMini(_context, _leftView, _rightViewPrefab);
            spawnerMini.Initialize();
            
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}