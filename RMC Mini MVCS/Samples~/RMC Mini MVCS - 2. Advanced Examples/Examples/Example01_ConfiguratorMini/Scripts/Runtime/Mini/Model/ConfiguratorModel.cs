using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace RMC.MiniMvcs.Samples.Configurator.Mini.Model
{
    //  Namespace Properties ------------------------------
    public enum GameMode
    {
        Menu,
        Game,
        CustomizeCharacter,
        CustomizeEnvironment
    }
    
    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class ConfiguratorModel: BaseModel // Extending 'base' is optional
    {
        //  Properties ------------------------------------
        public Observable<bool> HasBackNavigation { get { return _hasBackNavigation;} }
        public Observable<bool> HasLoadedService { get { return _hasLoadedService;} }
        public Observable<CharacterData> CharacterData { get { return _characterData;} }
        public Observable<EnvironmentData> EnvironmentData { get { return _environmentData;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<bool> _hasBackNavigation = new Observable<bool>();
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
                _characterData.Value = RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data.CharacterData.FromDefaultValues();
                _environmentData.Value = RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data.EnvironmentData.FromDefaultValues();
            }
        }

        
        //  Methods ---------------------------------------
        public bool CharacterDataIsDefault()
        {
            var x = CharacterData.Value;
            var y = RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data.CharacterData
                .FromDefaultValues();
            
            return CharacterData.Value.Equals(RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data.CharacterData
                .FromDefaultValues());
        }
        
        public bool EnvironmentDataIsDefault()
        {
            return EnvironmentData.Value.Equals(RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data.EnvironmentData
                .FromDefaultValues());
        }

        //  Event Handlers --------------------------------
 
    }
}