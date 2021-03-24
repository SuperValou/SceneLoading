using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.LoadingSystems.Editor.SceneRenaming
{
    public class UiElementsSceneRenameWindow : EditorWindow
    {
        [MenuItem("Rooms/UIElementsTest")]
        public static void ShowExample()
        {
            UiElementsSceneRenameWindow window = GetWindow<UiElementsSceneRenameWindow>();
            window.minSize = new Vector2(400, 200);
            window.titleContent = new GUIContent("Example");
        }

        void OnEnable()
        {
            var root = this.rootVisualElement;
            
            // Note: PopupField has no UXML support because it is a generic type.
            var choices = SceneInfo.GetAll().ToList();

            // Create a new field and assign it its value.
            var myField = new PopupField<SceneInfo>("Choose", choices, defaultIndex: 0, formatListItemCallback:FormatDisplayedOption);
            root.Add(myField);

            // Mirror value of uxml field into the C# field.
            myField.RegisterCallback<ChangeEvent<SceneInfo>>(OnOptionSelected);
        }

        private string FormatDisplayedOption(SceneInfo arg)
        {
            return $"{arg.SceneName} ({(int) arg.Id})";
        }

        private void OnOptionSelected(ChangeEvent<SceneInfo> evt)
        {
            Debug.Log($"{evt.previousValue} -> {evt.newValue}");
        }
    }
}