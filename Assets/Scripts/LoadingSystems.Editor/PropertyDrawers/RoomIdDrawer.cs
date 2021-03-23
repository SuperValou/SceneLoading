using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using UnityEditor;

namespace Assets.Scripts.LoadingSystems.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(RoomIdAttribute))]
    public class RoomIdDrawer : SceneIdDrawer
    {
        public RoomIdDrawer() : base(SceneType.Room)
        {
        }
    }
}