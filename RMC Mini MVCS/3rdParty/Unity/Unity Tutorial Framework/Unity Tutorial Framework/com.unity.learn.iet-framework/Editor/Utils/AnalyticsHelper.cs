using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

namespace Unity.Tutorials.Core.Editor
{
    enum TutorialConclusion
    {
        Completed,
        Quit,
        Reloaded
    }

    class TutorialAnalyticsEventData
    {
        public string tutorialName;
        public string version;
        public TutorialConclusion conclusion;
        public string lessonID;

        public TutorialAnalyticsEventData(string tutorialName, string version, TutorialConclusion conclusion, string lessonID)
        {
            this.tutorialName = tutorialName;
            this.version = version;
            this.conclusion = conclusion;
            this.lessonID = lessonID;
        }
    }

    enum TutorialPageConclusion
    {
        Completed,
        Reviewed
    }

    class TutorialPageAnalyticsEventData
    {
        public string tutorialName;
        public int pageIndex;
        public string guid;
        public TutorialPageConclusion conclusion;

        public TutorialPageAnalyticsEventData(string tutorialName, int pageIndex, string guid, TutorialPageConclusion conclusion)
        {
            this.tutorialName = tutorialName;
            this.pageIndex = pageIndex;
            this.guid = guid;
            this.conclusion = conclusion;
        }
    }

    enum TutorialParagraphConclusion
    {
        Completed,
        Regressed
    }

    class TutorialParagraphAnalyticsEventData
    {
        public string tutorialName;
        public int pageIndex;
        public int paragraphIndex;
        public TutorialParagraphConclusion conclusion;

        public TutorialParagraphAnalyticsEventData(string tutorialName, int pageIndex, int paragraphIndex, TutorialParagraphConclusion conclusion)
        {
            this.tutorialName = tutorialName;
            this.pageIndex = pageIndex;
            this.paragraphIndex = paragraphIndex;
            this.conclusion = conclusion;
        }
    }

    class AnalyticsHelper : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        Tutorial currentTutorial;

        [SerializeField]
        TutorialPage currentPage;

        [SerializeField]
        int currentPageIndex;

        [SerializeField]
        TutorialPage lastPage;

        [SerializeField]
        int lastPageIndex;

        [SerializeField]
        int currentParagraphIndex;

        DateTime currentTutorialStartTime;

        DateTime currentPageStartTime;

        DateTime lastPageStartTime;

        DateTime currentParagraphStartTime;

        [SerializeField]
        long currentTutorialStartTicks;

        [SerializeField]
        long currentPageStartTicks;

        [SerializeField]
        long lastPageStartTicks;

        [SerializeField]
        long currentParagraphStartTicks;

        public static AnalyticsHelper Instance
        {
            get
            {
                if (!s_Instance)
                {
                    var instance = Resources.FindObjectsOfTypeAll<AnalyticsHelper>();
                    if (instance.Length == 0)
                    {
                        s_Instance = CreateInstance<AnalyticsHelper>();
                        s_Instance.hideFlags = HideFlags.HideAndDontSave;
                    }
                    else
                    {
                        s_Instance = instance[0] as AnalyticsHelper;
                    }
                }
                return s_Instance;
            }
        }
        static AnalyticsHelper s_Instance;

        public void OnBeforeSerialize()
        {
            currentTutorialStartTicks = currentTutorialStartTime.Ticks;
            currentPageStartTicks = currentPageStartTime.Ticks;
            lastPageStartTicks = lastPageStartTime.Ticks;
            currentParagraphStartTicks = currentParagraphStartTime.Ticks;
        }

        public void OnAfterDeserialize()
        {
            currentTutorialStartTime = new DateTime(currentTutorialStartTicks, DateTimeKind.Utc);
            currentPageStartTime = new DateTime(currentPageStartTicks, DateTimeKind.Utc);
            lastPageStartTime = new DateTime(lastPageStartTicks, DateTimeKind.Utc);
            currentParagraphStartTime = new DateTime(currentParagraphStartTicks, DateTimeKind.Utc);
        }

        static void DebugWarning(string message, params object[] args)
        {
#if DEBUG_TUTORIALS
            Debug.LogWarningFormat(message, args);
#endif
        }

        static void DebugLog(string message, params object[] args)
        {
#if DEBUG_TUTORIALS
            Debug.LogFormat(message, args);
#endif
        }

        static void DebugError(string message, params object[] args)
        {
#if DEBUG_TUTORIALS
            Debug.LogErrorFormat(message, args);
#endif
        }

        internal static void TutorialStarted(Tutorial tutorial)
        {
            if (Instance.currentTutorial != null)
            {
                DebugWarning("TutorialStarted Ignored because tutorial is already set: {0}", tutorial);
                return;
            }
            DebugLog("Tutorial Started: {0}", tutorial);
            Instance.currentTutorial = tutorial;
            Instance.currentTutorialStartTime = DateTime.UtcNow;
            Instance.currentPageIndex = Instance.lastPageIndex = Instance.currentParagraphIndex = -1;
            Instance.currentPage = Instance.lastPage = null;
        }

