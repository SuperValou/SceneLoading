using UnityEngine;

namespace Assets.Scripts.LoadingSystems.PersistentVariables
{
    [CreateAssetMenu(fileName = nameof(PersistentVector3), menuName = nameof(LoadingSystems) + "/" 
                                                                 + nameof(PersistentVariables) + "/" 
                                                                 + nameof(PersistentVector3))]
    public class PersistentVector3 : Persistent<Vector3>
    {
    }
}