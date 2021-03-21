using System;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.LoadingSystems.SceneInfos;

namespace Assets.Scripts.LoadingSystems.Editor.SceneInfoGenerations
{
    public class SceneInfoDataGenerator
    {
        private readonly Regex _enumMemberReplacementRegex = new Regex(@"[^\w]"); // matches all non-word characters

        private readonly Regex _gameplaySceneRegex = new Regex(@"^\d-Gameplay$", RegexOptions.None); // Example of match: "0-Gameplay"
        
        public string GetEnumMemberName(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                throw new ArgumentException($"{nameof(sceneName)} cannot be null, empty, or whitespace.", nameof(sceneName));
            }

            string enumMemberName = _enumMemberReplacementRegex.Replace(sceneName, "_");
            if (char.IsNumber(enumMemberName, 0))
            {
                return $"_{enumMemberName}";
            }

            return enumMemberName;
        }

        public string GetSceneType(string sceneName)
        {
            string sceneType = string.Empty;
            foreach (var sceneTypeEnumMemberName in Enum.GetNames(typeof(SceneType)).OrderByDescending(n => n))
            {
                if (sceneName.EndsWith(sceneTypeEnumMemberName))
                {
                    sceneType = sceneTypeEnumMemberName;
                }
            }

            return sceneType;
        }
    }

    public class SceneInfoData
    {
        public string SceneName { get; set; }
        public int SceneIdInteger { get; set; }
        public string SceneIdName { get; set; }
        public string SceneTypeName { get; set; }
    }
}