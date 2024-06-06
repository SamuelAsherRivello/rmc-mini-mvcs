using RMC.Core.Architectures.Mini.Features.SceneSystem;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Feature;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Standard
{
    /// <summary>
    /// This is the main entry point to one of the scenes
    /// </summary>
    public class Scene05_DeveloperConsole : MonoBehaviour
    {
        //  Fields ----------------------------------------
        
        [SerializeField] 
        private HudView _hudView;
        
        [SerializeField] 
        private DeveloperConsoleView developerConsoleView;

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
            
            //  Scene-Specific ----------------------------
            DeveloperConsoleFeature developerConsoleFeature = new DeveloperConsoleFeature();
            developerConsoleFeature.AddView(developerConsoleView);
            mini.AddFeature<DeveloperConsoleFeature>(developerConsoleFeature);
            
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
            
            //  Scene-Specific ----------------------------
            mini.RemoveFeature<DeveloperConsoleFeature>();
            
            //  Scene-Agnostic ----------------------------
            mini.RemoveFeature<HudFeature>();
            
        }
        
        //  Event Handlers --------------------------------
    }
}