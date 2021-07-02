using Assets.Scripts.LoadingSystems.Editor.LoadMenuGenerations;
using Assets.Scripts.LoadingSystems.Editor.SceneClassesGenerations;
using UnityEditor;

namespace Assets.Scripts.LoadingSystems.Editor
{
    public static partial class CustomMenu
    {
        [MenuItem("SceneLoading/Generate Scene classes", priority = 0)]
        public static void GenerateSceneInfo()
        {
            SceneClassesGeneration.Execute();
        }

        [MenuItem("SceneLoading/Load Scene/Generate Load Scene menu", priority = 1)]
        public static void GenerateLoadMenu()
        {
            LoadMenuGeneration.Execute();
        }
    }
}