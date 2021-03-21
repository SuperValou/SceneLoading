using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor
{
    public static class AssetDatabaseExt
    {
        public static ICollection<string> GetAllScenePaths(bool relativeToAssetFolder = false)
        {
            var sceneGuids = AssetDatabase.FindAssets("t:Scene");
            var relativeScenePaths = sceneGuids.Select(AssetDatabase.GUIDToAssetPath);
            if (relativeToAssetFolder)
            {
                return relativeScenePaths.ToList();
            }

            var absoluteScenePaths = relativeScenePaths.Select(GetAssetFullPath).ToList();
            return absoluteScenePaths;
        }

        public static string GetAssetFilePath(string filename, bool relativeToAssetFolder = false)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException($"{nameof(filename)} cannot be null or empty.", nameof(filename));
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            string fileExtension = Path.GetExtension(filename);

            string relativePath = GetAssetRelativePath(fileNameWithoutExtension, fileExtension);
            if (relativeToAssetFolder)
            {
                return relativePath;
            }

            string fullPath = GetAssetFullPath(relativePath);
            return fullPath;
        }
        
        private static string GetAssetRelativePath(string assetName, string assetExtension)
        {
            if (assetName == null)
            {
                throw new ArgumentNullException(nameof(assetName));
            }

            if (assetExtension == null)
            {
                throw new ArgumentNullException(nameof(assetExtension));
            }

            var paths = AssetDatabase.FindAssets(assetName).Select(AssetDatabase.GUIDToAssetPath).ToList();
            foreach (var path in paths)
            {
                string filename = Path.GetFileNameWithoutExtension(path);
                string extension = Path.GetExtension(path);

                if (filename == assetName && extension == assetExtension)
                {
                    // looks like "Assets/Scripts/Toto.cs"
                    return path;
                }
            }

            throw new InvalidOperationException($"Unable to find '{assetName}.{assetExtension}', results were: '{string.Join(", ", paths)}' ({paths.Count} results).");
        }

        private static string GetAssetFullPath(string assetRelativePath)
        {
            if (assetRelativePath == null)
            {
                throw new ArgumentNullException(nameof(assetRelativePath));
            }

            string root = Application.dataPath; // looks like "C:/Users/Foo/Bar/MyProject/Assets"
            string subRelativePath = assetRelativePath.Substring("Assets/".Length);
            string fullPath = Path.Combine(root, subRelativePath);
            return fullPath;
        }
    }
}