using RMC.Core.Experimental.Systems.DependencyInjectionSystem;
using RMC.Core.Experimental.Systems.DependencyInjectionSystem.Attributes;
using UnityEngine;

namespace RMC.IntroToUnity.Examples
{
    /// <summary>
    /// The Example is the main entry point to the demo
    ///
    /// This experimental system allows you to inject dependencies. The relevance for
    /// RMC Mini MVCS is that it allows you to inject any IConcern
    /// </summary>
    public class DependencyInjectionExample: MonoBehaviour
    {
        [Inject]
        private SampleService1 _sampleService1;
        
        [Inject]
        private SampleService2 _sampleService2;
        
        protected void Awake () 
        {
            Debug.Log($"This Scene has no UI. It has only console logging.");
            
            // Call at any time before Inject()
            DependencyInjectorSystem.Instance.Bind<SampleService1>(new SampleService1());
            DependencyInjectorSystem.Instance.Bind<SampleService2>(new SampleService2());
            
            // Call at anytime (repeatedly is ok) before referencing vars
            DependencyInjectorSystem.Instance.InjectAll();
            Debug.Log("_sampleService1: " + _sampleService1);
            Debug.Log("_sampleService2: " + _sampleService2);
        }

    }
    
    
    public class SampleService1
    {
        public override string ToString()
        {
            return $"[{this.GetType().Name}]";
        }
    }
    
    public class SampleService2
    {
        public override string ToString()
        {
            return $"[{this.GetType().Name}]";
        }
    }
}