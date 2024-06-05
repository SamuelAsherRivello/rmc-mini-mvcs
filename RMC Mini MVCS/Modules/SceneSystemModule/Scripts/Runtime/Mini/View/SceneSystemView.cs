using System;
using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using RMC.Core.DesignPatterns.Creational.Singleton.CustomSingletonMonobehaviour;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Core.Architectures.Mini.Modules.SceneSystemModule
{
    /// <summary>
    /// The View handles user interface and user input
    ///
    /// Relates to the <see cref="SceneSystemController"/>
    /// 
    /// </summary>
    public class SceneSystemView: SingletonMonobehaviour<SceneSystemView>, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] 
        public readonly UnityEvent OnBack = new UnityEvent();
        
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }

        private VisualElement ScreenVisualElement { get { return _uiDocument?.rootVisualElement.Q<VisualElement>("Screen");  } }
        private bool WillBlockUIDuringTransition { get { return _willBlockUIDuringTransition; } }
        
        public StyleColor ScreenBackgroundColor
        {
            get
            {
                return ScreenVisualElement.style.backgroundColor;
            }
            
            set
            {
                ScreenVisualElement.style.backgroundColor = value;
            }
        }

        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [Header("UI")]
        [SerializeField]
        private UIDocument _uiDocument;

        [Header("Settings")]
        [SerializeField]
        private float _transitionDurationSeconds = 0.25f;

        [SerializeField]
        private Color _transitionColor = Color.black;

        [SerializeField] 
        private bool _willBlockUIDuringTransition = true;

        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                _uiDocument.enabled = true;
            }
        }

        
        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }
        
        
        //  Unity Methods ---------------------------------


        //  Methods ---------------------------------------
        public async Task FadeScreenOnAsync()
        {
            if (_willBlockUIDuringTransition)
            {
                ScreenVisualElement.pickingMode = PickingMode.Position;
            }
            await SetColorAsync(_transitionColor, _transitionDurationSeconds/2);
        }
        
        public async Task FadeScreenOffAsync()
        {
            Color toColor = _transitionColor;
            toColor.a = 0;
            await SetColorAsync(toColor, _transitionDurationSeconds/2);
            
            if (_willBlockUIDuringTransition)
            {
                ScreenVisualElement.pickingMode = PickingMode.Ignore;
            }
        }
        
        private async Task SetColorAsync(Color toColor, float durationInSeconds)
        {
            float startTime = Time.time;
            float endTime = startTime + durationInSeconds;
            Color fromColor = ScreenBackgroundColor.value;

            while (Time.time < endTime)
            {
                float elapsed = Time.time - startTime;
                float percent = Mathf.Clamp01(elapsed / durationInSeconds);
                var percentEase = Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, percent));
                ScreenBackgroundColor = new StyleColor(Color.Lerp(fromColor, toColor, percentEase));
                await Task.Yield();
            }
            
            // Ensure the final color is set to the target color
            ScreenBackgroundColor = new StyleColor(toColor);
        }
        
        //  Event Handlers --------------------------------
        

    }
}