using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(RestrictedSceneIdAttribute))]
    public class SceneIdDrawer : PropertyDrawer
    {
        private List<int> _optionIds;
        private GUIContent[] _displayedOptions;

        private int _selectedIndex;

        private bool _initialized;

        private void Initialize()
        {
            RestrictedSceneIdAttribute restrictedSceneIdAttribute = (RestrictedSceneIdAttribute)this.attribute;
            var sceneInfos = SceneInfo.GetAll().Where(si => si.Type == restrictedSceneIdAttribute.SceneIdType).ToList();
            _optionIds = sceneInfos.Select(ri => (int)ri.Id).ToList();
            _displayedOptions = sceneInfos.Select(si =>
            {
                string displayedName = $"{si.SceneName}\t[{si.Type.ToString()} id={(int)si.Id}]";
                return new GUIContent(displayedName);
            }).ToArray();

            _initialized = true;
        }

        // Here you can define the GUI for your property drawer. Called by Unity.
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_initialized)
            {
                Initialize();
            }

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