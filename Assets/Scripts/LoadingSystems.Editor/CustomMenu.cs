using Assets.Scripts.LoadingSystems.Editor.SceneInfoGeneration;
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
    }
}