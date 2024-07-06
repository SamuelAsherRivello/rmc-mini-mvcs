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
    public class Scene04_Ugs_UserGeneratedContent : MonoBehaviour
    {
        //  Fields ----------------------------------------
        [SerializeField] 
        private HudView _hudView;
        
        [SerializeField] 
        private UserGeneratedContentView _userGeneratedContentView;

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
            
            if (mini == null)
            {
                return;
            }
            
            //  Scene-Specific ----------------------------
            UserGeneratedContentFeature userGeneratedContentFeature = new UserGeneratedContentFeature();
            userGeneratedContentFeature.AddView(_userGeneratedContentView);
            mini.AddFeature<UserGeneratedContentFeature>(userGeneratedContentFeature);
            
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
            
            if (mini == null)
            {
                return;
            }
            
            //  Scene-Specific ----------------------------
            mini.RemoveAndDisposeFeature<UserGeneratedContentFeature>();
            
            //  Scene-Agnostic ----------------------------
            mini.RemoveAndDisposeFeature<HudFeature>();
        }
        
        //  Event Handlers --------------------------------
    }
}