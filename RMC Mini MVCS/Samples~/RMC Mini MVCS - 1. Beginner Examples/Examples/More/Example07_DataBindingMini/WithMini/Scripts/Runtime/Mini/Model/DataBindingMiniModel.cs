using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Data.Types;

namespace RMC.Core.Architectures.Mini.Samples.DataBindingMini.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class DataBindingMiniModel: BaseModel // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<string> Message { get { return _message;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<string> _message = new Observable<string>();

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _message.Value = "Edit This Text :)";
            }
        }
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}