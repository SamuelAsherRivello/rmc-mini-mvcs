using RMC.Core.DesignPatterns.Creational.Singleton.CustomSingletonMonobehaviour;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.SOM.Mini
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
    public class InventoryMiniSingleton: SingletonMonobehaviour<InventoryMiniSingleton>
    {
        //  Fields ----------------------------------------
        public InventoryMini InventoryMini { get; set; }
        
        //  Unity Methods ---------------------------------
        public override void InstantiateCompleted()
        {
            base.InstantiateCompleted();

            // Create the mini as you typically do  
            // Store the mini on this singleton for easy access
            InventoryMini = new InventoryMini();
            InventoryMini.Initialize();
        }
    }
}