using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.BouncyBall.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class BouncyBallModel: BaseModel // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<int> BounceCount { get { return _bounceCount;} }
        public Observable<int> BounceCountMax { get { return _bounceCountMax;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<int> _bounceCount = new Observable<int>();
        private readonly Observable<int> _bounceCountMax = new Observable<int>();

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _bounceCount.Value = 0;
                _bounceCountMax.Value = 0;
            }
        }
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}