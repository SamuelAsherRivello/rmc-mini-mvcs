using System;
using System.Collections.Generic;
using System.Reflection;
using RMC.Mini.Experimental.DependencyInjectionSystem.Attributes;
using UnityEngine;

namespace RMC.Mini.Experimental.DependencyInjectionSystem
{
    
    /// <summary>
    /// The DependencyInjectorSystem is a singleton that can
    /// be used to inject dependencies into MonoBehaviours
    /// </summary>
    public class DependencyInjectorSystem
    {
        public static DependencyInjectorSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DependencyInjectorSystem();
                }

                return _instance;
            }
        }

        private static DependencyInjectorSystem _instance;
        private readonly Dictionary<Type, object> _dependencies;

        private DependencyInjectorSystem()
        {
            _dependencies = new Dictionary<Type, object>();
        }
    
        
        /// <summary>
        ///  Bind a dependency to a type
        /// </summary>
        /// <param name="someService"></param>
        /// <typeparam name="T"></typeparam>
        public void Bind<T>(T someService)
        {
            if (!_dependencies.ContainsKey(typeof(T)))
            {
                _dependencies[typeof(T)] = someService;
            }
            else
            {
                Debug.LogWarning($"Dependency already found for type {typeof(T).Name}.");
            }
        }

        
        /// <summary>
        /// Inject all dependencies into all MonoBehaviours
        /// </summary>
        public void InjectAll()
        {
            foreach (var monoBehaviour in UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                InjectAllInto(monoBehaviour);
            }
        }
        
        private void InjectAllInto(MonoBehaviour monoBehaviour)
        {
            foreach (var field in monoBehaviour.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (Attribute.IsDefined(field, typeof(InjectAttribute)))
                {
                    Type fieldType = field.FieldType;
                    if (_dependencies.TryGetValue(fieldType, out var dependency))
                    {
                        if (field.GetValue(monoBehaviour) == null)
                        {
                            field.SetValue(monoBehaviour, dependency);
                        }
                   
                    }
                    else
                    {
                        Debug.LogWarning($"[Inject] used, but  no dependency found for type {fieldType.Name} in {monoBehaviour.name}");
                    }
                }
            }
        }
  
    }

}