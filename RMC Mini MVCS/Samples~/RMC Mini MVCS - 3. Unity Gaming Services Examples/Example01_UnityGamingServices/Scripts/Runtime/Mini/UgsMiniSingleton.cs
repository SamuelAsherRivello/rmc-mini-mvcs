using RMC.Core.Borrowed.DesignPatterns.Creational.Singletons;

namespace RMC.Mini.Samples.UGS.Mini
{
    /// <summary>
    /// Here is a <see cref="UgsMiniSingleton"/> that can be used to access the <see cref="UgsMini"/>.
    /// Replace this with your own custom implementation.
    /// </summary>
    public class UgsMiniSingleton: SingletonMonoBehaviour<UgsMiniSingleton>
    {
        //  Fields ----------------------------------------
        public UgsMini UgsMini { get; set; }
        
        //  Unity Methods ---------------------------------
        public override void OnInitialized()
        {
            base.OnInitialized();
            
            // Create the mini as you typically do  
            // Store the mini on this singleton for easy access
            UgsMini = new UgsMini();
            UgsMini.Initialize();
        }
    }
}