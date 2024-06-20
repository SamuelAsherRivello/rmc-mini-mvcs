using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace RMC.Mini.Samples.UGS.Standard
{
    [TestFixture]
    [Category ("RMC.Mini.Configurator")]
    public class AllScenesTest
    {
        private static readonly List<string> SceneNames = new List<string>
        {
            UgsConstants.Scene01_Ugs_Menu,
            UgsConstants.Scene02_Ugs_PlayerAccounts,
            UgsConstants.Scene03_Ugs_CloudSave ,
            UgsConstants.Scene04_Ugs_UserGeneratedContent,
            UgsConstants.Scene05_Ugs_DeveloperConsole
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
