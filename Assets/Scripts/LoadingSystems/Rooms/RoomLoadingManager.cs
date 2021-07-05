using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.Doors;
using Assets.Scripts.LoadingSystems.PersistentVariables;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Rooms
{
    /// <summary>
    /// Is in charge of loading and unloading rooms.
    /// </summary>
    public class RoomLoadingManager : MonoBehaviour
    {
        // -- Editor

        [Tooltip("Max number of rooms loaded at the same time.")]
        public int maxLoadedRooms = 2;

        [Header("References")]
        public DoorPairing doorPairing;
        public PersistentRoomId playerCurrentRoomId;
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        private readonly ICollection<DoorState> _doorStates = new[] {DoorState.WaitingToOpen, DoorState.Open};

        private readonly Queue<SceneId> _roomIdsQueue = new Queue<SceneId>();

        void Start()
        {
            var doors = doorPairing.GetDoors(_doorStates);
            if (!doors.Any())
            {
                return;
            }

            if (Enum.IsDefined(typeof(SceneId), playerCurrentRoomId.Value))
            {
                _roomIdsQueue.Enqueue(playerCurrentRoomId.Value);
            }
        }

        void Update()
        {
            var doors = doorPairing.GetDoors(_doorStates);
            
            foreach (var kvp in doors)
            {
                IDoor door = kvp.Key;
                IDoor doorOnTheOtherSide = kvp.Value; // can be null
                
                // Opening door
                if (door.State == DoorState.WaitingToOpen)
                {
                    if (sceneLoadingManager.IsLoaded(door.RoomIdOnTheOtherSide))
                    {
                        if (doorOnTheOtherSide == null)
                        {
                            // there is no door on the other side!
                            Debug.LogError($"No back door was found on the other side of '{door}'. " +
                                           $"Are you sure this door is supposed to lead to '{door.RoomIdOnTheOtherSide}'?");
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
                        Debug.LogError($"No back door was found on the other side of '{door}'. " +
                                       $"Are you sure this door is supposed to lead to '{door.RoomIdOnTheOtherSide}'?");
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
                if (roomIdToUnload == playerCurrentRoomId.Value)
                {
                    EnqueueRoom(roomIdToUnload);
                }
                else
                {
                    sceneLoadingManager.Unload(roomIdToUnload);
                }
            }

            // TODO: delete this
            this.name = $"{nameof(RoomLoadingManager)}({string.Join(">", _roomIdsQueue.Select(id => id.ToString()))})";
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
