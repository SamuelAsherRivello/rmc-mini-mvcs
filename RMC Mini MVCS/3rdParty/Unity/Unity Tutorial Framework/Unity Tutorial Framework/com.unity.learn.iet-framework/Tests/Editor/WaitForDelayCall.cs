using System.Collections;
using UnityEditor;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class WaitForDelayCall : IEditModeTestYieldInstruction
    {
        bool m_WaitingForDelayCall = true;

        public WaitForDelayCall()
        {
            EditorApplication.delayCall += () => m_WaitingForDelayCall = false;
        }

        public IEnumerator Perform()
        {
            while (m_WaitingForDelayCall)
                yield return null;
        }

        public bool ExpectDomainReload { get; }
        public bool ExpectedPlaymodeState { get; }
    }
}
