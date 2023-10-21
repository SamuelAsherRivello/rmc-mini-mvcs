using System;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class BuildPlayerTests : TestBase
    {
        const string k_TempScenePath = "Assets/UntitledScene.unity";

        static string BuildPath
        {
            get
            {
                // NOTE Use "Builds" subfolder in order to make this test pass locally when
                // using Windows & Visual Studio
                const string buildName = "Builds/BuildPlayerTests_Build";
                if (Application.platform == RuntimePlatform.OSXEditor)
                    return buildName + ".app";
                return buildName;
            }
        }

        [SetUp]
        public void SetUp()
        {
            // 2021.2.0 and newer will fail if we don't save the current scene
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), k_TempScenePath);
            Assert.That(File.Exists(BuildPath), Is.False, "Existing file at path " + BuildPath);
            Assert.That(Directory.Exists(BuildPath), Is.False, "Existing directory at path " + BuildPath);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(BuildPath))
            {
                File.Delete(BuildPath);
            }

            if (Directory.Exists(BuildPath))
            {
                Directory.Delete(BuildPath, true);
            }

            AssetDatabase.DeleteAsset(k_TempScenePath);
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

        [Test]
        public void BuildPlayer_Succeeds()
        {
            var buildTarget = BuildTarget;
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, buildTarget))
            {
                // Need to have this for Katana as it doesn't have Player building capabilities
                Assert.Pass();
            }

            var report = BuildPipeline.BuildPlayer(
                new BuildPlayerOptions
                {
                    scenes = new[] { GetTestAssetPath("EmptyTestScene.unity") }, // NOTE could probably pass 'null' here as well
                    locationPathName = BuildPath,
                    targetGroup = BuildTargetGroup.Standalone,
                    target = buildTarget,
                    options = BuildOptions.StrictMode,
                }
            );

            Assert.AreEqual(BuildResult.Succeeded, report.summary.result);
        }
    }
}
