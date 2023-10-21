using System.IO;
using System.Linq;
using System.Reflection;

using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class TestBase
    {
        // Returns the correct path (forward slash used as the separator) depending whether we're running the tests locally or as a separate package.
        protected static string GetTestAssetPath(string relativeAssetPath)
        {
            var packagePath = PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly()).assetPath;
            return Path.Combine($"{packagePath}/Tests/Editor", relativeAssetPath).Replace('\\', '/');
        }
    }
}
