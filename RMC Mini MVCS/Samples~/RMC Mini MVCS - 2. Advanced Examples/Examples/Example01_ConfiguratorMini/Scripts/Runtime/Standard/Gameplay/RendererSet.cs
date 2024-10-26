using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace RMC.Mini.Samples.Configurator.Standard.Gameplay
{
    /// <summary>
    /// Represents a group of <see cref="Renderer"/>s
    /// </summary>
    public class RendererSet : MonoBehaviour
    {
        //  Properties ------------------------------------

        //  Fields ----------------------------------------
        [SerializeField] 
        public List<Renderer> Renderers;

        //  Unity Methods ---------------------------------
        
        //  Methods ---------------------------------
    }
}
