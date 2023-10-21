namespace Unity.Tutorials.Core.Editor
{
    internal class TutorialFrameworkController : Controller
    {
        TutorialFrameworkModel m_Model;

        internal TutorialFrameworkController(TutorialFrameworkModel model)
        {
            m_Model = model;
        }

        internal override void RemoveListeners()
        {
        }
        internal void OnSignInClicked()
        {
            UnityConnectSession.instance.ShowLogin();
        }
    }
}
