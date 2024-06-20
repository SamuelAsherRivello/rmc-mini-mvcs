using RMC.Mini.Features.SceneSystem;
using RMC.Mini.Samples.UGS.Mini;
using RMC.Mini.Samples.UGS.Mini.Feature;
using RMC.Mini.Samples.UGS.Mini.View;
using UnityEngine;

namespace RMC.Mini.Samples.UGS.Standard
{
    /// <summary>
    /// This is the main entry point to one of the scenes
    /// </summary>
    public class Scene01_Ugs_Menu : MonoBehaviour
    {
        //  Fields ----------------------------------------
        [SerializeField] 
        private HudView _hudView;
       
        [SerializeField] 
        private MenuView _menuView;
        
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
            UgsMini mini = UgsMiniSingleton.Instance.UgsMini;
            
            //  Scene-Specific ----------------------------
            MenuFeature menuFeature = new MenuFeature();
            menuFeature.AddView(_menuView);
            mini.AddFeature<MenuFeature>(menuFeature);
            
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
            if (UgsMiniSingleton.IsShuttingDown)
            {
                return;
            }
            UgsMini mini = UgsMiniSingleton.Instance.UgsMini;
            
            //  Scene-Specific ----------------------------
            mini.RemoveFeature<MenuFeature>();
            
            //  Scene-Agnostic ----------------------------
            mini.RemoveFeature<HudFeature>();
        }
        
        //  Event Handlers --------------------------------
    }
}