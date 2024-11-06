using RMC.Core.Borrowed.DesignPatterns.Creational.Singletons;

namespace RMC.Mini.Lessons.Scalability.StandardWithFeature.Mini
{
    /// <summary>
    /// Here is a <see cref="InventoryMiniSingleton"/> that can be
    /// used to access the <see cref="InventoryMini"/>.
    /// 
    /// Use of a Singleton is optional.
    ///
    /// Replace this with your own custom implementation.
    /// 
    /// </summary>
    public class InventoryMiniSingleton: SingletonMonoBehaviour<InventoryMiniSingleton>
    {
        //  Fields ----------------------------------------
        public InventoryMini InventoryMini { get; set; }
        
        //  Unity Methods ---------------------------------
        public override void OnInitialized()
        {
            base.OnInitialized();

            // Create the mini as you typically do  
            // Store the mini on this singleton for easy access
            InventoryMini = new InventoryMini();
            InventoryMini.Initialize();
        }
    }
}