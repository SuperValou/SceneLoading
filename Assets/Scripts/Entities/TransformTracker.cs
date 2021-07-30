using Assets.Scripts.LoadingSystems.PersistentVariables;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class TransformTracker : MonoBehaviour
    {
        // -- Inspector
        
        public PersistentVector3 position;
        public PersistentQuaternion rotation;

        // -- Class

        void Update()
        {
            this.position.Value = this.transform.position;
            this.rotation.Value = this.transform.rotation;
        }
    }
}