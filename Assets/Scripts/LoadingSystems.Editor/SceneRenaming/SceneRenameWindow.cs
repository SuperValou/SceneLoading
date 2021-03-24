using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.SceneRenaming
{
    public class SceneRenameWindow : EditorWindow
    {
        private string _sceneNameToRename;

        private string[] _sceneNames;
        private GUIContent[] _displayedOptions;
        private int _selectedIndex = -1;

        void Awake()
        {
            this.titleContent = new GUIContent(nameof(SceneRenameWindow));

            var sceneInfos = SceneInfo.GetAll().ToArray();
            _sceneNames = new string[sceneInfos.Length];
            _displayedOptions = new GUIContent[sceneInfos.Length];
            for (int i = 0; i < sceneInfos.Length; i++)
            {
                var sceneInfo = sceneInfos[i];
                string displayedName = $"{sceneInfo.SceneName}\t[id={(int)sceneInfo.Id}]";
                _sceneNames[i] = sceneInfo.SceneName;
                _displayedOptions[i] = new GUIContent(displayedName);
            }
        }

        void OnGUI()
        {
            GUILayout.Label("Select a room to rename.");

            _selectedIndex = EditorGUILayout.Popup(_selectedIndex, _displayedOptions);

            EditorGUI.BeginDisabledGroup(_selectedIndex < 0 || _selectedIndex >= _sceneNames.Length);

            if (GUILayout.Button("Rename"))
            {
                Debug.Log("Renaming " + _sceneNames[_selectedIndex]);
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}