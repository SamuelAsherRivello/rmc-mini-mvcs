using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.View;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini
{
    /// <summary>
    /// This mock class creates a new object instance
    /// for use in testing.
    /// </summary>
    public class MockLoginMini
    {
        public static LoginSimpleMini CreateLoginMini(LoginView loginView)
        {
            //View
            LoginSimpleMini loginSimpleMini = new LoginSimpleMini(loginView);

            return loginSimpleMini;
        }
    }
}