using System.IO;
using Packages.SceneLoading.Runtime;
using Packages.SceneLoading.Runtime.Loaders;
using UnityEditor;

namespace Packages.SceneLoading.Editor.CustomEditors
{
    [CustomEditor(typeof(RoomUnloadManager))]
    public class RoomUnloadManagerEditor : UnityEditor.Editor
    {
        private RoomUnloadManager _roomUnloadManager;

        void OnEnable()
        {
            _roomUnloadManager = (RoomUnloadManager) this.target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Room queue");

            if (_roomUnloadManager.Queue.Count == 0)
            {
                EditorGUILayout.LabelField("(empty)");
                return;
            }

            
            for (int i = 0; i < _roomUnloadManager.Queue.Count; i++)
            {
                string scenePath = _roomUnloadManager.Queue[i];
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                if (i == 0)
                {
                    EditorGUILayout.LabelField($"- {sceneName} (next unloaded)");
                }
                else
                {
                    EditorGUILayout.LabelField($"- {sceneName}");
                }
            }
        }
    }
}