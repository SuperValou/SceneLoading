using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.Doors;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Rooms
{
    public class RoomManager : MonoBehaviour
    {
        // -- Editor

        [Tooltip("Max number of rooms loaded at the same time.")]
        public int maxLoadedRooms = 2;

        [Header("References")]
        public DoorSet doorSet;
        public PlayerCurrentRoom playerCurrentRoom;
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        private readonly ICollection<DoorState> _doorStates = new[] {DoorState.WaitingToOpen, DoorState.Open};

        private readonly Queue<SceneId> _roomIdsQueue = new Queue<SceneId>();

        void Start()
        {
            var doors = doorSet.GetDoors(_doorStates);
            if (doors.Count == 0)
            {
                return;
            }

            if (!Enum.IsDefined(typeof(SceneId), playerCurrentRoom.RoomId))
            {
                // Let's assume the first registered doors are from the room the player is in.
                // It can potentially be the wrong room under certain conditions, but should be correct most of the time.
                // Getting close to any door will correct it anyway.
                playerCurrentRoom.RoomId = doors.First().Key.RoomId;
            }
            
            _roomIdsQueue.Enqueue(playerCurrentRoom.RoomId);
        }

        void Update()
        {
            var doors = doorSet.GetDoors(_doorStates);
            
            foreach (var kvp in doors)
            {
                IDoor door = kvp.Key;
                IDoor doorOnTheOtherSide = kvp.Value; // can be null

                // Track the room the player is in
                if (door.PlayerIsAround && door.RoomId != playerCurrentRoom.RoomId)
                {
                    playerCurrentRoom.RoomId = door.RoomId;
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
                if (roomIdToUnload == playerCurrentRoom.RoomId)
                {
                    EnqueueRoom(roomIdToUnload);
                }
                else
                {
                    sceneLoadingManager.Unload(roomIdToUnload);
                }
            }

            this.name = $"Current room: {playerCurrentRoom.RoomId} ({string.Join(">", _roomIdsQueue.Select(id => id.ToString()))})";
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
