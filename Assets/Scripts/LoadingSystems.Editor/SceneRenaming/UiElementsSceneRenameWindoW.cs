using System;
using System.IO;
using System.Linq;
using Assets.Scripts.LoadingSystems.Editor.SceneInfoGenerations;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Assets.Scripts.LoadingSystems.Editor.SceneRenaming
{
    public class UiElementsSceneRenameWindow : EditorWindow
    {
        private readonly char[] _invalidChars;

        private PopupField<SceneInfo> _popupField;
        private TextField _newNameField;
        private Label _idLabel;
        private TextField _warningLabel;
        private Button _renameButton;

        public UiElementsSceneRenameWindow()
        {
            _invalidChars = Path.GetInvalidFileNameChars();
        }

        void OnEnable()
        {
            //var bindableModel = new SerializedObject(_model);
            
            var root = this.rootVisualElement;
            
            var sceneInfos = SceneInfo.GetAll().ToList();

            // Popup
            _popupField = new PopupField<SceneInfo>("Scene to rename", sceneInfos, defaultIndex: 0, formatListItemCallback: ToDisplayString, formatSelectedValueCallback: ToDisplayString);
            _popupField.RegisterCallback<ChangeEvent<SceneInfo>>(OnOptionSelected);
            
            root.Add(_popupField);

            // Info
            _idLabel = new Label(string.Empty);

            var idGroup = new VisualElement
            {
                style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row) }
            };

            idGroup.Add(new Label("Id:"));
            idGroup.Add(_idLabel);

            root.Add(idGroup);

            // New name field
            _newNameField = new TextField("New scene name")
            {
                value = string.Empty,
                multiline = false
            };

            _newNameField.RegisterCallback<ChangeEvent<string>>(OnInputChanged);
            root.Add(_newNameField);

            // Warning label
            _warningLabel = new TextField(string.Empty)
            {
                multiline = true,
                focusable = false,
            };
            _warningLabel.SetEnabled(false);
            _warningLabel.style.display = DisplayStyle.None;
            
            root.Add(_warningLabel);

            // Rename button
            _renameButton = new Button(OnRenameButtonClick)
            {
                text = "Rename"
            };

            root.Add(_renameButton);

            Refresh();
        }

        private void OnOptionSelected(ChangeEvent<SceneInfo> evt)
        {
            Refresh();
        }

        private void OnInputChanged(ChangeEvent<string> evt)
        {
            Refresh();
        }

        private void OnRenameButtonClick()
        {
            throw new System.NotImplementedException();
        }

        private void Refresh()
        {
            var sceneToRename = _popupField.value;
            _idLabel.text = ((int) sceneToRename.Id).ToString();

            string newName = _newNameField.text;
            if (string.IsNullOrWhiteSpace(newName)
             || newName.Any(c => _invalidChars.Contains(c)))
            {
                _renameButton.SetEnabled(false);
                return;
            }

            _renameButton.SetEnabled(true);

            var sceneNames = SceneInfo.GetAll().Select(si => si.SceneName).ToList();
            sceneNames.Remove(sceneToRename.SceneName);
            sceneNames.Add(newName);
            
            throw new NotImplementedException();
            //var dataBuilder = new SceneDataGenerator(sceneNames);
            //dataBuilder.Process();
            //var newData = dataBuilder.Data.First(d => d.SceneName == newName);

            //_warningLabel.style.display = DisplayStyle.Flex;
            //if (newData.SceneTypeName != sceneToRename.Type.ToString())
            //{
            //    _warningLabel.SetValueWithoutNotify($"'{sceneToRename.SceneName}' is currently a {sceneToRename.Type.ToString()} scene, with id '{(int) sceneToRename.Id}'.\n" +
            //                                        $"After being renamed, it will be matched as a {newData.SceneTypeName} scene, with id '{newData.SceneEnumMemberInteger}'.");
            //}
            //else if (newData.SceneEnumMemberInteger != (int) sceneToRename.Id)
            //{
            //    _warningLabel.SetValueWithoutNotify(
            //        $"'{sceneToRename.SceneName}' currently has the '{((int) sceneToRename.Id).ToString()}' id.\n" +
            //        $"After being renamed, it will hold the {newData.SceneEnumMemberInteger} id.");
            //}
            //else
            //{
            //    _warningLabel.SetValueWithoutNotify(string.Empty);
            //    _warningLabel.style.display = DisplayStyle.None;
            //}
        }

        private string ToDisplayString(SceneInfo sceneInfo)
        {
            return $"\"{sceneInfo.SceneName}\"";
        }
    }
}