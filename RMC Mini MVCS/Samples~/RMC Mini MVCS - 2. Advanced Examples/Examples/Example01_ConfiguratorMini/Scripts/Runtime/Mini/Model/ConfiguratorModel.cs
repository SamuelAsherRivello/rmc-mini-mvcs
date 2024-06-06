using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model.Data;
using RMC.Core.Data.Types;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class ConfiguratorModel: BaseModel // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        public Observable<bool> HasNavigationBack { get { return _hasNavigationBack;} }
        public Observable<bool> HasNavigationDeveloperConsole { get { return _hasNavigationDeveloperConsole;} }
        public Observable<bool> HasLoadedService { get { return _hasLoadedService;} }
        public Observable<CharacterData> CharacterData { get { return _characterData;} }
        public Observable<EnvironmentData> EnvironmentData { get { return _environmentData;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<bool> _hasNavigationBack = new Observable<bool>();
        private readonly Observable<bool> _hasNavigationDeveloperConsole = new Observable<bool>();
        private readonly Observable<bool> _hasLoadedService = new Observable<bool>();
        private readonly Observable<CharacterData> _characterData = new Observable<CharacterData>();
        private readonly Observable<EnvironmentData> _environmentData = new Observable<EnvironmentData>();
        
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _hasLoadedService.Value = false;
                _hasNavigationBack.Value = false;
                _hasNavigationDeveloperConsole.Value = false;
                
                _characterData.Value = Data.CharacterData.FromDefaultValues();
                _environmentData.Value = Data.EnvironmentData.FromDefaultValues();
            }
        }

        
        //  Methods ---------------------------------------
        public bool CharacterDataIsDefault()
        {
            var x = CharacterData.Value;
            var y = Data.CharacterData
                .FromDefaultValues();
            
            return CharacterData.Value.Equals(Data.CharacterData
                .FromDefaultValues());
        }
        
        public bool EnvironmentDataIsDefault()
        {
            return EnvironmentData.Value.Equals(Data.EnvironmentData
                .FromDefaultValues());
        }

        //  Event Handlers --------------------------------
 
    }
}