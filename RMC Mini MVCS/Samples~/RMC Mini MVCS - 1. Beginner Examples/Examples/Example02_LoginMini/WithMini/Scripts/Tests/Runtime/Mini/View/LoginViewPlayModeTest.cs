using System.Collections;
using NUnit.Framework;
using RMC.Core.Utilities.Testing;
using UnityEngine.TestTools;

namespace RMC.Mini.Samples.Login.WithMini.Mini.View
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Login")]
    public class LoginViewPlayModeTest
    {
        
        private static PrefabManagerForTesting _prefabManagerForTesting;
        
        [SetUp]
        public void Setup()
        {
            _prefabManagerForTesting = new PrefabManagerForTesting();
        }

        [TearDown]
        public void TearDown()
        {
            _prefabManagerForTesting.Clear();
            
        }

        
        [UnityTest]
        public IEnumerator Initialize_DoesNotThrow_WhenCalled()
        {
            // Arrange
            IContext context = new Context();
            LoginView loginView = 
                _prefabManagerForTesting.LoadAndInstantiate<LoginView>("Prefabs/LoginView");
            
            // Assert
            Assert.DoesNotThrow(() =>
            {
                // Act
                loginView.Initialize(context);
            });
            
            yield return null;
            
        }
    }
}