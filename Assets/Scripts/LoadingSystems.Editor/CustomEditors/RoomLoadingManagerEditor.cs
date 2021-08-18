using System.Text;
using Assets.Scripts.LoadingSystems.Rooms;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;

namespace Assets.Scripts.LoadingSystems.Editor.CustomEditors
{
    [CustomEditor(typeof(RoomLoadingManager))]
    public class RoomLoadingManagerEditor : UnityEditor.Editor
    {
        private RoomLoadingManager _roomLoadingManager;

        void OnEnable()
        {
            _roomLoadingManager = (RoomLoadingManager) this.target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_roomLoadingManager.Queue.Count == 0)
            {
                return;
            }

            EditorGUILayout.Separator();

            StringBuilder builder = new StringBuilder("[Freshly loaded] ");
            builder.Append(string.Join(" > ", _roomLoadingManager.Queue));
            builder.Append(" [Next unloaded]");

            EditorGUILayout.LabelField(builder.ToString());
        }
    }
}