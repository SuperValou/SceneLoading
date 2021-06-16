using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    /// <summary>
    /// Is in charge of pairing <see cref="Door"/>s from different scenes together.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(DoorPairing), menuName = nameof(LoadingSystems) + "/" + nameof(DoorPairing))]
    public class DoorPairing : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField]
        private List<string> _debug = new List<string>();

#endif
        private readonly IDictionary<IDoor, IDoor> _doors = new Dictionary<IDoor, IDoor>();

        /// <summary>
        /// Register the given <see cref="Door"/>. Attempts to pair it with one of the other registered doors.
        /// </summary>
        /// <param name="newDoor">The door to register.</param>
        public void Register(IDoor newDoor)
        {
            if (newDoor == null)
            {
                throw new ArgumentNullException(nameof(newDoor));
            }

            if (_doors.ContainsKey(newDoor))
            {
                throw new ArgumentException($"{newDoor} is already registered.");
            }

            // Find the room on the other side if applicable (the room may not be loaded yet)
            IDoor doorOnTheOtherSide = null;
            foreach (var registeredDoor in _doors.Keys)
            {
                if (newDoor.Position != registeredDoor.Position)
                {
                    continue;
                }

                doorOnTheOtherSide = registeredDoor;
                _doors[registeredDoor] = newDoor; // set the new door as the back side of the registered door

                if (newDoor.RoomIdOnTheOtherSide != doorOnTheOtherSide.RoomId
                    || newDoor.RoomId != doorOnTheOtherSide.RoomIdOnTheOtherSide)
                {
                    Debug.LogError("Destinations doesn't match between paired doors. " +
                                   $"The door being added is from {newDoor.RoomId} and leads to {newDoor.RoomIdOnTheOtherSide}, " +
                                   $"but its corresponding back door is from {doorOnTheOtherSide.RoomId} and leads to {doorOnTheOtherSide.RoomIdOnTheOtherSide}.");
                }

                break;
            }

            // set the registered door (or null if it's not registered yet) as the back side of the new door
            _doors.Add(newDoor, doorOnTheOtherSide);

#if UNITY_EDITOR
            _debug.Clear();
            foreach (var kvp in _doors)
            {
                _debug.Add($"{kvp.Key.RoomId} => {kvp.Value?.RoomId}");
            }
#endif
        }

        /// <summary>
        /// Unregister the given <see cref="Door"/>. Unpair it from any other registered doors.
        /// </summary>
        /// <param name="doorToRemove"></param>
        public void Unregister(IDoor doorToRemove)
        {
            if (doorToRemove == null)
            {
                throw new ArgumentNullException(nameof(doorToRemove));
            }

            bool removed = false;
            foreach (IDoor door in _doors.Keys.ToList())
            {
                if (door == doorToRemove)
                {
                    _doors.Remove(door);
                    removed = true;
                    continue;
                }

                var doorOnTheOtherSide = _doors[door];
                if (doorOnTheOtherSide == doorToRemove)
                {
                    _doors[door] = null;
                }
            }

            if (!removed)
            {
                throw new ArgumentException($"{doorToRemove} was not registered in the first place.");
            }

#if UNITY_EDITOR
            _debug.Clear();
            foreach (var kvp in _doors)
            {
                _debug.Add($"{kvp.Key.RoomId} => {kvp.Value?.RoomId}");
            }
#endif
        }
        
        public IEnumerable<KeyValuePair<IDoor, IDoor>> GetDoors(ICollection<DoorState> doorStates)
        {
            return _doors.Where(d => doorStates.Contains(d.Key.State));
        }

        void OnDisable()
        {
            if (_doors.Count != 0)
            {
                Debug.LogError($"{this.name} is being disabled, but {_doors.Count} door(s) are still referenced. " +
                               $"These doors are in {string.Join(", ", _doors.Keys.Select(d => d.RoomId).Distinct())}. " +
                               $"Did you forgot some calls to the {nameof(Unregister)} method?");
            }

            _doors.Clear();
        }
    }
}