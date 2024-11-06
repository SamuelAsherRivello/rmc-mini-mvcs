using System;
using System.IO;
using RMC.Core.Attributes;
using RMC.Core.Borrowed.DesignPatterns.Creational.Singletons;
using UnityEditor;
using UnityEngine;

#pragma warning disable CS0414
namespace RMC.Core.IO
{
    public sealed class LocalDiskStorage: Singleton<LocalDiskStorage>, ISingletonParent
    {
        //  Properties ------------------------------------
        
        
        //  Fields ----------------------------------------
        public const string Title = "LocalDiskStorage";
        
        
        //  Initialization Methods-------------------------
        public LocalDiskStorage()
        {
            // Avoid using constructor for initialization
        }

        
        void ISingletonParent.OnInstantiatedChild()
        {
            // Use this for initialization
        }

        
        //  General Methods -------------------------------
        public bool Save<T>(T obj)
        {
            return Save<T>(obj, true);
        }
        
        
        public bool Save<T>(T obj, bool willOverwrite)
        {
            CustomFilePathAttribute customFilePathAttribute = GetCustomFilePathAttributeSafe<T>();
            
            CreateDirectorySafe(customFilePathAttribute.Filepath);
            string json = JsonUtility.ToJson(obj, true);

            bool isSuccess = false;
            if (!willOverwrite && File.Exists(customFilePathAttribute.Filepath))
            {
                Debug.LogWarning($"LocalDiskStorage.Save() failed. File already exists and willOverwrite = {willOverwrite}.");
            }
            else
            {
                File.WriteAllText(customFilePathAttribute.Filepath, json);

                isSuccess = !string.IsNullOrEmpty(json);
                
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif //UNITY_EDITOR
                
            }

            return isSuccess;
        }

        public bool Delete<T>()
        {
            CustomFilePathAttribute customFilePathAttribute = GetCustomFilePathAttributeSafe<T>();

            bool isSuccess = false;

            string[] paths = new[]
            {
                customFilePathAttribute.Filepath,
                customFilePathAttribute.Filepath + ".meta"
            };

            for (int i = 0; i < paths.Length; i++)
            {
                if (File.Exists(paths[i]))
                {
                    File.Delete(paths[i]);
                    isSuccess = true;
                }
                else
                {
                    Debug.LogWarning($"LocalDiskStorage.Delete() failed. " +
                                     $"File must already exist. Path={paths[i]}");
                }
            }
            
            
            if (isSuccess)
            {
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif //UNITY_EDITOR
            }

            return isSuccess;
        }
        
        
        public bool Has<T>()
        {
            return LoadWithoutValidation<T>() != null;
        }
        
        
        public T Load<T>()
        {
            CustomFilePathAttribute customFilePathAttribute = GetCustomFilePathAttributeSafe<T>();

            if (string.IsNullOrEmpty(customFilePathAttribute.Filepath))
            {
                throw new Exception( $"LocalDiskStorage.Load() failed. [CustomFilePathAttribute (Filepath = {customFilePathAttribute.Filepath})] is invalid for {typeof(T).Name}.");
            }

            if (!File.Exists(customFilePathAttribute.Filepath))
            {
                throw new Exception( $"LocalDiskStorage.Load() failed. Call LocalDiskStorage.Has<T>() beforehand.");
            }
            
            return LoadWithoutValidation<T>();
        }
        
        
        private T LoadWithoutValidation<T>()
        {
            CustomFilePathAttribute customFilePathAttribute = GetCustomFilePathAttributeSafe<T>();
            if (!File.Exists(customFilePathAttribute.Filepath))
            {
                //null
                return default(T);
            }
            string json = File.ReadAllText(customFilePathAttribute.Filepath);
            return JsonUtility.FromJson<T>(json);
        }

        
        private CustomFilePathAttribute GetCustomFilePathAttributeSafe<T>()
        {
            CustomFilePathAttribute customFilePathAttribute = CustomFilePathAttribute.GetCustomFilePathAttribute<T>();
            if (customFilePathAttribute == null)
            {
                throw new Exception( $"LocalDiskStorage method failed. Add [CustomFilePathAttribute] to {typeof(T).Name}.");
            }

            return customFilePathAttribute;
        }


        private void CreateDirectorySafe(string filepath)
        {
            string directoryPath = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        
        //  Event Handlers --------------------------------
    }
}
