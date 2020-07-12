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

        private bool _shouldOpen = false;
        private bool _shouldClose = false;

        private bool _shouldLock = false;

        public DoorState State { get; private set; } = DoorState.Closed;
        public Vector3 Position => this.transform.position;
        public SceneId RoomOnTheOtherSide { get; private set; }
        
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

            _shouldOpen = true;
        }

        void OnTriggerExit(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }

            _shouldClose = true;
        }

        protected virtual void Update()
        {
            this.name = $"{_initialName}-{State.ToString()}";

            // Door is locked and the player wants to open it
            if (State == DoorState.Locked && _shouldOpen)
            {
                _shouldOpen = false;
            }

            // Door is closed and the player wants to open it
            else if (State == DoorState.Closed && _shouldOpen)
            {
                State = DoorState.WaitingToOpen;
            }

            // Door is opened and the player wants to close it
            else if (State == DoorState.Opened && _shouldClose)
            {
                State = DoorState.WaitingToClose;
            }

            // Door is closed and must lock
            else if (State == DoorState.Closed && _shouldLock)
            {
                State = DoorState.Locked;
            }

            // Door is locked and must revert back to a normally closed state
            else if (State == DoorState.Locked && !_shouldLock)
            {
                State = DoorState.Closed;
            }
        }

        public void NotifyLoadingProgress(float progress)
        {
            if (State != DoorState.WaitingToOpen)
            {
                throw new InvalidOperationException($"Door {name} is in state '{State}' " +
                                                    $"and was not expected to be notified of some loading progess.");
            }

            OnLoading(progress);
        }
        
        public void OpenInSync()
        {
            if (State != DoorState.Closed && State != DoorState.WaitingToOpen)
            {
                throw new InvalidOperationException($"Door {name} is in state '{State}' " +
                                                    $"and was not expected to open.");
            }

            // TODO: Use Closing state instead
            State = DoorState.Opened;
            _shouldOpen = false;
            OnOpened();
        }
        
        public void CloseInSync()
        {
            if (State != DoorState.Opened && State != DoorState.WaitingToClose)
            {
                throw new InvalidOperationException($"Door {name} is in state '{State}' " +
                                                    $"and was not expected to close.");
            }

            // TODO: Use Closing state instead
            State = DoorState.Closed;
            _shouldClose = false;
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