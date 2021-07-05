using UnityEngine;

namespace Assets.Scripts.LoadingSystems.PersistentVariables
{
    [CreateAssetMenu(fileName = nameof(PersistentInt), menuName = nameof(LoadingSystems) + "/"
                                                             + nameof(PersistentVariables) + "/"
                                                             + nameof(PersistentInt))]
    public class PersistentInt : Persistent<int>
    {
    }
}