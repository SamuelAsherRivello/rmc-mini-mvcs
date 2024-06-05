using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RMC.MiniMvcs.Samples.Configurator.Mini;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace RMC.MiniMvcs.Samples.Configurator.Standard
{
    [TestFixture]
    [Category ("RMC.Mini.Configurator")]
    public class AllScenesTest
    {
        private static readonly List<string> SceneNames = new List<string>
        {
            nameof(Scene01_Menu),
            nameof(Scene02_CustomizeCharacter),
            nameof(Scene03_CustomizeEnvironment),
            nameof(Scene04_Game),
        };
        
        [SetUp]
        public void SetUp()
        {
            
        }
        
        [TearDown]
        public void TearDown()
        {
        }
        
        [UnityTest]
        public IEnumerator SceneName_ThrowsNoErrors_WhenLoadScene([ValueSource(nameof(SceneNames))] string sceneName)
        {
            // Arrange
            
            // Act
            Assert.DoesNotThrow(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
  
            // Assert
            yield return new WaitForSeconds(1);
        }
        
    }
}
