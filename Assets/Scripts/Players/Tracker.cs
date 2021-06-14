using Assets.Scripts.LoadingSystems.CrossSceneObjects;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class Tracker : MonoBehaviour
    {
        // -- Editor

        public CrossSceneVector3 position;
        public CrossSceneQuaternion rotation;

        // -- Class

        void Update()
        {
            position.Value = this.transform.position;
            rotation.Value = this.transform.rotation;
        }
    }
}