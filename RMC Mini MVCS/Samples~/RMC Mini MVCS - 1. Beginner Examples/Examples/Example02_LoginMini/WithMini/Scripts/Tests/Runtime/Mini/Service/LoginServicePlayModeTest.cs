
using System.Collections;
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Experimental;
using UnityEngine;
using UnityEngine.TestTools;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Service
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Login")]
    public class LoginServicePlayModeTest
    {
        [TearDown]
        public void TearDown()
        {
            if (ContextLocator.Instance.HasItem<Context>())
            {
                ContextLocator.Instance.RemoveItem<Context>();
            }
        }
        
        [UnityTest]
        public IEnumerator UserDataOutput_IsSameAsInput_WhenLoginCompleted()
        {
            // Arrange
            IContext context = new Context();
            LoginService loginService = new LoginService();
            loginService.Initialize(context);
            
            UserData inputUserData = new UserData("myUsername", "wrongPassword");
            UserData resultUserData = null;
            bool isLoginCompleted = false;
            loginService.OnLoginCompleted.AddListener((userData, wasSuccess) =>
            {
                isLoginCompleted = true;
                resultUserData = userData;
            });
            
            // Act
            loginService.Login(inputUserData);
            yield return new WaitUntil(() => isLoginCompleted);

            // Assert
            Assert.That(resultUserData, Is.SameAs(inputUserData));
        }
        
        [UnityTest]
        public IEnumerator WasSuccess_IsFalse_WhenIncorrectPassword()
        {
            // Arrange
            IContext context = new Context();
            LoginService loginService = new LoginService();
            loginService.Initialize(context);
            
            UserData inputUserData = new UserData("myUsername", "wrongPassword");
            bool resultWasSuccess = false;
            bool isLoginCompleted = false;
            loginService.OnLoginCompleted.AddListener((userData, wasSuccess) =>
            {
                isLoginCompleted = true;
                resultWasSuccess = wasSuccess;
            });
            
            // Act
            loginService.Login(inputUserData);
            yield return new WaitUntil(() => isLoginCompleted);

            // Assert
            Assert.That(resultWasSuccess, Is.False);
        }
        
        [UnityTest]
        public IEnumerator WasSuccess_IsTrue_WhenCorrectPassword()
        {
            // Arrange
            IContext context = new Context();
            LoginService loginService = new LoginService();
            loginService.Initialize(context);
            
            UserData inputUserData = new UserData("myUsername", "pass1234");
            bool resultWasSuccess = false;
            bool isLoginCompleted = false;
            loginService.OnLoginCompleted.AddListener((userData, wasSuccess) =>
            {
                isLoginCompleted = true;
                resultWasSuccess = wasSuccess;
            });
            
            // Act
            loginService.Login(inputUserData);
            yield return new WaitUntil(() => isLoginCompleted);

            // Assert
            Assert.That(resultWasSuccess, Is.True);
        }

    }
}