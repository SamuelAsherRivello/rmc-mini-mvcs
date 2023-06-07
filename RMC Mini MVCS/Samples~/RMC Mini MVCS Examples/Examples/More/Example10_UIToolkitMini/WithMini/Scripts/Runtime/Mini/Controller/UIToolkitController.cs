using System.Collections.Generic;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class UIToolkitController: BaseController // Extending 'base' is optional
        <UIToolkitModel, UIToolkitView, UIToolkitService> 
    {

        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        
        public UIToolkitController(UIToolkitModel model, UIToolkitView view, UIToolkitService service) 
            : base(model, view, service)
        {
            
        }

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Model
                _model.HairIndex.OnValueChanged.AddListener(delegate(int previousValue, int currentValue)
                {
                    _view.HairSprite = _view.HairSprites[currentValue];
                });
                
                _model.FaceIndex.OnValueChanged.AddListener(delegate(int previousValue, int currentValue)
                {
                    _view.FaceSprite = _view.FaceSprites[currentValue];
                });
                
                _model.ShirtIndex.OnValueChanged.AddListener(delegate(int previousValue, int currentValue)
                {
                    _view.ShirtSprite = _view.ShirtSprites[currentValue];
                });
                
                _model.BodyIndex.OnValueChanged.AddListener(delegate(int previousValue, int currentValue)
                {
                    _view.BodySprite = _view.BodySprites[currentValue];
                });
                
                // View
                _view.HairButton.clickable.clicked += delegate {
                    View_OnClickedHairButton(_view.HairSprites, _model.HairIndex); };
                
                _view.FaceButton.clickable.clicked += delegate {
                    View_OnClickedHairButton(_view.FaceSprites, _model.FaceIndex); };
                
                _view.ShirtButton.clickable.clicked += delegate {
                    View_OnClickedHairButton(_view.ShirtSprites, _model.ShirtIndex); };
                
                _view.BodyButton.clickable.clicked += delegate {
                    View_OnClickedHairButton(_view.BodySprites, _model.BodyIndex); };
                
                // Controller
                _view.RandomizeButton.clickable.clicked += delegate {
                    context.CommandManager.InvokeCommand(new RandomizeCommand(_model, _view)); };
                
                _view.ResetButton.clickable.clicked += delegate {
                    context.CommandManager.InvokeCommand(new ResetCommand(_model)); };
                
                _service.OnLoaded.AddListener(Service_OnLoaded);
                _service.Load();
            }
        }

        private void Service_OnLoaded(CharacterData characterData)
        {
            // Store it, JUST in case of 'reset'
            _model.InitialCharacterData = characterData;
            
            // Use it
            _model.HairIndex.Value = _model.InitialCharacterData.HairIndex;
            _model.FaceIndex.Value = _model.InitialCharacterData.FaceIndex;
            _model.ShirtIndex.Value = _model.InitialCharacterData.ShirtIndex;
            _model.BodyIndex.Value = _model.InitialCharacterData.BodyIndex;
        }

        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
        private void View_OnClickedHairButton(List<Sprite> sprites, Observable<int> indexObservable)
        {
            RequireIsInitialized();

            int nextValue = indexObservable.Value + 1;
            if (nextValue >= sprites.Count)
            {
                nextValue = 0;
            }
            indexObservable.Value = nextValue;
        }
    }
}