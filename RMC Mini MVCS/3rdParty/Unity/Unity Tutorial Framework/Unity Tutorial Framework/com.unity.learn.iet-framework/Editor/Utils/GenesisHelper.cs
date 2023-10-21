using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Unity.Tutorials.Core.Editor
{
    [InitializeOnLoad]
    internal static class GenesisHelper
    {
        static readonly string prodHost = @"https://api.unity.com";
        static readonly string stagingHost = @"https://api-staging.unity.com";

        static List<KeyValuePair<UnityWebRequestAsyncOperation, Action<UnityWebRequest>>> m_Requests = new List<KeyValuePair<UnityWebRequestAsyncOperation, Action<UnityWebRequest>>>();

        static readonly HttpClient s_HttpClient = new HttpClient();

        public static bool HasWarnedAboutLogin { get; set; }

        static string HostAddress
        {
            get { return (IsStagingEnv() ? stagingHost : prodHost); }
        }

        static bool IsStagingEnv()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            for (int i = 0; i < commandLineArgs.Length; i++)
            {
                if (commandLineArgs[i] == "-cloudEnvironment")
                {
                    if (i + 1 < commandLineArgs.Length && commandLineArgs[i + 1] == "staging")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static string GetVersion()
        {
            return UnityEditor.PackageManager.PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly()).version;
        }

        static GenesisHelper()
        {
            s_HttpClient.BaseAddress = new Uri(HostAddress);
            EditorApplication.update += WebRequestProcessor;
        }

        public static void LogTutorialStarted(string lessonId)
        {
            if (!IsLessonIdValid(lessonId))
            {
                return;
            }
            GetTutorial(lessonId, (list) =>
            {
                var lesson = list.FirstOrDefault(s => s.lessonId == lessonId);
                if (lesson == null || string.IsNullOrEmpty(lesson.status.Trim()))
                {
                    // The fact no entries exist means the user never opened (started) this tutorial
                    LogTutorialStatusUpdate(lessonId, TutorialProgressStatus.Status.Started.ToString());
                }
            });
        }

        public static void LogTutorialEnded(string lessonId)
        {
            if (!IsLessonIdValid(lessonId))
            {
                return;
            }
            // Always set the status to Finished: if we just began a tutorial and completed it very fast
            // we might not have received the up-to-date (Started) state from the backend if querying it here.
            LogTutorialStatusUpdate(lessonId, TutorialProgressStatus.Status.Finished.ToString());
        }

        static bool IsLessonIdValid(string lessonId)
        {
            if (string.IsNullOrEmpty(lessonId.Trim()))
            {
                LogWarningOnlyInAuthoringMode("LessonId is not set. You can set LessonId on the tutorial asset");
                return false;
            }
            return true;
        }

        static void LogWarningOnlyInAuthoringMode(string message)
        {
            // We don't want to spam users with warning messages
            // but we want to catch them while creating tutorials
            if (ProjectMode.IsAuthoringMode())
                Debug.LogWarning(message);
        }

        public static void PrintAllTutorials()
        {
            GetAllTutorials((tutorials) =>
            {
                var result = "";
                foreach (var tutorial in tutorials)
                {
                    result += tutorial.lessonId + ": " + tutorial.status + "\n";
                }
                Debug.Log(result);
            });
        }

        public static void GetAllTutorials(Action<List<TutorialProgressStatus>> action)
        {
            GetTutorial(null, action);
        }

        static bool IsRequestSuccess(UnityWebRequest unityWebRequest)
        {
#if UNITY_2020_1_OR_NEWER
            if ((unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                || (unityWebRequest.result == UnityWebRequest.Result.ProtocolError))
#else
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
#endif
            {
                LogWarningOnlyInAuthoringMode(unityWebRequest.error);
                return false;
            }
            return true;
        }

        static void GetTutorial(string lessonId, Action<List<TutorialProgressStatus>> action)
        {
            var userId = UnityConnectSession.instance.GetUserId();
            if (userId.IsNullOrEmpty() || userId == UnityConnectSession.k_NotSignedInUserUsername)
            {
                if (!HasWarnedAboutLogin)
                {
                    Debug.LogWarning("Error: No user ID. Are you logged in?");
                    HasWarnedAboutLogin = true;
                }
                return;
            }
            string getLink = @"/v1/users/" + userId + @"/lessons";
            string url = HostAddress + getLink;
            UnityWebRequest req = MakeGetLessonsRequest(url, lessonId);
            SendWebRequest(req, (UnityWebRequest r) =>
            {
                if (!IsRequestSuccess(r))
                {
                    return;
                }
                var lessonResponses = TutorialProgressStatus.ParseResponses(r.downloadHandler.text);
                action(lessonResponses);
            });
        }

        public static async void LogTutorialStatusUpdate(string lessonId, string lessonStatus)
        {
            var userId = UnityConnectSession.instance.GetUserId();
            if (userId.IsNullOrEmpty()) return;
            var getLink = @"/v1/users/" + userId + @"/lessons";

            var jsonData = RegisterLessonRequest.GetJSONString(lessonStatus, userId, lessonId);

            // UnityWebRequests were causing memory leaks here, so they were replaced with HttpClient
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, getLink))
            {
                var data = new StringContent(jsonData, Encoding.UTF8, "application/json");

                request.Content = data;

                request.Headers.Add("X-IET-Version", GetVersion());
                request.Headers.Add("Authorization", "Bearer " + UnityConnectSession.instance.GetAccessToken());
                HttpResponseMessage response = await s_HttpClient.SendAsync(request);
            }
        }

        static void SendWebRequest(UnityWebRequest request, Action<UnityWebRequest> onFinished)
        {
            var pair = new KeyValuePair<UnityWebRequestAsyncOperation, Action<UnityWebRequest>>(request.SendWebRequest(), onFinished);
            m_Requests.Add(pair);
        }

        static void WebRequestProcessor()
        {
            if (!m_Requests.Any())
                return;

            for (int i = 0; i < m_Requests.Count; i++)
            {
                var request = m_Requests[i].Key;
                if (!request.isDone)
                    continue;
                var callback = m_Requests[i].Value;
                m_Requests.RemoveAt(i);
                callback(request.webRequest);
                break;
            }
        }

        static UnityWebRequest MakeGetLessonsRequest(string url, string lessonId)
        {
            if (!string.IsNullOrEmpty(lessonId))
            {
                url += "?lessonId=" + lessonId;
            }

            var request = UnityWebRequest.Post(url, new WWWForm());
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-IET-Version", GetVersion());
            request.SetRequestHeader("Authorization", "Bearer " + UnityConnectSession.instance.GetAccessToken());
            request.method = "GET";
            return request;
        }

        [Serializable]
        public class TutorialProgressStatus
        {
            public string lessonId;
            public string status; // TODO make an enum instead

            public enum Status
            {
                Started,
                Finished
            }

            [Serializable]
            class Wrapper
            {
                public List<TutorialProgressStatus> statuses = null;
            }

            public static List<TutorialProgressStatus> ParseResponses(string respText)
            {
                var builder = new StringBuilder(12 + respText.Length + 1);
                builder.Append("{\"statuses\":");
                builder.Append(respText);
                builder.Append("}");
                var wrapper = JsonUtility.FromJson<Wrapper>(builder.ToString());
                return wrapper.statuses;
            }
        }

        class RegisterLessonRequest
        {
            public string status;
            public string userId;
            public string lessonId;

            public static string GetJSONString(string status, string userId, string lessonId)
            {
                var r = new RegisterLessonRequest();
                r.status = status;
                r.userId = userId;
                r.lessonId = lessonId;
                return JsonUtility.ToJson(r);
            }
        }
    }
}
