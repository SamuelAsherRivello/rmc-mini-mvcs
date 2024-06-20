using System;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace RMC.Core.Utilities.Testing
{
    public static class TestRunnerEditorModeUtilities
    {
#if UNITY_EDITOR
        public static bool IsRunningTests = false;
        private static bool _isInitialized = false;
        private static bool _hasShownWarning = false;

        public static void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            _isInitialized = true;

            var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            testRunnerApi.RegisterCallbacks(new TestRunnerCallback(
                () =>
                {
                    IsRunningTests = true;
                    ShowWarning();
                    TestRunnerEditorLoggingUtilities.SuppressLogMessages(true);
                },
                () =>
                {
                    IsRunningTests = false;
                    TestRunnerEditorLoggingUtilities.SuppressLogMessages(false);
                }
            ));
        }

        private static void ShowWarning()
        {
            if (!_hasShownWarning)
            {
                _hasShownWarning = true;
                UnityEngine.Debug.LogWarning("Warning: TestRunnerEditorUtilities is suppressing some log messages during test run to reduce Console Window clutter.\n\nTo disable this behavior, please modify TestRunnerEditorUtilities.cs.\n\n");
            }
        }

        private class TestRunnerCallback : ICallbacks
        {
            private readonly Action _onStarting;
            private readonly Action _onEnding;

            public TestRunnerCallback(Action onStarting, Action onEnding)
            {
                _onStarting = onStarting;
                _onEnding = onEnding;
            }

            public void RunStarted(ITestAdaptor testsToRun)
            {
                _onStarting?.Invoke();
            }

            public void RunFinished(ITestResultAdaptor result)
            {
                _onEnding?.Invoke();
            }

            public void TestStarted(ITestAdaptor test)
            {
                _onStarting?.Invoke();
            }

            public void TestFinished(ITestResultAdaptor result)
            {
                _onEnding?.Invoke();
            }
        }
#endif
    }
}
