using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;
using RMC.Core.Architectures.Mini.Structure.Simple;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// See <see cref="SimpleMiniMvcs{TContext,TModel,TView,TController,TService}"/>"/>
    /// </summary>
    public class LoginSimpleMini: SimpleMiniMvcs
            <Context, 
            LoginModel, 
            LoginView, 
            LoginController,
            LoginService>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------


        //  Initialization  -------------------------------
        public LoginSimpleMini(LoginView view)
        {
            _view = view;
        
        }
        
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                //
                _context = new Context();
                _model = new LoginModel();
                _service = new LoginService();
                _controller = new LoginController(_model, _view, _service);
                
                //
                _model.Initialize(_context);
                _view.Initialize(_context);
                _service.Initialize(_context);
                _controller.Initialize(_context);
            }
        }

        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
    }
}