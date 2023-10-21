using UnityEditor.Connect;

namespace Unity.Tutorials.Core.Editor
{
    internal class UnityConnectSession
    {
        internal const string k_NotSignedInUserUsername = "anonymous";
        static UnityConnectSession _instance = new UnityConnectSession();

        public static UnityConnectSession instance { get { return _instance; } }

        public string GetAccessToken() { return UnityConnect.instance.GetAccessToken(); }

        public string GetUserId() { return UnityConnect.instance.GetUserId(); }

        public string GetEnvironment() { return UnityConnect.instance.GetEnvironment(); }

        public void ShowLogin() { UnityConnect.instance.ShowLogin(); }

        /// <summary>
        /// NOTE no-op if user is not logged in
        /// </summary>
        /// <param name="url"></param>
        public static void OpenAuthorizedURLInWebBrowser(string url) { UnityConnect.instance.OpenAuthorizedURLInWebBrowser(url); }

        public static bool loggedIn { get { return UnityConnect.instance.loggedIn; } }
    }
}
