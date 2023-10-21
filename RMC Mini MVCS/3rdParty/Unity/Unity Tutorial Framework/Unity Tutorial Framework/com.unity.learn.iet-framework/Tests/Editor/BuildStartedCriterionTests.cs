using System;
using System.IO;
using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    class BuildStartedCriterionTests : CriterionTestBase<BuildStartedCriterion>
    {
        // TODO Make some kind of reusable utils out of BuildPath, BuildTarget, and so on, as they are copy-pasted in multiple places now.
        static string BuildPath
        {
            get
            {
                const string buildName = "Test/Test";
                if (Application.platform == RuntimePlatform.OSXEditor)
                    return buildName + ".app";
                return buildName;
            }
        }

        [SetUp]
        public void SetUp()
        {
            SaveActiveScene();
            Assert.That(File.Exists(BuildPath), Is.False, "Existing file at path " + BuildPath);
            Assert.That(Directory.Exists(BuildPath), Is.False, "Existing directory at path " + BuildPath);
        }

        [TearDown]
        public override void TearDown()
        {
            if (File.Exists(BuildPath))
                File.Delete(BuildPath);

            if (Directory.Exists(BuildPath))
                Directory.Delete(BuildPath, true);

            DeleteTempScene();

            base.TearDown();
        }

        static BuildTarget BuildTarget
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                        return BuildTarget.StandaloneOSX;
                    case RuntimePlatform.WindowsEditor:
                        return BuildTarget.StandaloneWindows;
                    case RuntimePlatform.LinuxEditor:
                        // NOTE Universal & 32-bit Linux support dropped after 2018 LTS
                        return BuildTarget.StandaloneLinux64;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [UnityTest]
        public IEnumerator CustomHandlerIsInvoked_IsCompleted()
        {
            var buildTarget = BuildTarget;
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, buildTarget))
            {
                // Need to have this for Katana as it doesn't have Player building capabilities
                Assert.Pass();
            }

            m_Criterion.BuildPlayerCustomHandler(new BuildPlayerOptions
            {
                scenes = null,
                target = buildTarget,
                locationPathName = BuildPath,
                targetGroup = BuildTargetGroup.Unknown
            });
            yield return null;

            Assert.IsTrue(m_Criterion.IsCompleted);
        }

        [UnityTest]
        public IEnumerator AutoComplete_IsCompleted()
        {
            yield return null;
            Assert.IsTrue(m_Criterion.AutoComplete());
        }
    }
}
