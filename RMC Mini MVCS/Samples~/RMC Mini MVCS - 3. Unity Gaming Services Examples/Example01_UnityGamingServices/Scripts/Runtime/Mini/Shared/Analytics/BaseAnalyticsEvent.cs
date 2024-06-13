
using Unity.Services.Analytics;

namespace RMC.Core.Architectures.Mini.Samples.UGS.Mini.Service
{
    public abstract class BaseAnalyticsEvent : CustomEvent
    {
        public string Name { get; }
        protected BaseAnalyticsEvent(string name) : base(name)
        {
            Name = name;
        }
    }
    
    public class SceneVisitEvent : BaseAnalyticsEvent
    {
        //  Properties ------------------------------------
        
        //This wrapper makes it 'easier' to set parameters
        public string SceneName { set { SetParameter(nameof(SceneName), value); } }
        
        public SceneVisitEvent(string sceneName) : base(nameof(SceneVisitEvent))
        {
            SceneName = sceneName;
        }
    }
}