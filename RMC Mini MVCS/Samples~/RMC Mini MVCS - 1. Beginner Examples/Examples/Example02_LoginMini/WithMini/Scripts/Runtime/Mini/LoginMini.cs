using RMC.Mini.Experimental.ContextLocators;
using RMC.Mini.Samples.Login.WithMini.Mini.Controller;
using RMC.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Mini.Samples.Login.WithMini.Mini.Service;
using RMC.Mini.Samples.Login.WithMini.Mini.View;

namespace RMC.Mini.Samples.Login.WithMini.Mini
{
    /// <summary>
    /// See <see cref="SimpleMiniMvcs{TContext,TModel,TView,TController,TService}"/>"/>
    /// </summary>
    public class LoginMini: SimpleMiniMvcs
            <ContextWithLocator, 
            LoginModel, 
            LoginView, 
            LoginController,
            LoginService>
    {

        //  Initialization  -------------------------------
        public LoginMini(LoginView view)
        {
            _view = view;
        
        }
        
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                //
                _context = ContextWithLocator.CreateNew();
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
    }
}