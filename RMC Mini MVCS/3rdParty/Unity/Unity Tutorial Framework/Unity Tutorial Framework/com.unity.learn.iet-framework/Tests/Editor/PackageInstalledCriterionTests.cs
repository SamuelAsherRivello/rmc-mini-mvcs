using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class PackageInstalledCriterionTests : CriterionTestBase<PackageInstalledCriterion>
    {
        const string k_UnfindablePackage = "com.unity.ads.ios-support"; //note: not in the project by default, so should never be there while our CI runs this
        const string k_FindablePackage = "com.unity.learn.iet-framework";

        [UnityTest]
        public IEnumerator PackageInstalled_IsCompleted()
        {
            var packageExists = new ResultHolder();
            yield return IsPackageInstalled(k_FindablePackage, packageExists);
            Assume.That(packageExists.AsBool);

            m_Criterion.StopTesting();
            Assume.That(!m_Criterion.IsCompleted);
            m_Criterion.PackageName = k_FindablePackage;
            m_Criterion.StartTesting();

            yield return new WaitUntil(() => m_Criterion.ResultIsAccurate);
            Assert.IsTrue(m_Criterion.IsCompleted);
        }

        [UnityTest]
        public IEnumerator PackageNotInstalled_NotCompleted()
        {
            var packageExists = new ResultHolder();
            yield return IsPackageInstalled(k_UnfindablePackage, packageExists);
            Assume.That(!packageExists.AsBool);

            m_Criterion.StopTesting();
            Assume.That(!m_Criterion.IsCompleted);
            m_Criterion.PackageName = k_UnfindablePackage;
            m_Criterion.StartTesting();
            yield return new WaitUntil(() => m_Criterion.ResultIsAccurate);

            Assert.IsFalse(m_Criterion.IsCompleted);
        }

        [System.Serializable]
        class ResultHolder
        {
            public object Result;
            public bool AsBool => (bool)Result;
        }


        static IEnumerator IsPackageInstalled(string packageName, ResultHolder result)
        {
            yield return null;

            var listRequest = Client.List(false, false);
            while (!listRequest.IsCompleted)
            {
                yield return null;
            }

            result.Result = listRequest.Result.Any(x => x.name == packageName);

            yield return null;
        }
    }
}
