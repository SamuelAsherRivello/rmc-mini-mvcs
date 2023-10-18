using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class LoginModel: BaseModel // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<UserData> LoggedInUserData { get { return _loggedInUserData;} }
        public bool IsLoggedIn { get { return LoggedInUserData.Value != null;} }
        public Observable<bool> CanLogin { get { return _canLogin;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<bool> _canLogin = new Observable<bool>();
        private readonly Observable<UserData> _loggedInUserData = new Observable<UserData>();

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _canLogin.Value = true;
                _loggedInUserData.Value = null;
            }
        }
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}