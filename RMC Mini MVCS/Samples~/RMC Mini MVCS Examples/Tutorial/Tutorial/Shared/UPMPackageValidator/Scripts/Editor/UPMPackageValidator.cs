using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace RMC.UPMPackageValidator
{
    public static class UPMPackageValidator
    {
        private static AddRequest addRequest;
        private static ListRequest listRequest;
        private static Stack<int> missingPackages = new Stack<int>();

        public class PackageEntry
        {
            public string name;
            public string version;
        }

        private static List<PackageEntry> packageToAdd;
        private const string LOG_PREFIX = "[UPM Package Validator] : ";
        private const string UPMPackageValidatorSymbol = "UPM_PACKAGE_VALIDATOR_CHECK_IS_COMPLETE";
        private const string PackageImportListFilename = "PackageImportListFilename.txt";

        private static string UPMValidatorDiskStorage
        {
            get
            {
                return Application.dataPath + "/../Library/UPMValidatorDiskStorage";
            }
        }

        private static void SetScriptingDefineSymbols(string symbolName, bool isAdd)
        {
            if (isAdd)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                    symbolName);
            }
            else
            {
                string existingSymbols =
                    PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

                // Replace the symbol you want to clear with an empty string
                existingSymbols = existingSymbols.Replace(symbolName + ";", "");

                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                    existingSymbols);
            }
        }


        public static void VerifyDependencies(bool willForceOperation)
        {
            if (willForceOperation)
            {
                File.Delete(UPMValidatorDiskStorage);
                SetScriptingDefineSymbols(UPMPackageValidatorSymbol, false);
            }

            if (!File.Exists(UPMValidatorDiskStorage) || !HasScriptingDefineSymbols(UPMPackageValidatorSymbol))
            {
                Debug.Log($"{LOG_PREFIX} Checking if required packages are included");

                var packageListFile =
                    Directory.GetFiles(Application.dataPath, PackageImportListFilename,
                        SearchOption.AllDirectories);

                if (packageListFile.Length == 0)
                {
                    Debug.LogError(
                        $"{LOG_PREFIX} Couldn't find the packages list. Be sure there is a file called {PackageImportListFilename} in your project");
                }
                else
                {
                    string packageListPath = packageListFile[0];
                    packageToAdd = new List<PackageEntry>();
                    string[] content = File.ReadAllLines(packageListPath);
                    foreach (var line in content)
                    {
                        //Ignore comments and require the @ symbol
                        if (!line.StartsWith("//") && line.Contains("@"))
                        {
                            var split = line.Split('@');
                            PackageEntry entry = new PackageEntry();

                            entry.name = split[0];
                            entry.version = split.Length > 1 ? split[1] : null;
                            packageToAdd.Add(entry);
                        }
                    }

                    listRequest = Client.List();
                    EditorApplication.update += Update;
                }
            }
        }

        
        private static bool HasScriptingDefineSymbols(string symbol)
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup)
                .Contains(symbol);
        }


        static void Update()
        {
            if (listRequest != null)
            {
                if (listRequest.IsCompleted)
                {
                    bool[] foundPackages = new bool[packageToAdd.Count];

                    for (int i = 0; i < foundPackages.Length; ++i)
                        foundPackages[i] = false;

                    foreach (var package in listRequest.Result)
                    {
                        for (int i = 0; i < foundPackages.Length; ++i)
                        {
                            if (package.packageId.Contains(packageToAdd[i].name))
                            {
                                foundPackages[i] = true;
                                Debug.Log($"{LOG_PREFIX} Package '" + packageToAdd[i].name +
                                          "' already imported in that project");
                            }
                        }
                    }

                    for (int i = 0; i < foundPackages.Length; ++i)
                    {
                        if (!foundPackages[i])
                            missingPackages.Push(i);
                    }

                    listRequest = null;
                }
                else if (listRequest.Error != null)
                {
                    Debug.Log(listRequest.Error.message);
                    listRequest = null;
                }
            }
            else
            {
                bool noMorePackage = false;

                if (missingPackages.Count > 0)
                    EditorUtility.DisplayProgressBar("Importing package", "Importing missing package for the project",
                        1.0f - missingPackages.Count / (float)packageToAdd.Count);
                else
                    EditorUtility.ClearProgressBar();

                if (addRequest == null)
                {
                    if (missingPackages.Count == 0)
                    {
                        noMorePackage = true;
                    }
                    else
                    {
                        int package = missingPackages.Pop();
                        string name = packageToAdd[package].name;
                        if (packageToAdd[package].version != null)
                            name += "@" + packageToAdd[package].version;

                        addRequest = Client.Add(name);
                    }
                }
                else
                {
                    if (addRequest.IsCompleted)
                    {
                        if (addRequest.Error != null)
                        {
                            Debug.LogError($"{LOG_PREFIX}" + addRequest.Error.message);
                        }
                        else if (addRequest.Result != null)
                        {
                            Debug.Log($"{LOG_PREFIX} Automatically added package " + addRequest.Result.displayName);
                        }
                        else
                        {
                            Debug.LogError(
                                $"{LOG_PREFIX} Unknown error with adding new package to the Package Manager");
                        }

                        addRequest = null;
                    }
                }

                if (noMorePackage)
                {
                    Debug.Log($"{LOG_PREFIX} All packages checked");
                    SetScriptingDefineSymbols(UPMPackageValidatorSymbol, true);
                    File.WriteAllText(UPMValidatorDiskStorage, "Delete this to trigger a new auto package check");
                    EditorApplication.update -= Update;
                }
            }
        }
    }
}

