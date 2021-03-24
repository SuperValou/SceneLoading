using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.SceneRenaming
{
    public class SceneRenameWindow : EditorWindow
    {
        private SceneInfo _sceneToRename = null;

        private SceneInfo[] _sceneInfos;
        private GUIContent[] _displayedOptions;
        private int _selectedIndex = -1;

        private string _newName = string.Empty;

        void Awake()
        {
            this.titleContent = new GUIContent(nameof(SceneRenameWindow));

            _sceneInfos = SceneInfo.GetAll().ToArray();
            _displayedOptions = new GUIContent[_sceneInfos.Length];

            for (int i = 0; i < _sceneInfos.Length; i++)
            {
                var sceneInfo = _sceneInfos[i];
                string displayedName = $"{sceneInfo.SceneName}\t[id={(int)sceneInfo.Id}]";
                _displayedOptions[i] = new GUIContent(displayedName);
            }
        }

        void OnGUI()
        {
            // Scene name selection
            GUILayout.BeginHorizontal();

            GUILayout.Label("Scene to rename");
            _selectedIndex = EditorGUILayout.Popup(_selectedIndex, _displayedOptions);

            GUILayout.EndHorizontal();

            // New name field
            GUILayout.BeginHorizontal();

            GUILayout.Label("New name");
            _newName = GUILayout.TextField(_newName);

            GUILayout.EndHorizontal();

            bool renamingIsAllowed = CanRename();
            EditorGUI.BeginDisabledGroup(!renamingIsAllowed);

            if (renamingIsAllowed)
            {
                // Info
                _sceneToRename = _sceneInfos[_selectedIndex];
                
                if (SceneNamingConvention.MasterSceneRegex.IsMatch(_newName) && _sceneToRename.Type != SceneType.Master)
                {
                    GUILayout.Label($"{_sceneToRename.SceneName} is currently a {_sceneToRename.Type.ToString()} scene. " +
                                    $"After being renamed, it will be matched as a {SceneType.Master} scene.");
                }
            }
            
            // Button
            if (GUILayout.Button("Rename"))
            {
                Debug.Log("Renaming '" + _sceneInfos[_selectedIndex].SceneName + "' to '" + _newName + "'...");
            }

            EditorGUI.EndDisabledGroup();
        }

        private bool CanRename()
        {
            return _selectedIndex >= 0 && _selectedIndex < _sceneInfos.Length;
        }
    }
}