using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    public interface IDoor
    {
        SceneId RoomId { get; }
        SceneId RoomIdOnTheOtherSide { get; }
        DoorState State { get; }
        Vector3 Position { get; }
        
        bool PlayerIsAround { get; }

        void NotifyLoadingProgress(float progress);
        void OpenInSync();
        void CloseInSync();

        void Lock();
        void Unlock();
    }
}