using Assets.Scripts.LoadingSystems.PersistentVariables;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class Tracker : MonoBehaviour
    {
        // -- Editor

        public PersistentVector3 position;
        public PersistentQuaternion rotation;

        // -- Class

        void Update()
        {
            position.Value = this.transform.position;
            rotation.Value = this.transform.rotation;
        }
    }
}