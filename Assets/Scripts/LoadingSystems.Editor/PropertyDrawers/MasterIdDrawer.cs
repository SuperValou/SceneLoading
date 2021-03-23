using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using UnityEditor;

namespace Assets.Scripts.LoadingSystems.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(MasterIdAttribute))]
    public class MasterIdDrawer : SceneIdDrawer
    {
        public MasterIdDrawer() : base(SceneType.Master)
        {
        }
    }
}