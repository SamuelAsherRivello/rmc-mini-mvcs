using System;
using System.Linq;
using UnityEngine;

namespace RMC.Core.Utilities
{
    public static class TestRunnerUtilities
    {
        //  Methods ---------------------------------------

        /// <summary>
        /// Determines if unit tests are running
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningUnitTests()
        {
#if UNITY_EDITOR
            var nunitAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.FullName.StartsWith("nunit.framework"));
            return nunitAssembly != null;
#else
            return false;
#endif

        }
    }
}
