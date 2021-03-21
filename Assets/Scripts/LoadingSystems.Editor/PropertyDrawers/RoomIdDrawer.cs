using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(RoomIdAttribute))]
    public class RoomIdDrawer : PropertyDrawer
    {
        private static bool _initialized;
        private static List<int> _optionIds;
        private static GUIContent[] _displayedOptions;

        private int _selectedIndex;

        public RoomIdDrawer() : base()
        {
            if (_initialized)
            {
                return;
            }

            var roomInfos = SceneInfo.GetAll().Where(si => si.IsRoom()).ToList();
            _optionIds = roomInfos.Select(ri => (int) ri.Id).ToList();
            _displayedOptions = roomInfos.Select(roomInfo =>
            {
                string displayedName = $"{roomInfo.SceneName} [{roomInfo.Id}={(int) roomInfo.Id}]";
                return new GUIContent(displayedName);
            }).ToArray();
            
            _initialized = true;
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