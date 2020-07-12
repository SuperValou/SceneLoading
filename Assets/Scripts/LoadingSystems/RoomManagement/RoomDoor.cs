using System;
using Assets.Scripts.LoadingSystems.SceneLoadings.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public abstract class RoomDoor : MonoBehaviour, IDoor
    {
        // -- Editor

        [Header("Values")]
        public SceneId roomOnTheOtherSide;
        public string triggeringTag = "Player";
        
        [Header("References")]
        public ZoneManagerProxy zoneManagerProxy;

        // -- Class

        private string _initialName;
        
        private bool _shouldLock = false;

        public DoorState State { get; private set; } = DoorState.Closed;
        public Vector3 Position => this.transform.position;
        public SceneId RoomOnTheOtherSide { get; private set; }
        public bool PlayerIsAround { get; private set; }

        protected virtual void Start()
        {
            _initialName = this.name;
            RoomOnTheOtherSide = roomOnTheOtherSide;
            zoneManagerProxy.Register(roomDoor: this);
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
            this.name = $"{_initialName}-{State.ToString()} (from '{this.gameObject.scene.name}' to '{RoomOnTheOtherSide}')";

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

            // Door is opened and the player wants to close it
            else if (State == DoorState.Opened && !PlayerIsAround)
            {
                State = DoorState.WaitingToClose;
            }
        }

        public void NotifyLoadingProgress(float progress)
        {
            if (State != DoorState.WaitingToOpen)
            {
                throw new InvalidOperationException($"Door '{name}' is in state '{State}' " +
                                                    $"and was not expected to be notified of some loading progess.");
            }

            OnLoading(progress);
        }
        
        public void OpenInSync()
        {
            if (State != DoorState.Closed 
             && State != DoorState.WaitingToOpen)
            {
                throw new InvalidOperationException($"Door '{name}' is in state '{State}' " +
                                                    $"and was not expected to open.");
            }

            // TODO: Use Closing state instead
            State = DoorState.Opened;
            OnOpened();
        }
        
        public void CloseInSync()
        {
            if (State != DoorState.Opened 
             && State != DoorState.WaitingToClose)
            {
                throw new InvalidOperationException($"Door '{name}' is in state '{State}' " +
                                                    $"and was not expected to close.");
            }

            // TODO: Use Closing state instead
            State = DoorState.Closed;
            OnClosed();
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

        protected abstract void OnOpened();

        protected abstract void OnClosed();

        void OnDestroy()
        {
            zoneManagerProxy.Unregister(roomDoor: this);
        }
    }
}