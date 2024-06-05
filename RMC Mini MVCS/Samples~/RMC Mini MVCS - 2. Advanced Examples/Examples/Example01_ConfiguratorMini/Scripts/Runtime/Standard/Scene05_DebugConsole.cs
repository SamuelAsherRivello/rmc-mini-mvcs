using RMC.Core.Architectures.Mini.Modules.SceneSystemModule;
using RMC.MiniMvcs.Samples.Configurator.Mini;
using RMC.MiniMvcs.Samples.Configurator.Mini.Feature;
using RMC.MiniMvcs.Samples.Configurator.Mini.View;
using UnityEngine;

namespace RMC.MiniMvcs.Samples.Configurator.Standard
{
    /// <summary>
    /// This is the main entry point to one of the scenes
    /// </summary>
    public class Scene05_DebugConsole : MonoBehaviour
    {
        //  Fields ----------------------------------------
        
        [SerializeField] 
        private HudView _hudView;
        
        [SerializeField] 
        private DebugConsoleView _debugConsoleView;

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
            DebugConsoleFeature debugConsoleFeature = new DebugConsoleFeature();
            debugConsoleFeature.AddView(_debugConsoleView);
            mini.AddFeature<DebugConsoleFeature>(debugConsoleFeature);
            
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
            mini.RemoveFeature<DebugConsoleFeature>();
            
            //  Scene-Agnostic ----------------------------
            mini.RemoveFeature<HudFeature>();
            
        }
        
        //  Event Handlers --------------------------------
    }
}