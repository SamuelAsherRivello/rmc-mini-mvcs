using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Data.Types;

namespace RMC.Core.Architectures.Mini.Samples.CountUp.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class CountUpModel: BaseModel // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<int> Counter { get { return _counter;} }

        
        //  Fields ----------------------------------------
        private readonly Observable<int> _counter = new Observable<int>();

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Set Defaults
                _counter.Value = 0;
            }
        }
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}