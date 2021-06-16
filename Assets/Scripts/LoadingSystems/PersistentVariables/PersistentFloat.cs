using UnityEngine;

namespace Assets.Scripts.LoadingSystems.PersistentVariables
{
    [CreateAssetMenu(fileName = nameof(PersistentFloat), menuName = nameof(LoadingSystems) + "/"
                                                               + nameof(PersistentVariables) + "/"
                                                               + nameof(PersistentFloat))]
    public class PersistentFloat : Persistent<float>
    {
    }
}