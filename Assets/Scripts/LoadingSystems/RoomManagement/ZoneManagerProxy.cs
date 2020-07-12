using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public class ZoneManagerProxy : MonoBehaviour
    {
        // -- Inspector

        // -- Class

        private ZoneManager _zoneManager;

        void Awake()
        {
            _zoneManager = GameObject.FindObjectOfType<ZoneManager>();
            if (_zoneManager == null)
            {
                Debug.LogError($"Unable to find {nameof(ZoneManager)} in hierarchy. Doors won't open.");
            }
        }

        public void Register(Door door)
        {
            _zoneManager?.Register(door);
        }

        public void Unregister(Door door)
        {
            _zoneManager?.Unregister(door);
        }
    }
}