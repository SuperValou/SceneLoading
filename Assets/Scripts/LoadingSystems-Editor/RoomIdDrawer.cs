using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    [CustomPropertyDrawer(typeof(RoomIdAttribute))]
    public class RoomIdDrawer : PropertyDrawer
    {
        private List<Tuple<int, string>> _options = new List<Tuple<int, string>>();

        private int _selectedIndex;

        public RoomIdDrawer() : base()
        {
            var roomInfos = SceneInfo.GetAll().Where(si => SceneInfo.IsRoom(si.Id)).ToArray();

            foreach (var roomInfo in roomInfos)
            {
                int id = (int)roomInfo.Id;
                string displayName = $"{roomInfo.SceneName} [{(int) roomInfo.Id}: {roomInfo.Id}]";
                var tuple = new Tuple<int, string>(id, displayName);
                _options.Add(tuple);
            }
        }

        // Here you can define the GUI for your property drawer. Called by Unity.
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int index = _options.FindIndex(tuple => tuple.Item1 == property.intValue);
            EditorGUI.LabelField(position, property.name);
            int selectedIndex = EditorGUI.Popup(position, index, _options.Select(o => o.Item2).ToArray());
            property.intValue = _options[selectedIndex].Item1;
        }
    }
}