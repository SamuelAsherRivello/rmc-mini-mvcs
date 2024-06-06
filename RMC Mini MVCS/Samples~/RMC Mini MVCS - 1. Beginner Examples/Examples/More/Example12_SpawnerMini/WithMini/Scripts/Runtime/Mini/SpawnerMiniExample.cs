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

        private Context _context;
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            _context = new Context();
            
            // LEFT
            SpawnerSimpleMini spawnerSimpleMini = new SpawnerSimpleMini(_context, _leftView, _rightViewPrefab);
            spawnerSimpleMini.Initialize();
            
        }


        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}