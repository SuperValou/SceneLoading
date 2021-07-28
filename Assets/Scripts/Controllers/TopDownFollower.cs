using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class TopDownFollower : MonoBehaviour
    {
        // -- Inspector

        public Transform target;

        // -- Class

        void LateUpdate()
        {
            Vector3 newPosition = new Vector3
            {
                x = target.position.x,
                y = this.transform.position.y,
                z = target.position.z
            };

            this.transform.position = newPosition;
        }
    }
}