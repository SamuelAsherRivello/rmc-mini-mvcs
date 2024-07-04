using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace RMC.Core.Testing
{
    /// <summary>
    /// Subclass this to add tests
    /// </summary>
    public abstract class GroupOfScenesBaseTest
    {
        private static bool _ignoreFailingMessagesBefore;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Do not put here: LogAssert.ignoreFailingMessages = true;
        }

        [SetUp]
        public void SetUp()
        {
            // Do before every test so that I can turn it off at the end of the delay inside each test in subclass
            _ignoreFailingMessagesBefore = LogAssert.ignoreFailingMessages;
            
            // Change to new value
            LogAssert.ignoreFailingMessages = true;
        }
        
        [TearDown]
        public void TearDown()
        {
            // Do not put here: LogAssert.ignoreFailingMessages = false;
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Do not put here: LogAssert.ignoreFailingMessages = false;
        }
        
        [UnityTest]
        public virtual IEnumerator SceneName_ThrowsNoErrors_WhenLoadScene(string sceneName)
        {
            // Arrange

            // Act & Assert
            
            // 1. Load the scene to be tested
            var loadAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (loadAsyncOperation != null && !loadAsyncOperation.isDone)
            {
                yield return null;
            }

            // 2. Create and load a temporary scene to switch context
            // Unity requires I load 'something', before I can do #3.
            var temporaryScene = SceneManager.CreateScene("TemporarySceneName");

            // 3. Unload the test scene
            var unloadAsyncOperation = SceneManager.UnloadSceneAsync(sceneName);
            while (unloadAsyncOperation != null && !unloadAsyncOperation.isDone)
            {
                yield return null;
            }

            // 4. Unload the temporary scene by setting it active and then unloading it
            SceneManager.SetActiveScene(temporaryScene);
            var tempUnloadAsyncOperation = SceneManager.UnloadSceneAsync(temporaryScene);
            while (tempUnloadAsyncOperation != null && !tempUnloadAsyncOperation.isDone)
            {
                yield return null;
            }

            Assert.DoesNotThrow(() => { });

            // Change back to old value
            LogAssert.ignoreFailingMessages = _ignoreFailingMessagesBefore;
            yield return null;
        }
    }
}
