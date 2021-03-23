using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    public class DoorManager : MonoBehaviour
    {
        // -- Editor

        [Tooltip("Max number of rooms loaded at the same time.")]
        public int maxLoadedRooms = 2;

        [Header("References")]
        public SceneLoadingManager sceneLoadingManager;

        // -- Class
        
        private readonly IDictionary<IDoor, IDoor> _doors = new Dictionary<IDoor, IDoor>();
        private readonly object _lock = new object();

        private readonly Queue<SceneId> _roomIdsQueue = new Queue<SceneId>();

        public SceneId PlayerCurrentRoomId { get; private set; } = (SceneId) ~0;

        void Update()
        {
            // copy doors to a new dictionnary to avoid thread concurency issues
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

            // actually handle doors now
            foreach (var kvp in doors)
            {
                IDoor door = kvp.Key;
                IDoor doorOnTheOtherSide = kvp.Value; // can be null

                // Track the room the player is in
                if (door.PlayerIsAround && door.RoomId != PlayerCurrentRoomId)
                {
                    PlayerCurrentRoomId = door.RoomId;
                }

                // Opening door
                if (door.State == DoorState.WaitingToOpen)
                {
                    if (sceneLoadingManager.IsLoaded(door.RoomIdOnTheOtherSide))
                    {
                        if (doorOnTheOtherSide == null)
                        {
                            // there is no door on the other side!
                            Debug.LogError($"There is no opposite door for '{door}'.");
                            door.OpenInSync();
                        }
                        else
                        {
                            door.OpenInSync();
                            doorOnTheOtherSide.OpenInSync();
                        }

                        EnqueueRoom(door.RoomIdOnTheOtherSide);
                    }
                    else if (sceneLoadingManager.IsLoading(door.RoomIdOnTheOtherSide, out float progress))
                    {
                        door.NotifyLoadingProgress(progress);

                        if (sceneLoadingManager.IsReadyToActivate(door.RoomIdOnTheOtherSide))
                        {
                            sceneLoadingManager.Activate(door.RoomIdOnTheOtherSide);
                        }
                    }
                    else
                    {
                        StartCoroutine(sceneLoadingManager.LoadSubSceneAsync(door.RoomIdOnTheOtherSide));
                    }
                }

                // Closing door
                else if (door.State == DoorState.Open 
                     && !door.PlayerIsAround)
                {
                    if (doorOnTheOtherSide == null)
                    {
                        Debug.LogError($"There is no opposite door for '{door}'.");
                        door.CloseInSync();
                    }
                    else if (doorOnTheOtherSide.State == DoorState.Open 
                         && !doorOnTheOtherSide.PlayerIsAround)
                    {
                        door.CloseInSync();
                        doorOnTheOtherSide.CloseInSync();
                    }
                }
            }

            // Unload old rooms
            if (_roomIdsQueue.Count > maxLoadedRooms)
            {
                SceneId roomIdToUnload = _roomIdsQueue.Dequeue();
                if (roomIdToUnload == PlayerCurrentRoomId)
                {
                    EnqueueRoom(roomIdToUnload);
                }
                else
                {
                    sceneLoadingManager.Unload(roomIdToUnload);
                }
            }

            this.name = $"Current room: {PlayerCurrentRoomId.ToString()} ({string.Join(">", _roomIdsQueue.Select(id => id.ToString()))})";
        }

        public void Register(IDoor newDoor)
        {
            if (newDoor == null)
            {
                throw new ArgumentNullException(nameof(newDoor));
            }

            lock(_lock)
            {
                if (_doors.ContainsKey(newDoor))
                {
                    throw new ArgumentException($"{newDoor} is already registered.");
                }

                // The very first door being registered will act as the starting point for the player
                if (_doors.Count == 0)
                {
                    EnqueueRoom(newDoor.RoomId);
                    PlayerCurrentRoomId = newDoor.RoomId;
                }

                // Find the room on the other side if applicable (the room may not be loaded yet)
                IDoor doorOnTheOtherSide = null;
                foreach (var registeredDoor in _doors.Keys)
                {
                    if (newDoor.Position == registeredDoor.Position)
                    {
                        doorOnTheOtherSide = registeredDoor;
                        _doors[registeredDoor] = newDoor; // set the new door as the back side of the registered door
                        break;
                    }
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
        
        private void EnqueueRoom(SceneId roomId)
        {
            // remove the room id if it's already there
            int initialCount = _roomIdsQueue.Count;
            for (int i = 0; i < initialCount; i++)
            {
                SceneId id = _roomIdsQueue.Dequeue();
                if (id == roomId)
                {
                    continue;
                }

                _roomIdsQueue.Enqueue(id);
            }

            // enqueue the room id
            _roomIdsQueue.Enqueue(roomId);
        }
    }
}
