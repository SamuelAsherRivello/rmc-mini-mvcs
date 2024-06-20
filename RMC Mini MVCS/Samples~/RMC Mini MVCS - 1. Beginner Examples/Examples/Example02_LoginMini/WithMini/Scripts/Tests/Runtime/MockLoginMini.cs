using RMC.Mini.Samples.Login.WithMini.Mini.View;

namespace RMC.Mini.Samples.Login.WithMini.Mini
{
    /// <summary>
    /// This mock class creates a new object instance
    /// for use in testing.
    /// </summary>
    public class MockLoginMini
    {
        public static LoginMini CreateLoginMini(LoginView loginView)
        {
            //View
            LoginMini loginMini = new LoginMini(loginView);

            return loginMini;
        }
    }
}