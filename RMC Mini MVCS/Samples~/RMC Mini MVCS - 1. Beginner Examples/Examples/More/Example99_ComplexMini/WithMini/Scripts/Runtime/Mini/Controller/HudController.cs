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
    public class HudController: BaseController // Extending 'base' is optional
        <ComplexModel, HudView, ComplexService> 
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        public HudController(
            ComplexModel model, HudView view, ComplexService service) 
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
                _view.OnBack.AddListener(View_OnBack);
                
                //
                _service.OnLoadScoreCompleted.AddListener(Service_OnLoadCompleted);
    
            }
        }



        //  Methods ---------------------------------------
        public void Load()
        {
            RequireIsInitialized();
            _service.LoadScore();
        }
        
        //  Event Handlers --------------------------------
        private void View_OnBack()
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
            
            //Do nothing...
        }
    }
}