using System;
using Packages.SceneLoading.Runtime.SceneInfos;
using Packages.UniKit.Runtime.Attributes;
using Packages.UniKit.Runtime.Extensions;
using UnityEngine;

namespace Packages.SceneLoading.Runtime.Doors
{
    /// <summary>
    /// ScriptableObject linking two rooms together.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(DoorPairing),
        menuName = "SceneLoading/Create Door Pairing")]
    public class DoorPairing : ScriptableObject
    {
        [Header("External")]
        [SerializeField]
        [Tooltip("The room on one side.")]
        private RoomInfo _front = default ;

        [SerializeField]
        [Tooltip("The room on the other side.")]
        private RoomInfo _back = default ;

        [Header("Debug")]
        [SerializeField]
        [ReadOnlyField]
        private DoorState _state = DoorState.Closed;

        public DoorState State => _state;
        
        /// <summary>
        /// Identifier of the Scene on one side.
        /// </summary>
        public string FrontScenePath => _front?.ScenePath ?? string.Empty;

        /// <summary>
        /// Identifier of the Scene on the other side.
        /// </summary>
        public string BackScenePath => _back?.ScenePath ?? string.Empty;

        /// <summary>
        /// Fired when the state of the doors changes.
        /// </summary>
        public event Action<DoorState> OnStateChanged;

        /// <summary>
        /// Set the state of the doors.
        /// </summary>
        public void SetState(DoorState state)
        {
            if (state == _state)
            {
                return;
            }

            _state = state;
            OnStateChanged.SafeInvoke(_state);
        }

        void OnEnable()
        {
            _state = DoorState.Closed;
        }

        void OnDisable()
        {
            OnStateChanged = null;
        }
    }
}