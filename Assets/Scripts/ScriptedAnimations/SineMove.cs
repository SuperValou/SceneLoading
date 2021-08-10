using UnityEngine;

namespace Assets.Scripts.ScriptedAnimations
{
    public class SineMove : MonoBehaviour
    {
        [Tooltip("Coordinates of the destination relative to the object's initial position")]
        public Vector3 destination = Vector3.right;

        [Tooltip("Time in seconds to go and come back to the starting point. The target position will be reached in half that time.")]
        public float timeToLoop = 1;
        
        private Vector3 _startPosition;
        private Vector3 _endPosition;

        void Start()
        {
            _startPosition = this.transform.position;
            _endPosition = this.transform.position + destination;
        }

        void Update()
        {
            if (timeToLoop == 0)
            {
                return;
            }

            float cycleValue = Time.time * 2f * Mathf.PI / timeToLoop;
            float time = (Mathf.Sin(cycleValue) + 1f) / 2f;

            float x = Mathf.Lerp(_startPosition.x, _endPosition.x, time);
            float y = Mathf.Lerp(_startPosition.y, _endPosition.y, time);
            float z = Mathf.Lerp(_startPosition.z, _endPosition.z, time);

            this.transform.position = new Vector3(x, y, z);
        }
    }
}