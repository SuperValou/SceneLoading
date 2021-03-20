using Assets.Scripts.LoadingSystems.Editor.SceneInfoGenerations;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;

namespace Assets.Scripts.LoadingSystems.Editor
{
    public static class CustomMenu
    {
        [MenuItem("Rooms/Create new Room")]
        public static void ShowCreateNewRoomWindow()
        {
            //EditorWindow.GetWindow<ScriptLinkWindow>();
        }

        [MenuItem("Rooms/Generate " + nameof(SceneId) + " enumeration")]
        public static void ShowGenerateEnumerationWindow()
        {
            var toto = new SceneInfoGenerator();
            toto.Generate();
        }
    }
}