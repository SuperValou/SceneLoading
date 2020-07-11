using System;
using Assets.Scripts.LoadingSystems.SceneLoadings.SceneInfos;
using Assets.Scripts.LoadingSystems.Trackings;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public class RoomDoor : MonoBehaviour, IDoor
    {
        // -- Editor

        [Header("Values")]
        public SceneId roomOnTheOtherSide;
        public DoorState initialState = DoorState.Closed;
        public string triggeringTag = "Player";

        [Header("Parts")]
        public GameObject leftWing;
        public GameObject rightWing;
        
        [Header("References")]
        public ZoneManagerProxy zoneManagerProxy;

        // -- Class
        
        private ILoadingTracker _openingTracker;
        private bool _shouldLock = false;

        public DoorState State { get; private set; }
        public SceneId RoomOnTheOtherSide { get; private set; }

        void Start()
        {
            State = initialState;
            RoomOnTheOtherSide = roomOnTheOtherSide;
            zoneManagerProxy.Register(roomDoor: this);
        }

        void OnTriggerEnter(Collider collidingObject)
        {
            if (collidingObject.tag == triggeringTag
                && State == DoorState.Closed)
            {
                State = DoorState.RequestingOpening;
            }
        }

        void OnTriggerExit(Collider collidingObject)
        {
            if (collidingObject.tag == triggeringTag
                && State == DoorState.Opened)
            {
                State = DoorState.Closing;
            }
        }

        void Update()
        {
            // dumb effect
            float dumbEffetFactor = 4f;
            
            // Door is closed but wants to open
            if (State == DoorState.Opening && _openingTracker != null)
            {
                // dumb effect
                leftWing.transform.position += -1f * leftWing.transform.right * _openingTracker.Progress * dumbEffetFactor;
                rightWing.transform.position += rightWing.transform.right * _openingTracker.Progress * dumbEffetFactor;
                
                if (_openingTracker.LoadingIsDone)
                {
                    State = DoorState.Opened;
                    _openingTracker = null;
                }
            }

            // Door is already opened and is currently closing
            else if (State == DoorState.Closing)
            {
                // dumb effect (it doesn't reset position properly due to _progressTracker adding offset at each frame of loading... but that's ok for a prototype)
                leftWing.transform.position += leftWing.transform.right * dumbEffetFactor;
                rightWing.transform.position += -1f * rightWing.transform.right * dumbEffetFactor;

                State = _shouldLock ? DoorState.Locked : DoorState.Closed;
            }

            // Door must unlock
            else if (State == DoorState.Locked && !_shouldLock)
            {
                State = DoorState.Closed;
            }
        }
        
        public void Open(ILoadingTracker openingTracker)
        {
            if (_openingTracker != null || State != DoorState.RequestingOpening)
            {
                throw new InvalidOperationException($"Door is in state '{State}' and was not expecting to open.");
            }

            _openingTracker = openingTracker ?? throw new ArgumentNullException(nameof(openingTracker));
            State = DoorState.Opening;
        }

        public void Lock()
        {
            _shouldLock = true;
        }

        public void Unlock()
        {
            _shouldLock = false;
        }

        void OnDestroy()
        {
            zoneManagerProxy.Unregister(roomDoor: this);
        }
    }
}