using System.Collections;
using NUnit.Framework;
using RMC.Core.Testing;
using UnityEngine.TestTools;

namespace RMC.Mini.Samples.UGS.Standard
{
    [TestFixture]
    [Category("RMC.Mini.Ugs")]
    public class UgsGroupOfScenesTest : GroupOfScenesBaseTest
    {
        
        /// <summary>
        /// Update here with the scenes you want to test
        /// </summary>
        private static readonly string[] _sceneNames = new string[]
        {
            UgsConstants.Scene01_Ugs_Menu,
            UgsConstants.Scene02_Ugs_PlayerAccounts,
            UgsConstants.Scene03_Ugs_CloudSave ,
            UgsConstants.Scene04_Ugs_UserGeneratedContent,
            UgsConstants.Scene05_Ugs_DeveloperConsole
        };

        /// <summary>
        /// Do not add/edit tests below this class.
        ///
        /// Do add/edit tests in the superclass
        /// </summary>
        [UnityTest]
        public override IEnumerator SceneName_ThrowsNoErrors_WhenLoadScene(
            [ValueSource(nameof(_sceneNames))] string sceneName)
        {
            return base.SceneName_ThrowsNoErrors_WhenLoadScene(sceneName);
        }
    }
}