        internal static void TutorialEnded(TutorialConclusion conclusion)
        {
            if (Instance.currentTutorial == null)
            {
                DebugWarning("TutorialEnded Ignored because no tutorial is set");
                return;
            }
            if (conclusion == TutorialConclusion.Completed)
            {
                PageShown(Instance.lastPage, Instance.lastPageIndex + 1);  // "Show" a dummy page to get the last page to report
            }
            if (Instance.currentTutorial.ProgressTrackingEnabled)
            {
                SendTutorialEvent(
                    Instance.currentTutorial.name, Instance.currentTutorial.Version, conclusion,
                    Instance.currentTutorial.LessonId, Instance.currentTutorialStartTime,
                    DateTime.UtcNow - Instance.currentTutorialStartTime, false
                );
            }
            DebugLog("Tutorial Ended: " + conclusion);
            if (conclusion == TutorialConclusion.Quit)
                Instance.currentTutorial = null;
        }

        internal static void PageShown(TutorialPage page, int pageIndex)
        {
            if (Instance.currentTutorial == null)
            {
                DebugWarning("PageShown Ignored because no tutorial is set");
                return;
            }

            Instance.currentPageIndex = pageIndex;
            Instance.currentPage = page;
            Instance.currentPageStartTime = DateTime.UtcNow;

            if (Instance.lastPage != null)
            {
                if (Instance.currentPageIndex < Instance.lastPageIndex)
                {
                    SendTutorialPageEvent
                    (
                        Instance.currentTutorial.name, Instance.currentPageIndex, GetAssetGuid(Instance.currentPage),
                        TutorialPageConclusion.Reviewed, Instance.currentPageStartTime,
                        DateTime.UtcNow - Instance.currentPageStartTime, false
                    );

                    DebugLog("Page Reviewed: {0}", Instance.currentPageIndex);
                }
                else if (pageIndex > Instance.lastPageIndex)
                {
                    SendTutorialPageEvent
                    (
                        Instance.currentTutorial.name, Instance.lastPageIndex, GetAssetGuid(Instance.lastPage),
                        TutorialPageConclusion.Completed, Instance.lastPageStartTime,
                        DateTime.UtcNow - Instance.lastPageStartTime, false
                    );

                    DebugLog("Page Completed: {0}", Instance.lastPageIndex);
                }
            }

            if (Instance.currentPageIndex <= Instance.lastPageIndex)
                return;

            Instance.lastPageIndex = Instance.currentPageIndex;
            Instance.lastPage = Instance.currentPage;
            Instance.lastPageStartTime = DateTime.UtcNow;
            Instance.currentParagraphIndex = -1;
        }

        internal static void ParagraphStarted(int paragraphIndex)
        {
            if (Instance.currentTutorial == null)
            {
                DebugWarning("ParagraphStarted Ignored because no tutorial is set: {0}", paragraphIndex);
                return;
            }
            if (Instance.currentParagraphIndex >= 0)
            {
                ParagraphEnded(true);
            }
            DebugLog("Paragraph Started: {0}", paragraphIndex);
            Instance.currentParagraphStartTime = DateTime.UtcNow;
            Instance.currentParagraphIndex = paragraphIndex;
        }

        internal static void ParagraphEnded()
        {
            ParagraphEnded(false);
        }

        internal static void ParagraphEnded(bool regressed)
        {
            if (Instance.currentTutorial == null)
            {
                DebugWarning("ParagraphEnded Ignored because no tutorial is set");
                return;
            }
            if (Instance.currentParagraphIndex == -1)
            {
                DebugWarning("ParagraphEnded Ignored because no paragraph is active");
                return;
            }
            DebugLog("Paragraph Ended: regression = {0}", regressed);
            var conclusion = regressed ? TutorialParagraphConclusion.Regressed : TutorialParagraphConclusion.Completed;

            SendTutorialParagraphEvent
            (
                Instance.currentTutorial.name, Instance.currentPageIndex, Instance.currentParagraphIndex, conclusion,
                Instance.currentParagraphStartTime, DateTime.UtcNow - Instance.currentParagraphStartTime, false
            );

            Instance.currentParagraphIndex = -1;
        }

