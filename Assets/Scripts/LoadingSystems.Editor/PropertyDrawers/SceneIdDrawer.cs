using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.PropertyDrawers
{
    public class SceneIdDrawer : PropertyDrawer
    {
        private readonly List<int> _optionIds;
        private readonly GUIContent[] _displayedOptions;

        private int _selectedIndex;

        protected SceneIdDrawer(SceneType type)
        {
            var sceneInfos = SceneInfo.GetAll().Where(si => si.Type == type).ToList();
            _optionIds = sceneInfos.Select(ri => (int) ri.Id).ToList();
            _displayedOptions = sceneInfos.Select(si =>
            {
                string displayedName = $"{si.SceneName}\t[id={(int) si.Id}]";
                return new GUIContent(displayedName);
            }).ToArray();
        }

        // Here you can define the GUI for your property drawer. Called by Unity.
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int previousSelectedIndex = _optionIds.FindIndex(id => id == property.intValue);
            var guiContent = new GUIContent(property.displayName);
            int newSelectedIndex = EditorGUI.Popup(position, guiContent, previousSelectedIndex, _displayedOptions.Select(d => new GUIContent(d)).ToArray() );

            if (newSelectedIndex >= 0 && newSelectedIndex < _optionIds.Count)
            {
                property.intValue = _optionIds[newSelectedIndex];
            }
        }
    }
}