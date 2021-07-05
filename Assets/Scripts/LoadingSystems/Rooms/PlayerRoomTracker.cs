using System.Collections.Generic;
using Assets.Scripts.LoadingSystems.Doors;
using Assets.Scripts.LoadingSystems.PersistentVariables;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Rooms
{
    public class PlayerRoomTracker : MonoBehaviour
    {
        // -- Editor

        [Header("References")]
        public DoorPairing doorPairing;
        public PersistentRoomId playerCurrentRoomId;

        // -- Class
        
        private readonly ICollection<DoorState> _relevantStates = new[] {DoorState.Open}; 

        void Update()
        {
            // Update the current room when the player goes through an open door belonging to a different room
            var doors = doorPairing.GetDoors(_relevantStates);
            foreach (var kvp in doors)
            {
                var door = kvp.Key;
                if (door.PlayerIsAround)
                {
                    playerCurrentRoomId.Value = kvp.Key.RoomId;
                    break;
                }
            }
        }
    }
}