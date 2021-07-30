using Assets.Scripts.LoadingSystems.PersistentVariables;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class TransformTracker : MonoBehaviour
    {
        // -- Inspector
        [Header("Values")]
        public TrackingMode mode;

        public bool positionX = true;
        public bool positionY = true;
        public bool positionZ = true;

        public bool rotationXYZ = true;

        [Header("Parts")]
        public PersistentVector3 position;
        public PersistentQuaternion rotation;

        // -- Class

        void Update()
        {
            if (mode == TrackingMode.WriteToPersistent)
            {
                // write position
                Vector3 positionToWrite = new Vector3
                {
                    x = positionX ? this.transform.position.x : position.Value.x,
                    y = positionY ? this.transform.position.y : position.Value.y,
                    z = positionZ ? this.transform.position.z : position.Value.z
                };

                this.position.Value = positionToWrite;

                // write rotation
                var rotationToWrite = this.rotationXYZ ? this.transform.rotation : rotation.Value;
                this.rotation.Value = rotationToWrite;
            }
            else
            {
                // read position
                Vector3 positionToRead = new Vector3
                {
                    x = positionX ? position.Value.x : this.transform.position.x,
                    y = positionY ? position.Value.y : this.transform.position.y,
                    z = positionZ ? position.Value.z : this.transform.position.z
                };

                this.transform.position = positionToRead;

                // read rotation
                Quaternion rotationToRead = this.rotationXYZ ? this.rotation.Value : this.transform.rotation;
                this.transform.rotation = rotationToRead;
            }
        }

        public enum TrackingMode
        {
            WriteToPersistent,
            ReadFromPersistent
        }
    }
}