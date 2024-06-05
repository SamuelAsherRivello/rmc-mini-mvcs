using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.View;
using UnityEngine;
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
                _view.OnSave.AddListener(View_OnSave);
                
                //
                _service.OnLoadScoreCompleted.AddListener(Service_OnLoadCompleted);
                _service.LoadScore();
            }
        }

        //  Methods ---------------------------------------
        private void CheckServiceDirty()
        {
            _model.IsServiceDirty.Value = _model.Score.Value != _service.Score;
        }
        
        
        //  Event Handlers --------------------------------
        private void View_OnScorePoints()
        {
            RequireIsInitialized();
            
            // Update model
            _model.Score.Value++;
            
            // Refresh if service has old data or not
            CheckServiceDirty();
        }
        
        private void View_OnReset()
        {
            RequireIsInitialized();

            // Update model
            _model.Score.Value = _model.ScoreMin.Value;
            
            //Refresh if service has old data or not
            CheckServiceDirty();
        }
        
        private void View_OnSave()
        {
            RequireIsInitialized();

            // Update Service
            _service.SaveScore(_model.Score.Value);
            
            //Refresh if service has old data or not
            CheckServiceDirty();
        }
        
            
        private void Service_OnLoadCompleted(int score)
        {
            RequireIsInitialized();
            
            _model.ServiceHasLoaded.Value = true;

            // Update Model
            _model.Score.Value = score;
            
            //Refresh if service has old data or not
            CheckServiceDirty();
        }

    }
}