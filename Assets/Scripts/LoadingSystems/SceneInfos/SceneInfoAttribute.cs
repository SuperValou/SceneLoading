using System;

namespace Assets.Scripts.LoadingSystems.SceneInfos
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class SceneInfoAttribute : Attribute
    {
        public string SceneName { get; }
        public SceneType SceneType { get; }

        public SceneInfoAttribute(string sceneName, SceneType sceneType)
        {
            SceneName = sceneName;
            SceneType = sceneType;
        }
    }
}