#if EDITOR_VERSION_CHECK

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;


namespace RMC_Mini_MVCS.Editor.RMC.Core.Helper
{
    public static class EditorVersionCheck
    {
        private const string Menu = "Tutorials";
        private const string MenuPath = Menu + "/";
        private const string ShowTutorials = "Reset PackageInstalled Switch";
        
        private const string Packagename = "com.unity.inputsystem";
        private static PackageInfo packageInfo;
        private static bool _packagePresent = false;
        private static bool _packageInstalled = false;
        private static AddRequest AddRequest;
        private static ListRequest ListRequest;


        [MenuItem(MenuPath + ShowTutorials, priority = -99)]
        private static void ResetPackageInstalledSwitch()
        {
            EditorPrefs.SetBool("PackageInstalled", false);
            EditorUtility.RequestScriptReload();
            EditorApplication.Step();
        }
        
        [InitializeOnLoadMethod]
        private static void CheckVersionDependency()
        {
            _packageInstalled = EditorPrefs.GetBool("PackageInstalled");
            if (!_packageInstalled)
            {
                Debug.Log($"It seems like the package: '{Packagename}' is not installed." 
                          + $"\nBut it is needed on Unity Editor versions higher then: 2019.4.+");
                ListRequest = Client.List();
                EditorApplication.update += ListProgress;
                Debug.Log($"Searching for Package: '{Packagename}'...");
            }
        }
        
        private static void ListProgress()
        {
            if (!ListRequest.IsCompleted) return;
            
            switch (ListRequest.Status)
            {
                case StatusCode.Success:
                {
                    foreach (var result in ListRequest.Result)
                    {
                        _packagePresent = result.packageId.Contains(Packagename);
                        if (_packagePresent)
                            break;
                    }

                    Debug.Log(_packagePresent
                        ? $"Package: '{Packagename}' is already installed. No need to import it."
                        : $"Package: '{Packagename}' not installed. Importing it...");
                    break;
                }
                case >= StatusCode.Failure:
                    Debug.Log(ListRequest.Error.message);
                    _packagePresent = false;
                    break;
            }

            EditorApplication.update -= ListProgress;
            if (ListRequest.IsCompleted && !_packagePresent)
            {
                AddRequest = Client.Add(Packagename);
                EditorApplication.update += AddProgress;
            }
            else
            {
                EditorPrefs.SetBool("PackageInstalled", true);
            }
        }
        
        private static void AddProgress()
        {
            if (!AddRequest.IsCompleted) return;
            
            switch (AddRequest.Status)
            {
                case StatusCode.Success:
                    Debug.Log($"Successfully installed: '{AddRequest.Result.packageId}'.");
                    break;
                case >= StatusCode.Failure:
                    Debug.LogError(AddRequest.Error.message);
                    Debug.LogError(
                        $"Could not install package: '{AddRequest.Result.packageId}'. Try manually in the 'Package Manager.");
                    break;
            }

            EditorApplication.update -= AddProgress;
            EditorUtility.RequestScriptReload();
            EditorApplication.Step();
            EditorPrefs.SetBool("PackageInstalled", true);
        }
    }
}

#endif