using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    public class DoorManagerProxy : MonoBehaviour
    {
        // -- Inspector

        // -- Class

        private DoorManager _doorManager;

        void Awake()
        {
            _doorManager = GameObject.FindObjectOfType<DoorManager>();
            if (_doorManager == null)
            {
                Debug.LogError($"Unable to find {nameof(DoorManager)} in hierarchy. Doors won't open.");
            }
        }

        public void Register(Door door)
        {
            _doorManager?.Register(door);
        }

        public void Unregister(Door door)
        {
            _doorManager?.Unregister(door);
        }
    }
}