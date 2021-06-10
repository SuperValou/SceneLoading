using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    [CreateAssetMenu(fileName = nameof(DoorSet), menuName = nameof(LoadingSystems) + "/" + nameof(DoorSet))]
    public class DoorSet : ScriptableObject
    {
        private readonly IDictionary<IDoor, IDoor> _doors = new Dictionary<IDoor, IDoor>();
        private readonly object _lock = new object();

        void OnEnable()
        {
            // do nothing
        }

        public void Register(IDoor newDoor)
        {
            if (newDoor == null)
            {
                throw new ArgumentNullException(nameof(newDoor));
            }

            lock (_lock)
            {
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
                    break;
                }

                // set the registered door (or null if it's not registered yet) as the back side of the new door
                _doors.Add(newDoor, doorOnTheOtherSide);
            }
        }

        public void Unregister(IDoor doorToRemove)
        {
            if (doorToRemove == null)
            {
                throw new ArgumentNullException(nameof(doorToRemove));
            }

            bool removed = false;
            lock (_lock)
            {
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
            }

            if (!removed)
            {
                throw new ArgumentException($"{doorToRemove} was not registered in the first place.");
            }
        }

        public IDictionary<IDoor, IDoor> GetDoors(ICollection<DoorState> doorStates)
        {
            var doors = new Dictionary<IDoor, IDoor>();
            lock (_lock)
            {
                foreach (var d in _doors)
                {
                    if (d.Key.State == DoorState.WaitingToOpen
                        || d.Key.State == DoorState.Open)
                    {
                        doors.Add(d.Key, d.Value);
                    }
                }
            }

            return doors;
        }

        void OnDisable()
        {
            lock (_lock)
            {
                if (_doors.Count != 0)
                {
                    Debug.LogError($"{this.name} is being disabled, but {_doors.Count} door(s) are still referenced. " +
                                   $"These doors are in {string.Join(", ", _doors.Keys.Select(d => d.RoomId).Distinct())}. " +
                                   $"Did you forgot some calls to the {nameof(Unregister)} method?");
                }
            }
        }
    }
}