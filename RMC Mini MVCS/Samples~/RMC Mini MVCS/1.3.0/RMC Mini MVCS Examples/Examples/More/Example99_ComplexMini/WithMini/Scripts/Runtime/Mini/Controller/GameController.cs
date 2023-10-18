using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.View;
using UnityEngine.SceneManagement;

namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class GameController: BaseController // Extending 'base' is optional
        <ComplexModel, GameView, ComplexService> 
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        public GameController(
            ComplexModel model, GameView view, ComplexService service) 
            : base(model, view, service)
        {
            
        }

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                //
                _model.SceneName.Value = SceneManager.GetActiveScene().name;
                
                //
                _view.OnScorePoints.AddListener(View_OnScorePoints);
                _view.OnReset.AddListener(View_OnReset);
                
                //
                _service.OnLoadCompleted.AddListener(Service_OnLoadCompleted);
                _service.Load();
            }
        }



        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
        private void View_OnScorePoints()
        {
            RequireIsInitialized();
            
            _model.Score.Value++;
        }
        
        private void View_OnReset()
        {
            RequireIsInitialized();

            _model.Score.Value = _model.ScoreMin.Value;
        }
        
            
        private void Service_OnLoadCompleted(string message)
        {
            RequireIsInitialized();
            
            _model.ServiceHasLoaded.Value = true;

            // Demo Only. We don't use this value. But you could.
            _model.Message.Value = message;
        }

    }
}