using UnityEditor;

namespace Unity.Tutorials.Core.Editor
{
    internal class PrefabUtilityShim
    {
        public static UnityEngine.Object GetCorrespondingObjectFromSource(UnityEngine.Object source)
        {
#if UNITY_2018_2_OR_NEWER
            return PrefabUtility.GetCorrespondingObjectFromSource(source);
#else
            return PrefabUtility.GetPrefabParent(source);
#endif
        }
    }
}
