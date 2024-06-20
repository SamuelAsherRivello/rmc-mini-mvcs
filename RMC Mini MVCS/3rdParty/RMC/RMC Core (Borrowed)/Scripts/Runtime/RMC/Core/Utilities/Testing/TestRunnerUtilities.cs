namespace RMC.Core.Utilities.Testing
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
            return TestRunnerEditorUtilities.IsRunningUnitTests();
#else
            return false;
#endif
        }
    }
}
