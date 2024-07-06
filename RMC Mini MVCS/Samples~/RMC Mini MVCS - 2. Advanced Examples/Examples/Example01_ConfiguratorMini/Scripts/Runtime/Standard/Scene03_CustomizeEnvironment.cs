using RMC.Mini.Features.SceneSystem;
using RMC.Mini.Samples.Configurator.Mini;
using RMC.Mini.Samples.Configurator.Mini.Feature;
using RMC.Mini.Samples.Configurator.Mini.View;
using UnityEngine;

namespace RMC.Mini.Samples.Configurator.Standard
{
    /// <summary>
    /// This is the main entry point to one of the scenes
    /// </summary>
    public class Scene03_CustomizeEnvironment : MonoBehaviour
    {
        //  Fields ----------------------------------------
        
        [SerializeField] 
        private HudView _hudView;
        
        [SerializeField] 
        private CustomizeEnvironmentView _customizeEnvironmentView;

        //  Unity Methods  --------------------------------
        protected void Start()
        {
            AddFeature();
        }
        
        protected void OnDestroy()
        {
            RemoveFeature();
            
            // Optional: Handle any cleanup here...
        }

        //  Methods ---------------------------------------
        private void AddFeature()
        {
            ConfiguratorMini mini = ConfiguratorMiniSingleton.Instance.ConfiguratorMini;

            if (mini == null)
            {
                return;
            }
            
            //  Scene-Specific ----------------------------
            CustomizeEnvironmentFeature customizeEnvironmentFeature = new CustomizeEnvironmentFeature();
            customizeEnvironmentFeature.AddView(_customizeEnvironmentView);
            mini.AddFeature<CustomizeEnvironmentFeature>(customizeEnvironmentFeature);
            
            //  Scene-Agnostic ----------------------------
            if (!mini.HasFeature<SceneSystemFeature>())
            {
                SceneSystemFeature sceneSystemFeature = new SceneSystemFeature();
                mini.AddFeature<SceneSystemFeature>(sceneSystemFeature);
            }
            
            HudFeature hudFeature = new HudFeature();
            hudFeature.AddView(_hudView);
            mini.AddFeature<HudFeature>(hudFeature);
        }
        
        private void RemoveFeature()
        {
            if (ConfiguratorMiniSingleton.IsShuttingDown)
            {
                return;
            }
            
            ConfiguratorMini mini = ConfiguratorMiniSingleton.Instance.ConfiguratorMini;
            
            if (mini == null)
            {
                return;
            }
            
            //  Scene-Specific ----------------------------
            mini.RemoveAndDisposeFeature<CustomizeEnvironmentFeature>();
            
            //  Scene-Agnostic ----------------------------
            mini.RemoveAndDisposeFeature<HudFeature>();
            
        }
        
        //  Event Handlers --------------------------------
    }
}