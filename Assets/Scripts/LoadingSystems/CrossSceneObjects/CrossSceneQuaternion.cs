using UnityEngine;

namespace Assets.Scripts.LoadingSystems.CrossSceneObjects
{
    [CreateAssetMenu(fileName = nameof(CrossSceneQuaternion), menuName = nameof(CrossSceneObjects) + "/" + nameof(CrossSceneQuaternion))]
    public class CrossSceneQuaternion : CrossScene<Quaternion>
    {
    }
}