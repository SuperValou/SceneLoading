using UnityEngine;

namespace Assets.Scripts.LoadingSystems.PersistentVariables
{
    [CreateAssetMenu(fileName = nameof(PersistentQuaternion), menuName = nameof(LoadingSystems) + "/"
                                                                     + nameof(PersistentVariables) + "/"
                                                                     + nameof(PersistentQuaternion))]
    public class PersistentQuaternion : Persistent<Quaternion>
    {
    }
}