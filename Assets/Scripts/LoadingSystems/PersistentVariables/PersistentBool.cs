using UnityEngine;

namespace Assets.Scripts.LoadingSystems.PersistentVariables
{
    [CreateAssetMenu(fileName = nameof(PersistentBool), menuName = nameof(LoadingSystems) + "/"
                                                              + nameof(PersistentVariables) + "/"
                                                              + nameof(PersistentBool))]
    public class PersistentBool : Persistent<bool>
    {
    }
}