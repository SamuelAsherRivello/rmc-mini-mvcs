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
                ScriptableObjectModel.SceneName.Value = SceneManager.GetActiveScene().name;
                
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
            
            ScriptableObjectModel.ClickCount.Value++;
            
            if (ScriptableObjectModel.SceneName.Value == MultiSceneModel.MultiSceneMiniExample01)
            {
                ScriptableObjectModel.SceneName.Value = MultiSceneModel.MultiSceneMiniExample02;
            }
            else if (ScriptableObjectModel.SceneName.Value == MultiSceneModel.MultiSceneMiniExample02)
            {
                ScriptableObjectModel.SceneName.Value = MultiSceneModel.MultiSceneMiniExample01;
            }
        }
        
            
        private void Service_OnLoadCompleted(string message)
        {
            RequireIsInitialized();
            
            ScriptableObjectModel.ServiceHasLoaded.Value = true;

            // Demo Only. We don't use this value. But you could.
            ScriptableObjectModel.Message.Value = message;
        }

    }
}