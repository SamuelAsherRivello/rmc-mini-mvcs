using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Samples.MultiScene.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class MultiSceneModel: BaseModel // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<string> SceneName { get { return _sceneName;} }
        public Observable<string> Message { get { return _message;} }
        public Observable<bool> ServiceHasLoaded { get { return _serviceHasLoaded;} }
        public Observable<int> ClickCount { get { return _clickCount;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<string> _sceneName = new Observable<string>();
        private readonly Observable<string> _message = new Observable<string>();
        private readonly Observable<bool> _serviceHasLoaded = new Observable<bool>();
        private readonly Observable<int> _clickCount = new Observable<int>();

        public const string MultiSceneMiniExample01 = "MultiSceneMiniExample01";
        public const string MultiSceneMiniExample02 = "MultiSceneMiniExample02";
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                //
                _sceneName.OnValueChanged.AddListener(SceneName_OnValueChanged);
                
                // Set Defaults
                _sceneName.Value = "";
                _message.Value = "";
                _serviceHasLoaded.Value = false;
                _clickCount.Value = 0;
            }
        }

        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------
        private void SceneName_OnValueChanged(string previousValue, string currentValue)
        {
            Context.CommandManager.InvokeCommand(
                new SceneNameChangedCommand(previousValue, currentValue));
        }
    }
}