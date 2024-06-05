using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.SpawnerMini.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class SpawnerMiniModel: BaseModel // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<int> Counter { get { return _counter;} }
        
        public Observable<bool> IsSpawnMode { get { return _isSpawnMode;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<int> _counter = new Observable<int>();
        private readonly Observable<bool> _isSpawnMode = new Observable<bool>();
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _counter.Value = 0;
                _isSpawnMode.Value = true;
            }
        }
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}