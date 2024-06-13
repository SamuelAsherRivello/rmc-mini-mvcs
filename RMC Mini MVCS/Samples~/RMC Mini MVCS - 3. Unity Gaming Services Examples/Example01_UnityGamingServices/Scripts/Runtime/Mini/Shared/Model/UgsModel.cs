using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service;
using RMC.Core.Data.Types;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.Model
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class UgsModel: BaseModel // Extending 'base' is optional
    {
        //  Properties (Authentication) ------------------------------------
        public Observable<AuthenticationService> AuthenticationService { get { return _authenticationService;} }
        public Observable<bool> IsSignedIn { get { return _isSignedIn;} }
        
        
        //  Properties (Navigation) ------------------------------------
        public Observable<bool> SceneHasNavigationBack { get { return _sceneHasNavigationBack;} }
        public Observable<bool> SceneHasNavigationDeveloperConsole { get { return _sceneHasNavigationDeveloperConsole;} }
        public Observable<bool> SceneHasAutoAuthentication { get { return _sceneHasAutoAuthentication;} }

        //  Fields ----------------------------------------
        private readonly Observable<AuthenticationService> _authenticationService = new Observable<AuthenticationService>();
        private readonly Observable<bool> _isSignedIn = new Observable<bool>();
        private readonly Observable<bool> _sceneHasNavigationBack = new Observable<bool>();
        private readonly Observable<bool> _sceneHasNavigationDeveloperConsole = new Observable<bool>();
        private readonly Observable<bool> _sceneHasAutoAuthentication = new Observable<bool>();
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _authenticationService.Value = null;
                _sceneHasNavigationBack.Value = false;
                _sceneHasNavigationDeveloperConsole.Value = false;
                _sceneHasAutoAuthentication.Value = false;
                
            }
        }

        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
    }
}