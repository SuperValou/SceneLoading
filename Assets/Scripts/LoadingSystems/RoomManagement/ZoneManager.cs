using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using Assets.Scripts.LoadingSystems.SceneLoadings.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public class ZoneManager : MonoBehaviour
    {
        // -- Editor

        [Tooltip("Load the gameplay scene?")]
        public bool loadGameplay = true;

        [Tooltip("First room to spawn")]
        public SceneId initialRoom;

        // -- Class

        private readonly ISceneLoadingSystem _sceneLoadingSystem = new SceneLoadingSystem();

        private readonly IDictionary<IDoor, IDoor> _doors = new Dictionary<IDoor, IDoor>();
        private readonly object _lock = new object();

        IEnumerator Start()
        {
            _sceneLoadingSystem.Initialize();
            yield return LoadSceneAsync(initialRoom);

            if (loadGameplay)
            {
                yield return LoadSceneAsync(SceneId.GameplayScene);
            }
        }

        void Update()
        {
            // copy doors to a new dictionnary to avoid thread concurency issues
            var doors = new Dictionary<IDoor, IDoor>();
            lock (_lock)
            {
                foreach (var d in _doors)
                {
                    if (d.Key.State == DoorState.WaitingToOpen
                     || d.Key.State == DoorState.WaitingToClose)
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
                
                // Opening door
                if (door.State == DoorState.WaitingToOpen)
                {
                    if (_sceneLoadingSystem.IsLoaded(door.RoomOnTheOtherSide))
                    {
                        if (doorOnTheOtherSide == null)
                        {
                            // there is no door on the other side!
                            Debug.LogError($"There is no opposite door for '{door}'.");
                            door.CloseInSync();
                            continue;
                        }

                        door.OpenInSync();
                        doorOnTheOtherSide.OpenInSync();
                    }
                    else if (_sceneLoadingSystem.IsLoading(door.RoomOnTheOtherSide, out float progress))
                    {
                        door.NotifyLoadingProgress(progress);
                    }
                    else
                    {
                        _sceneLoadingSystem.Load(door.RoomOnTheOtherSide);
                    }
                }

                // Closing door
                else if (door.State == DoorState.WaitingToClose)
                {
                    if (doorOnTheOtherSide == null)
                    {
                        Debug.LogError($"Door {door} is in state {door.State} " +
                                       $"but there is no corresponding door for it in '{door.RoomOnTheOtherSide}'.");
                        door.CloseInSync();
                    }
                    else
                    {
                        door.CloseInSync();
                        doorOnTheOtherSide.CloseInSync();
                    }
                }
            }
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

        private IEnumerator LoadSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.Load(sceneId);

            var wait = new WaitForEndOfFrame();
            while (!_sceneLoadingSystem.IsLoaded(sceneId))
            {
                yield return wait;
            }
        }
    }
}
