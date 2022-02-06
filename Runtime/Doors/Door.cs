using System;
using System.Collections;
using Packages.UniKit.Runtime.Attributes;
using Packages.UniKit.Runtime.PersistentVariables;
using UnityEngine;
using UnityEngine.Events;

namespace Packages.SceneLoading.Runtime.Doors
{
    /// <summary>
    /// Leads to another room. Will synchronize with the corresponding <see cref="Door"/> in the other room.
    /// </summary>
    public class Door : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        [SerializeField]
        [Tooltip("Scriptable object pairing this room to the room the " + nameof(Door) + " leads to.")]
        private DoorPairing _doorPairing = default;

        [Header("External")]
        [Tooltip("Scriptable object tracking the room the player is in.")]
        public PersistentString playerCurrentRoom;

        /// <summary>
        /// Fired when the <see cref="Door"/> opens.
        /// </summary>
        [Header("Events")]
        public UnityEvent onOpen;

        /// <summary>
        /// Fired when the <see cref="Door"/> closes.
        /// </summary>
        public UnityEvent onClose;
        
        [Header("Debug")]
        
        [SerializeField]
        [ReadOnlyField]
        private bool _playerIsAround;

        // -- Class

        IEnumerator Start()
        {
            string scenePath = this.gameObject.scene.path;
            if (scenePath != _doorPairing.FrontScenePath && scenePath != _doorPairing.BackScenePath)
            {
                throw new ArgumentException($"{nameof(_doorPairing)} seems incorrectly configured. " +
                                            $"The current room is '{this.gameObject.scene.path}' " +
                                            $"but {nameof(_doorPairing)} knows about '{_doorPairing.FrontScenePath}' " +
                                            $"and '{_doorPairing.BackScenePath}'.");
            }
            
            _doorPairing.OnStateChanged += OnStateChanged;

            yield return null; // Wait for the rest of this gameobject to Start() properly

            if (_doorPairing.State == DoorState.WaitingToOpen)
            {
                // We are the other side of the door, and our room just got loaded
                _doorPairing.SetState(DoorState.Open);
            }
        }

        public void RequestOpen()
        {
            _playerIsAround = true;
            playerCurrentRoom.Set(this.gameObject.scene.path);

            if (_doorPairing.State == DoorState.Closed)
            {
                // Player is on our side and wants to open the door
                _doorPairing.SetState(DoorState.WaitingToOpen);
            }
        }

        public void RequestClose()
        {
            _playerIsAround = false;

            if (playerCurrentRoom.Value == this.gameObject.scene.path)
            {
                // Player moved away in our room
                _doorPairing.SetState(DoorState.Closed);
            }
        }

        private void OnStateChanged(DoorState newState)
        {
            switch (newState)
            {
                case DoorState.WaitingToOpen:
                    if (!_playerIsAround)
                    {
                        // Door wants to open and we are the other side,
                        // so this door can be opened now
                        _doorPairing.SetState(DoorState.Open);
                    }

                    break;

                case DoorState.Closed:
                    onClose.Invoke();
                    break;

                case DoorState.Open:
                    onOpen.Invoke();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        void OnDestroy()
        {
            _doorPairing.OnStateChanged -= OnStateChanged;
        }

#if UNITY_EDITOR
        // Called in the editor only when the script is loaded or a value is changed in the Inspector
        void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy || _doorPairing == null)
            {
                return;
            }

            bool isFront = this.gameObject.scene.path == _doorPairing.FrontScenePath;
            var otherSideScenePath =  isFront ? _doorPairing.BackScenePath
                                              : _doorPairing.FrontScenePath;
            if (string.IsNullOrEmpty(otherSideScenePath))
            {
                this.name = "[ERROR] Door to nowhere!";
            }
            else
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(otherSideScenePath);
                this.name = $"Door to {sceneName}";
            }
            
        }
#endif
    }
}