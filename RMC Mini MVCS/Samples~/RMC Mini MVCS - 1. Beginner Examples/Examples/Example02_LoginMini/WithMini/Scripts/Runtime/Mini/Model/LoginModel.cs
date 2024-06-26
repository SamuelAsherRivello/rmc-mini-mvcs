using RMC.Core.Observables;
using RMC.Mini.Model;

namespace RMC.Mini.Samples.Login.WithMini.Mini.Model
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class LoginModel: BaseModel // Extending 'base' is optional
    {
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