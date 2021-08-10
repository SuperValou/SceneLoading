
using UnityEngine;

namespace Assets.Scripts.ScriptedAnimations
{
    public class Rotator : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")]
        [Tooltip("Degrees per second")]
        public float speed = 360;

        public Vector3 axis = Vector3.forward;

        void Update()
        {
            this.transform.Rotate(axis, Time.deltaTime * speed);
        }
    }
}