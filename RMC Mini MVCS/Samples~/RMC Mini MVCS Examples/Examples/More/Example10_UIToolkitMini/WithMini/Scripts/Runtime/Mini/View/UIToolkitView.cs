using System;
using System.Collections.Generic;
using System.Linq;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.UIElements;

// ReSharper disable Unity.NoNullPropagation
namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class UIToolkitView: MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        
        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized;} }
        public IContext Context { get { return _context;} }
        public Button HairButton { get { return _uiDocument.rootVisualElement.Q<Button>("HairButton");} }
        public Button FaceButton { get { return _uiDocument.rootVisualElement.Q<Button>("FaceButton");} }
        public Button ShirtButton { get { return _uiDocument.rootVisualElement.Q<Button>("ShirtButton");} }
        public Button BodyButton { get { return _uiDocument.rootVisualElement.Q<Button>("BodyButton");} }
        public Button ResetButton { get { return _uiDocument.rootVisualElement.Q<Button>("ResetButton");} }
        public Button RandomizeButton { get { return _uiDocument.rootVisualElement.Q<Button>("RandomizeButton");} }

        //
        public Sprite HairSprite
        {
            get
            {
                return _uiDocument.GetSpriteForVisualElement("HairVisualElement");
            }
            set
            {

                _uiDocument.SetSpriteForVisualElementAsync("HairVisualElement", value,
                    "vfx-opacity-0", "vfx-opacity-1", 0.2f);
            }
        }
        
        public Sprite FaceSprite
        {
            get
            {
                return _uiDocument.GetSpriteForVisualElement("FaceVisualElement");
            }
            set
            {
                _uiDocument.SetSpriteForVisualElementAsync("FaceVisualElement", value,
                    "vfx-opacity-0", "vfx-opacity-1", 0.2f);
            }
        }
        
        public Sprite ShirtSprite
        {
            get
            {
                return _uiDocument.GetSpriteForVisualElement("ShirtVisualElement");
                
            }
            set
            {
                _uiDocument.SetSpriteForVisualElementAsync("ShirtVisualElement", value,
                    "vfx-opacity-0", "vfx-opacity-1", 0.2f);
            }
        }
        
        public Sprite BodySprite
        {
            get
            {
                return _uiDocument.GetSpriteForVisualElement("BodyVisualElement");
            }
            set
            {
                //NOTE: Do not animate. It looks odd if the 'skin' is missing for a moment.
                _uiDocument.SetSpriteForVisualElement("BodyVisualElement", value);
            }
        }
        
        //
        public List<Sprite> HairSprites { get { return _hairSprites; }}
        public List<Sprite> FaceSprites { get { return _faceSprites; }}
        public List<Sprite> ShirtSprites { get { return _shirtSprites; }}
        public List<Sprite> BodySprites { get { return _bodySprites; }}
        
        
        //  Fields ----------------------------------------
        private bool _isInitialized = false;
        private IContext _context;

        [SerializeField] 
        private UIDocument _uiDocument;

        [SerializeField] 
        private List<Sprite> _hairSprites = new List<Sprite>();
        
        [SerializeField] 
        private List<Sprite> _faceSprites = new List<Sprite>();
        
        [SerializeField] 
        private List<Sprite> _shirtSprites = new List<Sprite>();
        
        [SerializeField] 
        private List<Sprite> _bodySprites = new List<Sprite>();
        
        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
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
        
        //  Event Handlers --------------------------------
    }
}