using Assets.Scripts.LoadingSystems.Editor.SceneInfoGenerations;
using Assets.Scripts.LoadingSystems.Editor.SceneRenaming;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor
{
    public static class CustomMenu
    {
        [MenuItem("Rooms/Generate " + nameof(SceneInfo))]
        public static void GenerateSceneInfo()
        {
            Debug.Log($"Executing {nameof(GenerateSceneInfo)} script...");
            SceneInfoGenerator.Execute();
            Debug.Log($"Done executing {nameof(GenerateSceneInfo)} script.");
        }

        [MenuItem("Rooms/Rename Scene")]
        public static void RenameScene()
        {
            Debug.Log($"Launching {nameof(RenameScene)} window...");
            UiElementsSceneRenameWindow window = EditorWindow.GetWindow<UiElementsSceneRenameWindow>();
            window.titleContent = new GUIContent(nameof(UiElementsSceneRenameWindow));
            window.Show();
        }
    }
}