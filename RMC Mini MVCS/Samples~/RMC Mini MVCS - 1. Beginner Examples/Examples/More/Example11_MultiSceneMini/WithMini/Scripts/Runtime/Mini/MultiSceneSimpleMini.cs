using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.View;
using RMC.Core.Architectures.Mini.Structure.Simple;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// See <see cref="SimpleMiniMvcs{TContext,TModel,TView,TController,TService}"/>"/>
    /// </summary>
    public class MultiSceneSimpleMini: SimpleMiniMvcs
            <Context, 
            MultiSceneModel, 
            MultiSceneViewBase, 
            MultiSceneController,
            MultiSceneService>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------


        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                //NOTE: The Controller and View are not used yet
                _context = new Context();
                _model = new MultiSceneModel();
                _service = new MultiSceneService();
                
                //
                _model.Initialize(_context);
                _service.Initialize(_context);
            }
        }

        //  Methods ---------------------------------------
        public void AddFeaturesForScene01(MultiSceneView01 view)
        {
            RequireIsInitialized();

            // Remove Old
            if (_view != null)
            {
                GameObject.Destroy(_view.gameObject);
                _view = null;
                _controller = null;
                return;
            }
            
            //Add New
            _view = view;
            _view.Initialize(_context);
            
            //
            _controller = new MultiSceneController(  _model, _view, _service);
            _controller.Initialize(_context);
        }
        
        public void AddFeaturesForScene02(MultiSceneView02 view)
        {
            RequireIsInitialized();

            // Remove Old
            if (_view != null)
            {
                GameObject.Destroy(_view.gameObject);
                _view = null;
                _controller = null;
                return;
            }
            
            //Add New
            _view = view;
            _view.Initialize(_context);
            
            //
            _controller = new MultiSceneController(  _model, _view, _service);
            _controller.Initialize(_context);
        }
        
        //  Event Handlers --------------------------------
    }
}