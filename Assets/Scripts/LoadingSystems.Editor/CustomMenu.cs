using Assets.Scripts.LoadingSystems.Editor.SceneInfoGenerations;
using Assets.Scripts.LoadingSystems.Editor.SceneRenaming;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor
{
    public static class CustomMenu
    {
        [MenuItem("Rooms/Generate " + nameof(SceneId) + " enumeration")]
        public static void GenerateEnumeration()
        {
            Debug.Log($"Executing {nameof(GenerateEnumeration)} script...");
            SceneInfoGenerator.Execute();
            Debug.Log($"Done executing {nameof(GenerateEnumeration)} script.");
        }

        [MenuItem("Rooms/Rename Scene")]
        public static void RenameScene()
        {
            Debug.Log($"Launching {nameof(RenameScene)} window...");
            EditorWindow.GetWindow<SceneRenameWindow>();
        }
    }
}