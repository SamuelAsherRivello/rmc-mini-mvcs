using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.View;
using UnityEngine.SceneManagement;

namespace RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class MultiSceneController: BaseController // Extending 'base' is optional
        <MultiSceneModel, MultiSceneViewBase, MultiSceneService> 
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        public MultiSceneController(
            MultiSceneModel model, MultiSceneViewBase view, MultiSceneService service) 
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
                _view.OnRequestSceneChange.AddListener(View_OnRequestSceneChange);
                
                //
                _service.OnLoadCompleted.AddListener(Service_OnLoadCompleted);
                _service.Load();
            }
        }



        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
        private void View_OnRequestSceneChange()
        {
            RequireIsInitialized();
            
            _model.ClickCount.Value++;
            
            if (_model.SceneName.Value == MultiSceneModel.MultiSceneMiniExample01)
            {
                _model.SceneName.Value = MultiSceneModel.MultiSceneMiniExample02;
            }
            else if (_model.SceneName.Value == MultiSceneModel.MultiSceneMiniExample02)
            {
                _model.SceneName.Value = MultiSceneModel.MultiSceneMiniExample01;
            }
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