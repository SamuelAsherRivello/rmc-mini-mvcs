using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Controller;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.Service;
using RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini.View;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;
using RMC.Core.Experimental.Architectures.Mini;

namespace RMC.Core.Architectures.Mini.Samples.Complex.WithMini.Mini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The ComplexMini is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    /// </summary>
    public class ComplexMini: MiniMvcsComplex
            <Context.Context, 
                IModel, 
                IView, 
                IController,
                IService>
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------


        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                
                _context = new Context.Context();

                //
                ComplexModel model = new ComplexModel();
                ComplexService service = new ComplexService();
                    
                // ModelLocator is created in superclass
                _viewLocator = new Locator<IView>();
                _controllerLocator = new Locator<IController>();
                _serviceLocator = new Locator<IService>();
                
                // Model item is already added in superclass
                ServiceLocator.AddItem(service);
                
                //
                model.Initialize(_context);
                service.Initialize(_context);
                
                //
            }
        }

        //  Methods ---------------------------------------
        public void AddFeaturesForMenuScene(HudView hudView, MenuView menuView)
        {
            ComplexModel model = ModelLocator.GetItem<ComplexModel>();
            
            // Set Mode
            model.GameMode.Value = GameMode.Menu;
            
            AddFeaturesForHud(hudView);
            AddFeaturesForMenu(menuView);
        }
        
        public void AddFeaturesForGameScene(HudView hudView, GameView gameView)
        {
            ComplexModel model = ModelLocator.GetItem<ComplexModel>();
            
            // Set Mode
            model.GameMode.Value = GameMode.Game;
            
            AddFeaturesForHud(hudView);
            AddFeaturesForGame(gameView);
        }

        private void AddFeaturesForGame(GameView gameView)
        {
            ComplexModel model = ModelLocator.GetItem<ComplexModel>();
            ComplexService service = ServiceLocator.GetItem<ComplexService>();
            
            //////////////////////////////////////////////
            //Clear any old layout
            if (ControllerLocator.HasItem<GameController>())
            {
                ControllerLocator.RemoveItem<GameController>();
                ViewLocator.RemoveItem<GameView>();
            }
            
            //////////////////////////////////////////////
            //MENU: Now setup controller and view
            GameController gameController = new GameController(model, gameView, service);
            //
            ControllerLocator.AddItem(gameController);
            ViewLocator.AddItem(gameView);
            //
            gameView.Initialize(Context);
            gameController.Initialize(Context);
        }

        private void AddFeaturesForMenu(MenuView menuView)
        {
            ComplexModel model = ModelLocator.GetItem<ComplexModel>();
            ComplexService service = ServiceLocator.GetItem<ComplexService>();
            
            //////////////////////////////////////////////
            //Clear any old layout
            if (ControllerLocator.HasItem<MenuController>())
            {
                ControllerLocator.RemoveItem<MenuController>();
                ViewLocator.RemoveItem<MenuView>();
            }
            
            //////////////////////////////////////////////
            //MENU: Now setup controller and view
            MenuController menuController = new MenuController(model, menuView, service);
            //
            ControllerLocator.AddItem(menuController);
            ViewLocator.AddItem(menuView);
            //
            menuView.Initialize(Context);
            menuController.Initialize(Context);
        }

        private void AddFeaturesForHud(HudView hudView)
        {
            ComplexModel model = ModelLocator.GetItem<ComplexModel>();
            ComplexService service = ServiceLocator.GetItem<ComplexService>();
            

            
            //////////////////////////////////////////////
            //Clear any old layout
            if (ControllerLocator.HasItem<HudController>())
            {
                ControllerLocator.RemoveItem<HudController>();
                ViewLocator.RemoveItem<HudView>();
            }

            //////////////////////////////////////////////
            //HUD: Now setup controller and view
            HudController hudController = new HudController(model, hudView, service);
            //
            ControllerLocator.AddItem(hudController);
            ViewLocator.AddItem(hudView);
            //
            hudView.Initialize(Context);
            hudController.Initialize(Context);
            hudController.Load();
        }

        //  Event Handlers --------------------------------

    }
}