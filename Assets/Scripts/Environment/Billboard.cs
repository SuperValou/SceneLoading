using TMPro;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class Billboard : MonoBehaviour
    {
        public TextMeshPro textMesh;

        void Update()
        {
            textMesh.text = "Player is facing " + GetPlayerFacingDirection();
        }

        private string GetPlayerFacingDirection()
        {
            var playerTransform = GetPlayerTransform();
            Vector3 direction = Vector3.ProjectOnPlane(playerTransform.forward, Vector3.up).normalized;
            float angle = Vector3.Angle(direction, Vector3.forward);
            if (angle <= 45)
            {
                return "north";
            }

            if (angle >= 135)
            {
                return "south";
            }

            float dot = Vector3.Dot(direction, Vector3.right);
            if (dot > 0)
            {
                return "east";
            }

            return "west";
        }

        private Transform GetPlayerTransform()
        {
            // TODO
            return this.transform;
        }

    }
}