using UnityEngine;

namespace Unity.Tutorials.Core
{
    internal static class GameObjectProxy
    {
        public static Bounds CalculateBounds(GameObject gameObject)
        {
            return gameObject.CalculateBounds();
        }
    }
}
