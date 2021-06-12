using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    /// <summary>
    /// Minimal interface of a door.
    /// </summary>
    public interface IDoor
    {
        /// <summary>
        /// The room the door is in.
        /// </summary>
        SceneId RoomId { get; }

        /// <summary>
        /// The room the door leads to.
        /// </summary>
        SceneId RoomIdOnTheOtherSide { get; }

        /// <summary>
        /// The current <see cref="DoorState"/> the door is in.
        /// </summary>
        DoorState State { get; }

        /// <summary>
        /// The world position of the door.
        /// </summary>
        Vector3 Position { get; }
        
        /// <summary>
        /// True when the player wants the door to open.
        /// </summary>
        bool PlayerIsAround { get; }
        
        /// <summary>
        /// Notifies the door about the loading of the room behind it.
        /// </summary>
        /// <param name="progress"></param>
        void NotifyLoadingProgress(float progress);

        /// <summary>
        /// Opens the door. The corresponding opposite door will also open.
        /// </summary>
        void OpenInSync();

        /// <summary>
        /// Closes the door. The corresponding opposite door will also close.
        /// </summary>
        void CloseInSync();

        /// <summary>
        /// Requests the door to be <see cref="DoorState.Locked"/>.
        /// A locked door can be closed, but cannot be open afterwards (unless <see cref="Unlock"/> is called).
        /// </summary>
        void Lock();

        /// <summary>
        /// Allows a door to be open again.
        /// </summary>
        void Unlock();
    }
}