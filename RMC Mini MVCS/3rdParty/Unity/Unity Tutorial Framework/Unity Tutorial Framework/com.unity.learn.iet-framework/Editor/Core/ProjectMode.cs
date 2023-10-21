using System;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Can be used to query the mode of the Tutorial Project.
    /// </summary>
    public static class ProjectMode
    {
        /// <summary>
        /// Returns true if Tutorial Authoring Tools are present and we are in authoring mode.
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthoringMode()
        {
            return Type.GetType(
                "Unity.Tutorials.Authoring.Editor.TutorialExporterWindow, " +
                "Unity.Tutorials.Authoring.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                ) != null;
        }
    }
}
