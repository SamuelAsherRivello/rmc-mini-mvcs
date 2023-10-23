using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace RMC.UPMPackageValidator
{
    public static class UPMPackageValidatorMenuItems
    {
        private const string Menu = "Tutorials";
        private const string MenuPath = Menu + "/";
        private const string ShowTutorials = "Install Any Dependencies";

        [MenuItem(MenuPath + ShowTutorials, priority = -100)]
        public static void ReverifyDependencies()
        {
            //Verify always
            UPMPackageValidator.VerifyDependencies(true);
        }

        [InitializeOnLoadMethod]
        static void VerifyDependencies()
        {
            //Verify, BUT ONLY once per user, per unity install
            UPMPackageValidator.VerifyDependencies(false);
        }
    }
}

