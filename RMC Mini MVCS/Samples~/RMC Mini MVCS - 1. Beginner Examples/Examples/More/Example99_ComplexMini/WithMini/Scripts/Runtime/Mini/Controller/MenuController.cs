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
    public class MenuController: BaseController // Extending 'base' is optional
        <ComplexModel, MenuView, ComplexService> 
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        public MenuController(
            ComplexModel model, MenuView view, ComplexService service) 
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
                _view.OnBack.AddListener(View_OnRequestSceneChange);
                
                //
                _service.OnLoadScoreCompleted.AddListener(Service_OnLoadCompleted);
                _service.LoadScore();
            }
        }



        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
        private void View_OnRequestSceneChange()
        {
            RequireIsInitialized();
            
            if (_model.SceneName.Value == ComplexModel.MultiSceneMiniExample01)
            {
                _model.SceneName.Value = ComplexModel.MultiSceneMiniExample02;
            }
            else if (_model.SceneName.Value == ComplexModel.MultiSceneMiniExample02)
            {
                _model.SceneName.Value = ComplexModel.MultiSceneMiniExample01;
            }
        }
        
            
        private void Service_OnLoadCompleted(int score)
        {
            RequireIsInitialized();
            
            _model.ServiceHasLoaded.Value = true;

            //s
            _model.Score.Value = score;
            
        }
    }
}