using Assets.Scripts.LoadingSystems.Systems;
using Assets.Scripts.LoadingSystems.Systems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public class RoomDoor : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        public SceneId RoomOnTheOtherSide;

        [Header("Parts")]
        public string triggeringTag = "Player";
        
        // -- Class
        
        public bool OpeningRequested { get; private set; }

        void OnTriggerEnter(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }

            OpeningRequested = true;
        }
    }
}