using Packages.SceneLoading.Editor.LoadMenuGenerations;
using Packages.SceneLoading.Editor.SceneInfoGenerations;
using UnityEditor;

namespace Packages.SceneLoading.Editor
{
    public static class SceneLoadingCustomMenu
    {
        [MenuItem("SceneLoading/Generate 'Load Scene' Menu", priority = 1)]
        public static void GenerateLoadMenu()
        {
            LoadMenuGeneration.Execute();
        }

        [MenuItem("SceneLoading/Generate SceneInfo Assets", priority = 1)]
        public static void GenerateSceneInfoAssets()
        {
            SceneInfoGeneration.Execute();
        }
    }
}