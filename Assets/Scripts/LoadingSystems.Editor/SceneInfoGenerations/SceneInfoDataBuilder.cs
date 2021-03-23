using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.SceneInfoGenerations
{
    public class SceneInfoDataBuilder
    {
        private const int SceneTypeMultiplier = 10_000;

        private readonly Regex _enumMemberReplacementRegex = new Regex(@"[^\w]"); // matches all non-word characters

        public List<SceneInfoData> Data { get; } = new List<SceneInfoData>();

        private readonly HashSet<string> _remainingNamesToProcess;

        public SceneInfoDataBuilder(ICollection<string> sceneNames)
        {
            if (sceneNames == null)
            {
                throw new ArgumentNullException(nameof(sceneNames));
            }
            
            _remainingNamesToProcess = new HashSet<string>(sceneNames);
        }

        public void Process()
        {
            ProcessUnchangedScenes();
            DiscardIgnoredScenes();
            ProcessNewScenes(SceneNamingConvention.GameplaySceneRegex, SceneType.Gameplay);
            ProcessNewScenes(SceneNamingConvention.MasterSceneRegex, SceneType.Master);
            ProcessNewScenes(SceneNamingConvention.RoomSceneRegex, SceneType.Room);

            if (_remainingNamesToProcess.Any())
            {
                Debug.LogError($"Unhandled scene names (will be ignored): {string.Join(", ", _remainingNamesToProcess)}.");
            }

            foreach (var previousName in SceneInfo.GetAll().Select(si => si.SceneName))
            {
                if (Data.Any(d => d.SceneName == previousName))
                {
                    continue;
                }

                Debug.Log($"Thrown away: {previousName}");
            }
        }
        
        private void ProcessUnchangedScenes()
        {
            var currentInfos = SceneInfo.GetAll();
            foreach (var sceneName in new List<string>(_remainingNamesToProcess))
            {
                var existingInfo = currentInfos.FirstOrDefault(si => si.SceneName == sceneName);
                if (existingInfo == null)
                {
                    continue;
                }

                var data = new SceneInfoData()
                {
                    SceneName = existingInfo.SceneName,
                    SceneEnumMemberName = existingInfo.Id.ToString(),
                    SceneEnumMemberInteger = (int)existingInfo.Id,
                    SceneTypeName = existingInfo.Type.ToString()
                };

                Data.Add(data);
                _remainingNamesToProcess.Remove(sceneName);
            }
        }

        private void ProcessNewScenes(Regex matchingRegex, SceneType sceneType)
        {
            int newNumber = 0;
            foreach (var sceneName in new List<string>(_remainingNamesToProcess))
            {
                var match = matchingRegex.Match(sceneName);
                if (!match.Success)
                {
                    continue;
                }

                string enumMemberName = GetEnumMemberName(sceneName);
                int sceneIdInteger = SceneTypeMultiplier * ((int) sceneType) + newNumber++;

                while (Data.Any(d => d.SceneEnumMemberInteger == sceneIdInteger))
                {
                    sceneIdInteger++;
                    newNumber++;
                }

                var data = new SceneInfoData()
                {
                    SceneName = sceneName,
                    SceneEnumMemberName = enumMemberName,
                    SceneEnumMemberInteger = sceneIdInteger,
                    SceneTypeName = sceneType.ToString()
                };

                Data.Add(data);
                _remainingNamesToProcess.Remove(sceneName);

                Debug.Log($"Found new {sceneType}: {sceneName} (number {sceneIdInteger})");
            }
        }

        private void DiscardIgnoredScenes()
        {
            foreach (var sceneName in new List<string>(_remainingNamesToProcess))
            {
                if (SceneNamingConvention.IgnoredSceneRegex.IsMatch(sceneName))
                {
                    _remainingNamesToProcess.Remove(sceneName);
                }
            }
        }

        private string GetEnumMemberName(string sceneName)
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
    }
}