using RMC.Core.DesignPatterns.Creational.Singleton.CustomSingletonMonobehaviour;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini
{
    /// <summary>
    /// Here is a <see cref="ConfiguratorMiniSingleton"/> that can be used to access the <see cref="ConfiguratorMini"/>.
    /// Replace this with your own custom implementation.
    /// </summary>
    public class ConfiguratorMiniSingleton: SingletonMonobehaviour<ConfiguratorMiniSingleton>
    {
        //  Fields ----------------------------------------
        public ConfiguratorMini ConfiguratorMini { get; set; }
        
        //  Unity Methods ---------------------------------
        public override void InstantiateCompleted()
        {
            base.InstantiateCompleted();
            
            // Create the mini as you typically do  
            // Store the mini on this singleton for easy access
            ConfiguratorMini = new ConfiguratorMini();
            ConfiguratorMini.Initialize();
        }
    }
}