using System;
using Assets.Scripts.LoadingSystems.DoorManagement;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    public abstract class Door : MonoBehaviour, IDoor
    {
        // -- Editor

        [Header("Values")]
        public SceneId roomOnTheOtherSide;
        public string triggeringTag = "Player";
        
        [Header("References")]
        public DoorManagerProxy doorManagerProxy;

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
            doorManagerProxy.Register(door: this);
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
            State = DoorState.Open;
            OnOpen();
        }
        
        public void CloseInSync()
        {
            if (State != DoorState.Open)
            {
                throw new InvalidOperationException($"Door '{name}' is in state '{State}' " +
                                                    $"and was not expected to close.");
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
            doorManagerProxy.Unregister(door: this);
        }
    }
}