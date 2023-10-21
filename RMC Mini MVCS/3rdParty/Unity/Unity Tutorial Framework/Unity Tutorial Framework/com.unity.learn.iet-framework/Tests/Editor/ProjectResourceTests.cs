using NUnit.Framework;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class ProjectResourceTests
    {
        readonly string[] k_UITexturePaths =
        {
            UIElementsUtils.s_UIResourcesPath,
        };
        TutorialWindow Window => TutorialWindow.Instance;

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (Window != null)
            {
                yield return new WaitForDelayCall();
                if (Window.Model.Tutorial.CurrentTutorial)
                {
                    TutorialWindow.ExitTutorial();
                }
                Window.Close();
            }
        }

        [Test]
        public void CommonResourcesExist()
        {
            Assert.IsTrue(Directory.Exists(UIElementsUtils.s_UIResourcesPath), $"'{UIElementsUtils.s_UIResourcesPath}' does not exist");

            const string commonCallbacks = "Packages/com.unity.learn.iet-framework/Editor/DefaultAssets/CommonTutorialCallbacksHandler.asset";
            Assert.IsTrue(File.Exists(commonCallbacks), $"'{commonCallbacks}' does not exist");

            Assert.IsTrue(File.Exists(TutorialContainer.k_DefaultLayoutPath), $"'{TutorialContainer.k_DefaultLayoutPath}' does not exist");

            Assert.IsTrue(File.Exists(TutorialProjectSettings.k_DefaultStyleAsset), $"'{TutorialProjectSettings.k_DefaultStyleAsset}' does not exist");

            Assert.IsTrue(File.Exists(TutorialStyles.DefaultDarkStyleFile), $"'{TutorialStyles.DefaultDarkStyleFile}' does not exist");
            Assert.IsTrue(File.Exists(TutorialStyles.DefaultLightStyleFile), $"'{TutorialStyles.DefaultLightStyleFile}' does not exist");
        }

        [Ignore("Works locally, fails on Yamato")]
        [Test]
        public void DefaultLayoutContainsTutorialWindow()
        {
            TutorialModel.OnLayoutLoaded -= OnLayoutLoaded;
            TutorialModel.OnLayoutLoaded += OnLayoutLoaded;

            TutorialWindow.GetOrCreateWindow(null);
            TutorialWindow.Instance.Model.Tutorial.SaveOriginalWindowLayout();
            TutorialModel.LoadWindowLayout(TutorialContainer.k_DefaultLayoutPath);
            bool hasTutorialWindow = EditorWindowUtils.FindOpenInstance<TutorialWindow>();
            TutorialModel.ReopenWindowLayoutAsBeforeTutorialStarted(null);
            Assert.IsTrue(hasTutorialWindow, $"{TutorialContainer.k_DefaultLayoutPath} does not contain TutorialWindow.");
        }

        void OnLayoutLoaded(bool obj)
        {
            TutorialFrameworkModel.s_AreTestsRunning = true;
        }

        [Test]
        public void UITexturesPathsExist()
        {
            k_UITexturePaths.ToList().ForEach(path =>
                Assert.IsTrue(Directory.Exists(path), $"Path '{path}' does not exist")
            );
        }

        [Test]
        public void UITexturesHaveCorrectTextureType()
        {
            var texturesWithWrongType = AssetDatabase.FindAssets("t:Texture2D", k_UITexturePaths)
                .Select(guid => AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as TextureImporter)
                .Where(importer => importer.textureType != TextureImporterType.GUI)
                .Select(importer => $"\"{importer.assetPath}\"")
                .ToArray();

            Assert.IsEmpty(texturesWithWrongType);
        }
    }
}
