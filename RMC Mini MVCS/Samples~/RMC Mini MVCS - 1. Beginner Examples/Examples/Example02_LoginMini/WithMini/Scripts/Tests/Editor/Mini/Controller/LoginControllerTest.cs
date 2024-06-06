using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;
using RMC.Core.Experimental;
using RMC.Core.Testing;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Login")]
    public class LoginControllerTest
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
            if (ContextLocator.Instance.HasItem<Context>())
            {
                ContextLocator.Instance.RemoveItem<Context>();
            }
        }
        
        
        [Test]
        public void Controller_InvokesTimeChangedCommand_WhenModelTimeChanges()
        {
            // Arrange
            LoginView loginView = _prefabManagerForTesting.LoadAndInstantiate<LoginView>("Prefabs/LoginView");
            LoginSimpleMini loginSimpleMini = MockLoginMini.CreateLoginMini(loginView);
            loginSimpleMini.Initialize();
            UserData loggedInUserData = null;
            loginSimpleMini.Context.CommandManager.AddCommandListener<LoggedInUserDataChangedCommand>(
                (loggedInUserDataChangedCommand) =>
                {
                    loggedInUserData = loggedInUserDataChangedCommand.CurrentValue;
                });
            
            // Act
            loginSimpleMini.Model.LoggedInUserData.Value = new UserData("testusername", "testpassword");
            
            // Assert
            Assert.That(loggedInUserData, Is.SameAs(loginSimpleMini.Model.LoggedInUserData.Value));
        }
    }
}