using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;
using RMC.Core.Testing;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Login")]
    public class LoginMiniTest
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
        
        [Test]
        public void LoginMini_DoesNotThrow_WhenInitialize()
        {
            // Arrange
            LoginView loginView = _prefabManagerForTesting.LoadAndInstantiate<LoginView>("Prefabs/LoginView");
            
            // Act
            Assert.DoesNotThrow(() =>
            {
                // Assert
                LoginMini loginMini = MockLoginMini.CreateLoginMini(loginView);
            });
        }
    }
}