using UnityEditor;

namespace Assets.Editor
{
    public class CopyLigthingSettingsMenus
    {
        private const string CopySettingsMenuPath = "Window/Rendering/Copy Lighting Settings";
        private const string PasteSettingsMenuPath = "Window/Rendering/Paste Lighting Settings";
        private const string PasteSettingsAllMenuPath = "Window/Rendering/Paste Lighting Settings in Open Scenes";

        private static readonly CopyLightingSettings CopyLightingSettings = new CopyLightingSettings();

        [MenuItem(CopySettingsMenuPath, priority = 200)]
        public static void CopySettings()
        {
            CopyLightingSettings.CopySettings();
        }

        [MenuItem(PasteSettingsMenuPath, priority = 201)]
        public static void PasteSettings()
        {
            CopyLightingSettings.PasteSettings();
        }

        [MenuItem(PasteSettingsAllMenuPath, priority = 202)]
        public static void PasteSettingsAll()
        {
            CopyLightingSettings.PasteSettingsAll();
        }

        [MenuItem(PasteSettingsAllMenuPath, validate = true)]
        [MenuItem(PasteSettingsMenuPath, validate = true)]
        public static bool PasteValidate()
        {
            return CopyLightingSettings.CanPaste();
        }
    }
}