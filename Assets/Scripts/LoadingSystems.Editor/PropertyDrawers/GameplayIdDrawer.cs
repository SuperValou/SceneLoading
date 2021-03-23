using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using UnityEditor;

namespace Assets.Scripts.LoadingSystems.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(GameplayIdAttribute))]
    public class GameplayIdDrawer : SceneIdDrawer
    {
        public GameplayIdDrawer() : base(SceneType.Gameplay)
        {
        }
    }
}