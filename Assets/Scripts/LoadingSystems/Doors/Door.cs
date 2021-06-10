using System;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    public abstract class Door : MonoBehaviour, IDoor
    {
        // -- Editor

        [Header("Values")]
        [RestrictedSceneId(SceneType.Room)]
        public SceneId roomOnTheOtherSide;
        public string triggeringTag = "Player";
        
        [Header("References")]
        public DoorSet doorSet;

        // -- Class

        private string _initialName;
        
        private bool _shouldLock = false;
        
        public SceneId RoomId { get; private set; } = (SceneId) ~0; // Undefined SceneId (all bits set to 1)
        public SceneId RoomIdOnTheOtherSide { get; private set; } = (SceneId) ~0; // Undefined SceneId (all bits set to 1)
        public DoorState State { get; private set; } = DoorState.Closed;
        public Vector3 Position => this.transform.position;
        
        public bool PlayerIsAround { get; private set; }

        protected virtual void Start()
        {
            RoomId = SceneInfo.GetRoomIdForGameObject(this.gameObject);

            var sceneInfo = SceneInfo.GetOrThrow(roomOnTheOtherSide);
            if (!sceneInfo.IsRoom())
            {
                throw new ArgumentException($"Room id '{roomOnTheOtherSide}' on the other side of '{gameObject.name}' ({this.GetType().Name}) is not actually a Room. " +
                                            $"Are you sure you selected a valid room id?");
            }

            _initialName = $"Door to {sceneInfo.SceneName}";
            RoomIdOnTheOtherSide = roomOnTheOtherSide;

            doorSet.Register(newDoor: this);
        }
        
        void OnTriggerEnter(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }

            PlayerIsAround = true;
        }

        void OnTriggerExit(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }

            PlayerIsAround = false;
        }

        protected virtual void Update()
        {
            this.name = $"{_initialName} ({State.ToString()})";

            // Door is locked
            if (State == DoorState.Locked)
            {
                if (!_shouldLock)
                {
                    // Unlock door
                    State = DoorState.Closed;
                }
            }

            // Door is closed and must lock
            else if (State == DoorState.Closed && _shouldLock)
            {
                State = DoorState.Locked;
            }

            // Door is closed and the player wants to open it
            else if (State == DoorState.Closed && PlayerIsAround)
            {
                State = DoorState.WaitingToOpen;
            }
        }

        public void NotifyLoadingProgress(float progress)
        {
            if (State != DoorState.WaitingToOpen)
            {
                throw new InvalidOperationException($"Door '{name}' is in state '{State}' " +
                                                    $"and was not expecting to be notified about some loading progess.");
            }

            OnLoading(progress);
        }
        
        public void OpenInSync()
        {
            if (State != DoorState.Closed 
             && State != DoorState.WaitingToOpen)
            {
                throw new InvalidOperationException($"Door '{name}' is in state '{State}' " +
                                                    $"and was not expecting to open.");
            }
            
            State = DoorState.Open;
            OnOpen();
        }
        
        public void CloseInSync()
        {
            if (State != DoorState.Open)
            {
                throw new InvalidOperationException($"Door '{name}' is in state '{State}' " +
                                                    $"and was not expecting to close.");
            }
            
            State = DoorState.Closed;
            OnClose();
        }

        public void Lock()
        {
            _shouldLock = true;
        }

        public void Unlock()
        {
            _shouldLock = false;
        }

        protected abstract void OnLoading(float progress);
        protected abstract void OnOpen();
        protected abstract void OnClose();

        void OnDestroy()
        {
            doorSet.Unregister(doorToRemove: this);
        }

        // Called in the editor only when the script is loaded or a value is changed in the Inspector
        void OnValidate()
        {
            if (SceneInfo.TryGet(roomOnTheOtherSide, out var sceneInfo))
            {
                this.name = $"Door to {sceneInfo.SceneName}";
            }
            else
            {
                this.name = $"[ERROR] Door to invalid destination: {roomOnTheOtherSide}";
            }
        }
    }
}