using Assets.Scripts.LoadingSystems.Editor.LoadSceneGeneration;
using Assets.Scripts.LoadingSystems.Editor.SceneInfoGeneration;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor
{
    public static partial class CustomMenu
    {
        [MenuItem("SceneLoading/Generate " + nameof(SceneInfo))]
        public static void GenerateSceneInfo()
        {
            SceneInfoGenerator.Execute();
        }

        [MenuItem("SceneLoading/Load Scene/Generate Load Scene menu", priority = 0)]
        public static void GenerateLoadMenu()
        {
            LoadSceneGenerator.Execute();
        }
    }
}