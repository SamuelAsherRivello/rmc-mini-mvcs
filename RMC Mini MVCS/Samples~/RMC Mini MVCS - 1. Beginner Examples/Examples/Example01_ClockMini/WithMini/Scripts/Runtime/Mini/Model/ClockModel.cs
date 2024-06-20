using RMC.Core.Observables;
using RMC.Mini.Model;

namespace RMC.Mini.Samples.Clock.WithMini.Mini.Model
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class ClockModel: BaseModel // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        public Observable<int> TimeDelay { get { return _timeDelay;} }
        public Observable<float> Time { get { return _time;} }
        
        
        //  Fields ----------------------------------------
        private readonly Observable<int> _timeDelay = new Observable<int>();
        private readonly Observable<float> _time = new Observable<float>();
        
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Set Defaults
                _time.Value = 0;
                _timeDelay.Value = 0;
            }
        }
    }
}