using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Assets.Editor
{
    public class CopyLightingSettings
    {
        private SerializedObject _sourceLightmapSettings;
        private SerializedObject _sourceRenderSettings;
        private string _sourceSunName = string.Empty;

        public void CopySettings()
        {
            if (!TryGetSettings(typeof(LightmapEditorSettings), "GetLightmapSettings", out var lightmapSettings))
            {
                return;
            }

            if (!TryGetSettings(typeof(RenderSettings), "GetRenderSettings", out var renderSettings))
            {
                return;
            }

            _sourceLightmapSettings = new SerializedObject(lightmapSettings);
            _sourceRenderSettings = new SerializedObject(renderSettings);

            // Get the sun name
            _sourceSunName = "";
            var sunProperty = _sourceRenderSettings.FindProperty("m_Sun");
            if (sunProperty != null && sunProperty.objectReferenceValue != null)
            {
                _sourceSunName = sunProperty.objectReferenceValue.name;
            }
        }
        
        public void PasteSettings()
        {
            if (!TryGetSettings(typeof(LightmapEditorSettings), "GetLightmapSettings", out var lightmapSettings))
                return;

            if (!TryGetSettings(typeof(RenderSettings), "GetRenderSettings", out var renderSettings))
                return;

            CopyInternal(_sourceLightmapSettings, new SerializedObject(lightmapSettings));

            var targetRenderSettings = new SerializedObject(renderSettings);
            CopyInternal(_sourceRenderSettings, targetRenderSettings);

            var sunProperty = targetRenderSettings.FindProperty("m_Sun");
            TryConnectSunSource(sunProperty, _sourceSunName);

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
        
        public void PasteSettingsAll()
        {
            var activeScene = SceneManager.GetActiveScene();
            try
            {
                for (var n = 0; n < SceneManager.sceneCount; ++n)
                {
                    var scene = SceneManager.GetSceneAt(n);
                    if (!scene.IsValid() || !scene.isLoaded)
                        continue;

                    SceneManager.SetActiveScene(scene);

                    PasteSettings();
                }
            }
            finally
            {
                SceneManager.SetActiveScene(activeScene);
            }
        }
        
        public bool CanPaste()
        {
            return _sourceLightmapSettings != null && _sourceRenderSettings != null;
        }

        private void TryConnectSunSource(SerializedProperty sunProperty, string sunName)
        {
            if (sunProperty == null)
                return;

            if (sunProperty.objectReferenceValue != null)
                return; // don't set sun if it's assigned already

            if (string.IsNullOrEmpty(sunName))
                return;

            var activeScene = SceneManager.GetActiveScene();
            Light sunLight = null;

            // Try to find an active sun first
            foreach (var light in Resources.FindObjectsOfTypeAll<Light>())
            {
                if (!light.enabled)
                    continue;

                if (!string.Equals(light.name, sunName, System.StringComparison.OrdinalIgnoreCase))
                    continue;

                if (light.gameObject.scene != activeScene)
                    continue;

                sunLight = light;
                break;
            }

            // If no active sun was found, consider inactive as well
            if (sunLight == null)
            {
                foreach (var light in Resources.FindObjectsOfTypeAll<Light>())
                {
                    if (!string.Equals(light.name, _sourceSunName, System.StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (light.gameObject.scene != activeScene)
                        continue;

                    sunLight = light;
                    break;
                }
            }

            if (sunLight != null)
            {
                sunProperty.objectReferenceValue = sunLight;
                sunProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        private void CopyInternal(SerializedObject source, SerializedObject dest)
        {
            var prop = source.GetIterator();
            while (prop.Next(true))
            {
                var copyProperty = true;
                foreach (var propertyName in new[] { "m_Sun", "m_FileID", "m_PathID", "m_ObjectHideFlags" })
                {
                    if (string.Equals(prop.name, propertyName, StringComparison.Ordinal))
                    {
                        copyProperty = false;
                        break;
                    }
                }

                if (copyProperty)
                {
                    dest.CopyFromSerializedProperty(prop);
                }
            }

            dest.ApplyModifiedProperties();
        }

        private bool TryGetSettings(Type type, string methodName, out Object settings)
        {
            settings = null;

            var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
            if (method == null)
            {
                Debug.LogErrorFormat("CopyLightingSettings: Could not find {0}.{1}", type.Name, methodName);
                return false;
            }

            var value = method.Invoke(null, null) as Object;
            if (value == null)
            {
                Debug.LogErrorFormat("CopyLightingSettings: Could get data from {0}.{1}", type.Name, methodName);
                return false;
            }

            settings = value;
            return true;
        }
    }
}