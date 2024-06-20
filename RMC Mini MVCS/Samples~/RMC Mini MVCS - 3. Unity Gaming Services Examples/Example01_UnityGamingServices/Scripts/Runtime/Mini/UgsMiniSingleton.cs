using RMC.Core.DesignPatterns.Creational.SingletonMonobehaviour;

namespace RMC.Mini.Samples.UGS.Mini
{
    /// <summary>
    /// Here is a <see cref="UgsMiniSingleton"/> that can be used to access the <see cref="UgsMini"/>.
    /// Replace this with your own custom implementation.
    /// </summary>
    public class UgsMiniSingleton: SingletonMonobehaviour<UgsMiniSingleton>
    {
        //  Fields ----------------------------------------
        public UgsMini UgsMini { get; set; }
        
        //  Unity Methods ---------------------------------
        public override void InstantiateCompleted()
        {
            base.InstantiateCompleted();
            
            // Create the mini as you typically do  
            // Store the mini on this singleton for easy access
            UgsMini = new UgsMini();
            UgsMini.Initialize();
        }
    }
}