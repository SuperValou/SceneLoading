using UnityEngine;

namespace Assets.Scripts.LoadingSystems.CrossSceneObjects
{
    [CreateAssetMenu(fileName = nameof(CrossSceneVector3), menuName = nameof(CrossSceneObjects) + "/" + nameof(CrossSceneVector3))]
    public class CrossSceneVector3 : CrossScene<Vector3>
    {
    }
}