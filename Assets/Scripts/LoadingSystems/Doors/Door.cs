using System;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    /// <summary>
    /// Leads to another room.
    /// </summary>
    public abstract class Door : MonoBehaviour, IDoor
    {
        // -- Editor

        [Header("Values")]

        [SerializeField]
        [RestrictedSceneId(SceneType.Room)]
        private SceneId _roomOnTheOtherSide = (SceneId) ~0; // Undefined SceneId (all bits set to 1);

        [SerializeField]
        private DoorState _state = DoorState.Closed;

        public string triggeringTag = "Player";
        
        [Header("References")]
        public DoorPairing doorPairing;

        // -- Class

        private bool _shouldLock = false;
        
        public SceneId RoomId { get; private set; } = (SceneId) ~0; // Undefined SceneId (all bits set to 1)
        public SceneId RoomIdOnTheOtherSide => _roomOnTheOtherSide;

        public DoorState State
        {
            get => _state;
            private set => _state = value;
        }

        public Vector3 Position => this.transform.position;
        
        public bool PlayerIsAround { get; private set; }

        protected virtual void Awake()
        {
            RoomId = SceneInfo.GetRoomIdForGameObject(this.gameObject);

            var sceneInfo = SceneInfo.GetOrThrow(_roomOnTheOtherSide);
            if (!sceneInfo.IsRoom())
            {
                throw new ArgumentException($"Room id '{_roomOnTheOtherSide}' on the other side of '{gameObject.name}' ({this.GetType().Name}) is not actually a Room. " +
                                            $"Are you sure you selected a valid room id?");
            }

            doorPairing.Register(newDoor: this);
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
            doorPairing.Unregister(doorToRemove: this);
        }

# if UNITY_EDITOR
        // Called in the editor only when the script is loaded or a value is changed in the Inspector
        void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy)
            {
                return;
            }

            if (SceneInfo.TryGet(_roomOnTheOtherSide, out var sceneInfo))
            {
                this.name = $"{this.GetType().Name} to {sceneInfo.SceneName}";
            }
            else
            {
                this.name = $"[ERROR] Door to invalid destination: {_roomOnTheOtherSide}";
            }
        }
#endif

    }
}