        static string GetAssetGuid(ScriptableObject asset) => AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));

        /// <summary>
        /// Use for external references/links, documentation, assets, etc.
        /// https://docs.google.com/spreadsheets/d/1vftlkO4yps3qUoPgM2wnbJu4YwRO3fZ4cS7IELk4Gww/edit#gid=1343103808
        /// </summary>
        public struct ExternalReferenceEventData
        {
            public int ts; // timestamp
            public string id;
            public string title;
            public string type; // e.g. Asset Store or Mods
            public string path; // URL
        }
        public struct ExternalReferenceImpressionEventData
        {
            public int ts; // timestamp
            public string id;
            public string title;
            public string type; // e.g. Asset Store or Mods
            public string path; // URL
        }

        public struct TutorialEventData
        {
            public int ts; // timestamp
            public string tutorialName;
            public string version;
            public int conclusion;
            public string lessonID;
            public int startTime;
            public int duration;
            public bool isBlocking;
        }

        public struct TutorialPageEventData
        {
            public int ts; // timestamp
            public string tutorialName;
            public int pageIndex;
            public string guid;
            public int conclusion;
            public int startTime;
            public int duration;
            public bool isBlocking;
        }

        public struct TutorialParagraphEventData
        {
            public int ts; // timestamp
            public string tutorialName;
            public int pageIndex;
            public int paragraphIndex;
            public int conclusion;
            public int startTime;
            public int duration;
            public bool isBlocking;
        }

        const int k_MaxEventsPerHour = 1000;
        const int k_MaxNumberOfElements = 1000;
        const string k_VendorKey = "unity.iet"; // the format needs to be "unity.xxx"

        const string k_EventExternalReference = "iet_externalReference";
        const string k_EventExternalReferenceImpression = "iet_externalReferenceImpression";
        const string k_EventTutorial = "iet_tutorial";
        const string k_EventTutorialPage = "iet_tutorialPage";
        const string k_EventTutorialParagraph = "iet_tutorialParagraph";

        static bool RegisterEvent(string name)
        {
            AnalyticsResult result = EditorAnalytics.RegisterEventWithLimit(name, k_MaxEventsPerHour, k_MaxNumberOfElements, k_VendorKey);
            if (result != AnalyticsResult.Ok)
            {
                DebugError("Error in RegisterEvent: {0}", result);
                return false;
            }
            return true;
        }

        public static AnalyticsResult SendTutorialEvent(string tutorialName, string version, TutorialConclusion conclusion, string lessonID, DateTime startTime, TimeSpan duration, bool isBlocking)
        {
            if (!EditorAnalytics.enabled || !RegisterEvent(k_EventTutorial)) { return AnalyticsResult.AnalyticsDisabled; }
            var data = new TutorialEventData
            {
                ts = DateTime.UtcNow.Millisecond,
                tutorialName = tutorialName,
                version = version,
                conclusion = (int)conclusion,
                lessonID = lessonID,
                duration = duration.Milliseconds,
                startTime = startTime.Millisecond,
                isBlocking = isBlocking
            };

            return SendEvent(k_EventTutorial, data);
        }

        public static AnalyticsResult SendTutorialPageEvent(string tutorialName, int pageIndex, string guid, TutorialPageConclusion conclusion, DateTime startTime, TimeSpan duration, bool isBlocking)
        {
            if (!EditorAnalytics.enabled || !RegisterEvent(k_EventTutorialPage)) { return AnalyticsResult.AnalyticsDisabled; }
            var data = new TutorialPageEventData
            {
                ts = DateTime.UtcNow.Millisecond,
                tutorialName = tutorialName,
                pageIndex = pageIndex,
                guid = guid,
                conclusion = (int)conclusion,
                duration = duration.Milliseconds,
                startTime = startTime.Millisecond,
                isBlocking = isBlocking
            };
            return SendEvent(k_EventTutorialPage, data);
        }

        public static AnalyticsResult SendTutorialParagraphEvent(string tutorialName, int pageIndex, int paragraphIndex, TutorialParagraphConclusion conclusion, DateTime startTime, TimeSpan duration, bool isBlocking)
        {
            if (!EditorAnalytics.enabled || !RegisterEvent(k_EventTutorialParagraph)) { return AnalyticsResult.AnalyticsDisabled; }
            var data = new TutorialParagraphEventData
            {
                ts = DateTime.UtcNow.Millisecond,
                tutorialName = tutorialName,
                pageIndex = pageIndex,
                paragraphIndex = paragraphIndex,
                conclusion = (int)conclusion,
                duration = duration.Milliseconds,
                startTime = startTime.Millisecond,
                isBlocking = isBlocking
            };
            return SendEvent(k_EventTutorialParagraph, data);
        }

        public static AnalyticsResult SendExternalReferenceEvent(string url, string title, string contentType, string id = null)
        {
            if (!EditorAnalytics.enabled || !RegisterEvent(k_EventExternalReference)) { return AnalyticsResult.AnalyticsDisabled; }

            var data = new ExternalReferenceEventData
            {
                ts = DateTime.UtcNow.Millisecond,
                id = id,
                title = title,
                type = contentType,
                path = url
            };
            return SendEvent(k_EventExternalReference, data);
        }

        public static AnalyticsResult SendExternalReferenceImpressionEvent(string url, string title, string contentType, string id = null)
        {
            if (!EditorAnalytics.enabled || !RegisterEvent(k_EventExternalReferenceImpression)) { return AnalyticsResult.AnalyticsDisabled; }

            var data = new ExternalReferenceImpressionEventData
            {
                ts = DateTime.UtcNow.Millisecond,
                id = id,
                title = title,
                type = contentType,
                path = url
            };
            return SendEvent(k_EventExternalReferenceImpression, data);
        }

        static AnalyticsResult SendEvent(string eventName, object parameters)
        {
            AnalyticsResult result = EditorAnalytics.SendEventWithLimit(eventName, parameters);
            if (result != AnalyticsResult.Ok)
            {
                DebugError("Error in {0}: {1}", eventName, result);
            }
            return result;
        }
    }
}
