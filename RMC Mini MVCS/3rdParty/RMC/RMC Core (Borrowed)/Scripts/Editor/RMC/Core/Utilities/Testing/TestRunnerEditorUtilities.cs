using System.Linq;
using UnityEditor;

namespace RMC.Core.Utilities.Testing
{
    public static class TestRunnerEditorUtilities
    {
        // Set to false for normal operation
        private static bool IsThisClassDisabled = false;

        // Determines if unit tests are running
        public static bool IsRunningUnitTests()
        {
#if UNITY_EDITOR
            TestRunnerEditorModeUtilities.Initialize();
            var nunitAssembly = System.AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.FullName.StartsWith("nunit.framework"));
            return nunitAssembly != null && TestRunnerEditorModeUtilities.IsRunningTests;
#else
            return false;
#endif
        }

#if UNITY_EDITOR
        // Called when Unity editor loads
        [InitializeOnLoadMethod]
        public static void InitializeOnLoadMethod()
        {
            if (!IsThisClassDisabled)
            {
                TestRunnerEditorModeUtilities.Initialize();
            }
        }
#endif
    }
}