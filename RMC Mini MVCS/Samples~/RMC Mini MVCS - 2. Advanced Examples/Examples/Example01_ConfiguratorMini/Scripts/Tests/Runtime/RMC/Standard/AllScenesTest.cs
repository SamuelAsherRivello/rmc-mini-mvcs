using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RMC.Mini.Samples.Configurator.Mini.Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace RMC.Mini.Samples.Configurator.Standard
{
    [TestFixture]
    [Category ("RMC.Mini.Configurator")]
    public class AllScenesTest
    {
        private static readonly List<string> SceneNames = new List<string>
        {
            ConfiguratorConstants.Scene01_Menu,
            ConfiguratorConstants.Scene02_CustomizeCharacter,
            ConfiguratorConstants.Scene03_CustomizeEnvironment ,
            ConfiguratorConstants.Scene04_Game,
            ConfiguratorConstants.Scene05_DeveloperConsole
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
