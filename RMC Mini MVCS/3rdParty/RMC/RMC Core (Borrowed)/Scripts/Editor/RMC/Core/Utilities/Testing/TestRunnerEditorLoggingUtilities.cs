#if UNITY_EDITOR
using UnityEngine;
#endif

namespace RMC.Core.Utilities.Testing
{
    public static class TestRunnerEditorLoggingUtilities
    {
#if UNITY_EDITOR
        private static bool _isSuppressingLogs = false;
        private static LogType _originalLogType = LogType.Log;

        public static void SuppressLogMessages(bool suppress)
        {
            if (suppress && !_isSuppressingLogs)
            {
                _isSuppressingLogs = true;
                _originalLogType = Debug.unityLogger.filterLogType;
                Debug.unityLogger.filterLogType = LogType.Error;
            }
            else if (!suppress && _isSuppressingLogs)
            {
                _isSuppressingLogs = false;
                Debug.unityLogger.filterLogType = _originalLogType;
            }
        }
#endif
    }
}