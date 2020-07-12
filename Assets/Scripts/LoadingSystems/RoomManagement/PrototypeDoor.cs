using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public class PrototypeDoor : RoomDoor
    {
        [Header("Parts")]
        public float openingGap = 2f;

        [Header("Parts")]
        public GameObject leftWing;
        public GameObject rightWing;

        protected override void OnLoading(float progress)
        {
            // discard
        }

        protected override void OnOpened()
        {
            leftWing.transform.position += leftWing.transform.right * openingGap;
            rightWing.transform.position += -1f * rightWing.transform.right * openingGap;
        }

        protected override void OnClosed()
        {
            leftWing.transform.position += -1f * leftWing.transform.right * openingGap;
            rightWing.transform.position += rightWing.transform.right * openingGap;
        }
    }
}