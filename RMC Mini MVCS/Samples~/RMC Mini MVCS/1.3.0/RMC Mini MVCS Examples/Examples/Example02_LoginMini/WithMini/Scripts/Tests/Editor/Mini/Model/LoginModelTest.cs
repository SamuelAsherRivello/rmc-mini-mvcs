
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Experimental;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.Login")]
    public class LoginModelTest
    {
        [TearDown]
        public void TearDown()
        {
            if (ContextLocator.Instance.HasItem<Context.Context>())
            {
                ContextLocator.Instance.RemoveItem<Context.Context>();
            }
        }
        
        [Test]
        public void RollABallModel_DefaultValues_WhenCreated()
        {
            // Arrange
            IContext context = new Context.Context();
            LoginModel loginModel = new LoginModel();
            
            // Act
            loginModel.Initialize(context);
            
            // Assert
            Assert.That(loginModel.IsLoggedIn, Is.False);
            Assert.That(loginModel.LoggedInUserData.Value, Is.Null);
        }
    }
}