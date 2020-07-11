using System;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public class ZoneManagerProxy : MonoBehaviour
    {
        // -- Inspector

        // -- Class

        private ZoneManager _zoneManager;

        void Start()
        {
            _zoneManager = GameObject.FindObjectOfType<ZoneManager>();
            if (_zoneManager == null)
            {
                Debug.LogError($"Unable to find {nameof(ZoneManager)} in hierarchy. Doors won't open.");
            }
        }

        public void Register(RoomDoor roomDoor)
        {
            _zoneManager?.Register(roomDoor);
        }

        public void Unregister(RoomDoor roomDoor)
        {
            _zoneManager?.Unregister(roomDoor);
        }
    }
}