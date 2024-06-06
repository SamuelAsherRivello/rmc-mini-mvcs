using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Data.Types;

namespace RMC.Core.Architectures.Mini.Samples.Calculator.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class CalculatorModel : BaseModel
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<int> A { get { return _a;} }
        public Observable<int> B { get { return _b;} }
        public Observable<int> Result { get { return _result;} }

        
        //  Fields ----------------------------------------
        private Observable<int> _a = new Observable<int>();
        private Observable<int> _b = new Observable<int>();
        private Observable<int> _result = new Observable<int>();

        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _a.Value = 0;
                _b.Value = 0;
                _result.Value = 0;
            }
        }

        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
    }
}