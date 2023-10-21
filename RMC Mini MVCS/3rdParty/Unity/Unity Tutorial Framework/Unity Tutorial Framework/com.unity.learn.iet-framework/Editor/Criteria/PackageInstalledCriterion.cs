using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Criterion for checking that a specific package is installed in the project
    /// </summary>
    public class PackageInstalledCriterion : Criterion
    {
        /// <summary>
        /// Since this criterion does async operations, we need to know if the result is actually correct before we check it in tests
        /// </summary>
        internal bool ResultIsAccurate { get; private set; }
        internal string PackageName
        {
            get => m_PackageName;
            set => m_PackageName = value;
        }

        [SerializeField, Tooltip("The name of the package that will be searched (I.E: 'com.unity.learn.iet-framework', without quotes)")]
        string m_PackageName;
        [SerializeField, Tooltip("Specifies whether or not the Package Manager requests the latest information about the project's packages from the remote Unity package registry. When offlineMode is true, the PackageInfo objects in the PackageCollection returned by the Package Manager contain information obtained from the local package cache, which could be out of date.")]
        bool m_OfflineMode;
        [SerializeField, Tooltip("Indirect dependencies include packages referenced in the manifests of project packages or in the manifests of other indirect dependencies. Set this flag to false to include only the packages listed directly in the project manifest.")]
        bool m_IncludeIndirectDependencies;

        bool m_PackageIsInstalled;
        ListRequest m_PackagesListRequest;
        AddRequest m_PackagesAddRequest;

        /// <inheritdoc />
        public override void StartTesting()
        {
            base.StartTesting();
            InitiatePackageListRequest();
            EditorApplication.update += UpdateCompletion;
        }

        /// <inheritdoc />
        public override void StopTesting()
        {
            base.StopTesting();
            EditorApplication.update -= UpdateCompletion;
        }

        /// <inheritdoc />
        protected override bool EvaluateCompletion()
        {
            return m_PackageIsInstalled;
        }

        /// <inheritdoc />
        public override bool AutoComplete()
        {
            //note: as this is an async operation but we have to return a result, we have to assume that it'll work before we send the request
            m_PackageIsInstalled = true;
            InitiatePackageAddRequest();
            return m_PackageIsInstalled;
        }

        void CheckPackageListRequest()
        {
            if (!m_PackagesListRequest.IsCompleted)
            {
                return;
            }

            EditorApplication.update -= CheckPackageListRequest;
            if (m_PackagesListRequest.Status != StatusCode.Success)
            {
                InitiatePackageListRequest(); //maybe we could add an in creasing retry window here?
                return;
            }

            PackageCollection installedPackages = m_PackagesListRequest.Result;
            foreach (var package in installedPackages)
            {
                if (package.name == m_PackageName)
                {
                    m_PackageIsInstalled = true;
                    ResultIsAccurate = true;
                    return;
                }
            }
            ResultIsAccurate = true;
        }

        void InitiatePackageListRequest()
        {
            ResultIsAccurate = false;
            m_PackageIsInstalled = false;
            m_PackagesListRequest = Client.List(m_OfflineMode, m_IncludeIndirectDependencies);
            EditorApplication.update += CheckPackageListRequest;
        }

        void InitiatePackageAddRequest()
        {
            ResultIsAccurate = false;
            m_PackagesAddRequest = Client.Add(m_PackageName);
            EditorApplication.update += CheckPackageAddRequest;
        }

        void CheckPackageAddRequest()
        {
            if (!m_PackagesAddRequest.IsCompleted)
            {
                return;
            }

            EditorApplication.update -= CheckPackageAddRequest;
            if (m_PackagesAddRequest.Status != StatusCode.Success)
            {
                InitiatePackageAddRequest();
                return;
            }

            m_PackageIsInstalled = true;
            ResultIsAccurate = true;
        }
    }
}
