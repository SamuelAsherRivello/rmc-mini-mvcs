using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Standard
{
    public static class CustomGameObjectUtility
    {
        //  Methods ---------------------------------------
        public static void DontDestroyOnLoadSafe(GameObject gameObject)
        {
            gameObject.transform.parent = null;
            Object.DontDestroyOnLoad(gameObject);
        }
    }
}
