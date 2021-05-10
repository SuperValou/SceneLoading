using UnityEngine;

namespace Assets.Scripts.LoadingSystems.SceneInfos.Attributes
{
    public class RestrictedSceneIdAttribute : PropertyAttribute
    {
        public SceneType SceneIdType { get; }

        public RestrictedSceneIdAttribute(SceneType sceneIdType)
        {
            SceneIdType = sceneIdType;
        }
    }
}