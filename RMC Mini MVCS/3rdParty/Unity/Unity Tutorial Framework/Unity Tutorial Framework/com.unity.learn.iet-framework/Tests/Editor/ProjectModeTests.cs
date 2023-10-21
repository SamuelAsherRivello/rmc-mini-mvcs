using NUnit.Framework;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class ProjectModeTest
    {
        [Test]
        public void IsAuthoringMode_Passes()
        {
            ProjectMode.IsAuthoringMode();
        }
    }
}
